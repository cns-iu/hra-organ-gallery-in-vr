using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;


public class GitHubChecker : MonoBehaviour
{
    public static event Action<List<IdTypeMapping>> GitHubCTChecked;

    [SerializeField] private string gitHubUrl;
    [SerializeField] private RecordCollection recordList;
    [SerializeField] private List<IdTypeMapping> mappings;
    [SerializeField] private string entityApiUrl = "https://entity.api.hubmapconsortium.org/entities/";

    private async void Awake()
    {
        await GetGitHubRecords();
        await GetParentIds();
        GitHubCTChecked?.Invoke(mappings);
    }

    async Task GetParentIds()
    {
        for (int i = 0; i < recordList.records.Count; i++)
        {
            var record = recordList.records[i];
            if (record.DataType.ToLower() == "dataset" && record.PublicationStatus.ToLower() == "published")
            {
                string parentId = await Get(record.HubmapId);
                IdTypeMapping mapping = new IdTypeMapping(record.HubmapId, record.DataType, record.PublicationStatus, parentId);
                mappings.Add(mapping);
            }
            else if (record.DataType == "sample")
            {
                string parentId = record.HubmapId;
                IdTypeMapping mapping = new IdTypeMapping(record.HubmapId, record.DataType, record.PublicationStatus, parentId);
                mappings.Add(mapping);
            }
        }
    }

    private async Task<string> Get(string datasetHubmapId)
    {
        string awaitedResult;

        try
        {
            using var www = UnityWebRequest.Get(entityApiUrl + datasetHubmapId);
            var operation = www.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogError($"Failed: {www.error}");

            var result = www.downloadHandler.text;
            EntityApiResponse apiResponse = JsonUtility.FromJson<EntityApiResponse>(result);

            if (apiResponse.direct_ancestors[0].entity_type == "Dataset")
            {
                return awaitedResult = await Get(apiResponse.direct_ancestors[0].hubmap_id);
            }
            else
            {
                return apiResponse.direct_ancestors[0].hubmap_id;
            }

        }
        catch (Exception ex)
        {
            Debug.LogError($"{nameof(Get)} failed: {ex.Message}");
            return default;
        }
    }

    async Task GetGitHubRecords()
    {
        string response = await GetHubmapIds(gitHubUrl);
        var lines = response.Split('\n');

        recordList.records = new List<GitHubRecord>();

        //initialize i to 1 to skip header
        for (int i = 1; i < lines.Length; i++)
        {
            string[] elements = lines[i].Split(",");
            //guarding clause to remove empty line at the end
            if (!(elements.Length > 1)) continue;
            recordList.records.Add(new GitHubRecord(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5]));
        }
    }

    [Serializable]
    struct EntityApiResponse
    {
        [SerializeField] public DirectAncestor[] direct_ancestors;
    }

    [Serializable]
    struct DirectAncestor
    {
        public string entity_type;
        public string hubmap_id;
    }

    [Serializable]
    struct RecordCollection
    {
        [SerializeField] public List<GitHubRecord> records;
    }

    [Serializable]
    private struct GitHubRecord
    {
        [field: SerializeField] public string number { get; set; }
        [field: SerializeField] public string HubmapId { get; set; }
        [field: SerializeField] public string Organ { get; set; }
        [field: SerializeField] public string Sex { get; set; }
        [field: SerializeField] public string DataType { get; set; }
        [field: SerializeField] public string PublicationStatus { get; set; }

        public GitHubRecord(string p1, string p2, string p3, string p4, string p5, string p6) => (number, HubmapId, Organ, Sex, DataType, PublicationStatus) = (p1, p2, p3, p4, p5, p6);
    }

    private async Task<string> GetHubmapIds(string url)
    {

        try
        {
            using var www = UnityWebRequest.Get(gitHubUrl);
            var operation = www.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogError($"Failed: {www.error}");

            var result = www.downloadHandler.text;

            return result;
        }
        catch (Exception ex)
        {
            Debug.LogError($"{nameof(GetHubmapIds)} failed: {ex.Message}");
            return default;
        }
    }
}

[Serializable]
public class IdTypeMapping
{
    [SerializeField] public string Id;
    [SerializeField] public string Type;
    [SerializeField] public string ParentId;
    [SerializeField] public string PublicationStatus;

    public IdTypeMapping(string id, string type, string publicationStatus, string parentId = "") => (Id, Type, PublicationStatus, ParentId) = (id, type, publicationStatus, parentId);
}