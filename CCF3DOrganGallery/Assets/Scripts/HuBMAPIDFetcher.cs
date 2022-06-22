using System.Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class HuBMAPIDFetcher : MonoBehaviour
{
    private HubmapIdHolder response;

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

    private async Task<HubmapIdHolder> Get(string url)
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
            response = JsonUtility.FromJson<HubmapIdHolder>(text);
            return response;
        }
        catch (Exception ex)
        {
            Debug.LogError($"{nameof(Get)} failed: {ex.Message}");
            return default;
        }
    }

    private class HubmapIdArray
    {
        [SerializeField] public HubmapIdHolder[] hubmapIdHolder;
    }

    private class HubmapIdHolder
    {
        public string hubmap_id;
    }
}
