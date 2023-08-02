using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Data
{
    /// <summary>
    /// This is a more general implementation of DataFetcher. Not tested yet. 
    /// </summary>
    public class WebLoader : MonoBehaviour
    {
        public async Task<string> Get(string url)
        {
            try
            {
                using var www = UnityWebRequest.Get(url);
                var operation = www.SendWebRequest();

                while (!operation.isDone)
                    await Task.Yield();

                if (www.result != UnityWebRequest.Result.Success)
                    Debug.LogError($"Failed: {www.error}");

                return www.downloadHandler.text;
            }
            catch (Exception ex)
            {
                Debug.LogError($"{nameof(Get)} failed: {ex.Message}");
                return default;
            }
        }
    }
}
