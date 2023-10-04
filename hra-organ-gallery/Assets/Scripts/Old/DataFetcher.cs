using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Threading.Tasks;

namespace Assets.Scripts.Data
{
    public class DataFetcher : MonoBehaviour
    {
        //private NodeArray _nodeArray;
        //public NodeArray NodeArray
        //{
        //    get { return _nodeArray; }
        //}

        //public async Task<NodeArray> Get(string url)
        //{
        //    try
        //    {
        //        using var www = UnityWebRequest.Get(url);
        //        var operation = www.SendWebRequest();

        //        while (!operation.isDone)
        //            await Task.Yield();

        //        if (www.result != UnityWebRequest.Result.Success)
        //            Debug.LogError($"Failed: {www.error}");

        //        var result = www.downloadHandler.text;

        //        var text = www.downloadHandler.text
        //       .Replace("@id", "jsonLdId")
        //       .Replace("@type", "jsonLdType")
        //       .Replace("\"object\":", "\"glbObject\":");

        //        //_nodeArray = JsonUtility.FromJson<NodeArray>(
        //        //    "{ \"nodes\":" +
        //        //    text
        //        //    + "}"
        //        //    );
        //        return _nodeArray;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.LogError($"{nameof(Get)} failed: {ex.Message}");
        //        return default;
        //    }
        //}
    }

   
}