using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class DataFetcher : MonoBehaviour
{
    private NodeArray _nodeArray;
    public NodeArray NodeArray
    {
        get { return _nodeArray; }
    }

    public async Task<NodeArray> Get(string url)
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

            var text = www.downloadHandler.text
           .Replace("@id", "jsonLdId")
           .Replace("@type", "jsonLdType");
            _nodeArray = JsonUtility.FromJson<NodeArray>(
                "{ \"nodes\":" +
                text
                + "}"
                );
            return _nodeArray;
        }
        catch (Exception ex)
        {
            Debug.LogError($"{nameof(Get)} failed: {ex.Message}");
            return default;
        }
    }
}

[Serializable]
public class NodeArray
{
    [SerializeField] public Node[] nodes;
}

[Serializable]
public class Node
{
    public string jsonLdId;
    public string jsonLdType;
    public string entityId;

    public string[] ccf_annotations;
    public string representation_of;
    public string reference_organ;
    public bool unpickable;
    public bool wireframe;
    public bool _lighting;
    public string scenegraph;
    public string scenegraphNode;
    public bool zoomBasedOpacity;
    public bool zoomToOnLoad;
    public int[] color;
    public float opacity;
    public float[] transformMatrix;
    public string name;
    public string tooltip;
    public float priority;

    public int rui_rank;
    public GLBObject glbObject; //for reference organs
    public string sex; //for reference organs
}

[Serializable]
public class GLBObject
{
    public string id;
    public string file;
}