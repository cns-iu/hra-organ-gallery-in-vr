using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Siccity.GLTFUtility;
using System.Threading.Tasks;

public class ModelLoader : MonoBehaviour
{
    [SerializeField] private string filePath;
    [SerializeField] private GameObject wrapper;

    public async Task<GameObject> GetModel(string url)
    {
        filePath = $"{Application.persistentDataPath}/Models/";
        wrapper = new GameObject
        {
            name = "Model"
        };

        DownloadFile(url);
        await Task.Yield();
        return wrapper;
    }

    public async void DownloadFile(string url)
    {
        string path = GetFilePath(url);
        if (File.Exists(path))
        {
            Debug.Log("Found file locally, loading...");
            LoadModel(path);
            return;
        }

        await GetFileRequest(url,(UnityWebRequest req) =>
               {
                   Debug.Log("sending request for " + url);
                   if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
                   {
                       Debug.Log($"{req.error} : {req.downloadHandler.text}");
                   }
                   else
                   {
                       Debug.Log("coroutine finished");
                       LoadModel(path);
                   }
               });
        Debug.Log("Model downloaded for: " + path);
        
        // StartCoroutine(GetFileRequest(url, (UnityWebRequest req) =>
        //        {
        //            if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
        //            {
        //                Debug.Log($"{req.error} : {req.downloadHandler.text}");
        //            }
        //            else
        //            {
        //             Debug.Log("coroutine finished");
        //                LoadModel(path);
        //            }
        //        }));
    }

    string GetFilePath(string url)
    {
        string[] pieces = url.Split('/');
        string filename = pieces[pieces.Length - 1];

        return $"{filePath}{filename}";
    }

    void LoadModel(string path)
    {
        ResetWrapper();
        GameObject model = Importer.LoadFromFile(path);
        model.transform.SetParent(wrapper.transform);
    }

    async Task GetFileRequest(string url, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            req.downloadHandler = new DownloadHandlerFile(GetFilePath(url));

            var operation = req.SendWebRequest();

            while (!operation.isDone)
                Debug.Log(operation.progress);
                await Task.Yield();

            // while (!req.isDone) {
            //     Debug.Log(req.downloadProgress);
            //     await Task.Yield();
            // }
            // yield return req.SendWebRequest();
            

            callback(req);
            // ADD TASK>YIELD!!!!!!!! or transform into async method!!!!!!!!!
        }
    }

    void ResetWrapper()
    {
        if (wrapper != null)
        {
            foreach (Transform trans in wrapper.transform)
            {
                Destroy(trans.gameObject);
            }
        }
    }
}
