using Oculus.Platform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class GitHubChecker : MonoBehaviour
{
    [SerializeField] private string gitHubUrl;
    [SerializeField] private string response;
    [SerializeField] List<IdDataTypePair> idDataTypePairs;

    private async void Awake()
    {
        response = await GetHubmapIds(gitHubUrl);
        var data = CsvReader.Read(response);
        for (int i = 0; i < data.Count; i++)
        {
            //Debug.Log(data[i].TryGetValue("hubmap_id"));
        }
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


    enum DataType { Sample, Dataset, None }

    [Serializable]
    struct IdDataTypePair
    {
        private string id;
        private DataType type;

        public IdDataTypePair(string id, DataType type)
        {
            this.id = id;
            this.type = type;
        }
    }
}
