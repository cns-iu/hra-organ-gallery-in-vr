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

    public void DownloadFile(string url)
    {
        string path = GetFilePath(url);
        if (File.Exists(path))
        {
            Debug.Log("Found file locally, loading...");
            LoadModel(path);
            return;
        }
        StartCoroutine(GetFileRequest(url, (UnityWebRequest req) =>
               {
                   if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
                   {
                       Debug.Log($"{req.error} : {req.downloadHandler.text}");
                   }
                   else
                   {
                    Debug.Log("coroutine finished");
                       LoadModel(path);
                   }
               }));
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

    IEnumerator GetFileRequest(string url, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            req.downloadHandler = new DownloadHandlerFile(GetFilePath(url));
            yield return req.SendWebRequest();
            Debug.Log(req.isDone);
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
