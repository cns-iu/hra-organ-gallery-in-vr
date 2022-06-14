using System.Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class HuBMAPIDFetcher : MonoBehaviour
{
    private Stuff response;

    private void OnEnable()
    {
        SceneBuilder.OnSceneBuilt += FromEntityIdGetHubmapId;
    }

    private void OnDestroy()
    {
        SceneBuilder.OnSceneBuilt -= FromEntityIdGetHubmapId;
    }

    public async void FromEntityIdGetHubmapId()
    {
        TissueBlockData dataComponent = GetComponent<TissueBlockData>();
        string entityId = dataComponent.EntityId;

        // get hubmap id
        if (entityId.Contains("hubmap"))
        {
            response = await Get(entityId);
            dataComponent.HubmapId = response.hubmap_id;
        }
    }

    public async Task<Stuff> Get(string url)
    {
        try
        {
            using var www = UnityWebRequest.Get(url);
            var operation = www.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogError($"Failed: {www.error}");

            var result = www.downloadHandler.text;

            var text = www.downloadHandler.text;
            response = JsonUtility.FromJson<Stuff>(text);
            return response;
        }
        catch (Exception ex)
        {
            Debug.LogError($"{nameof(Get)} failed: {ex.Message}");
            return default;
        }
    }

    public class StuffArray
    {
        [SerializeField] public Stuff[] stuffs;
    }

    public class Stuff
    {
        public string hubmap_id;
    }
}
