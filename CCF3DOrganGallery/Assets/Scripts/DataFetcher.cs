using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class DataFetcher : MonoBehaviour
{
    private NodeArray _nodeArray;
    public NodeArray NodeArray
    {
        get { return _nodeArray; }
    }

    [SerializeField] private string Url = "https://ccf-api.hubmapconsortium.org/v1/scene?sex=male&ontology-terms=http%3A%2F%2Fpurl.obolibrary.org%2Fobo%2FUBERON_0004538";

    void Awake()
    {
        StartCoroutine(getRequest(Url));
    }

    IEnumerator getRequest(string uri)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            var text = uwr.downloadHandler.text
            .Replace("@id", "jsonLdId")
            .Replace("@type", "jsonLdType");
            _nodeArray = JsonUtility.FromJson<NodeArray>(
                "{ \"nodes\":" +
                text
                + "}"
                );
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
    // Doc see https://math.gl/modules/core/docs/api-reference/matrix4
    public float[] transformMatrix;
    public string name;
    public string tooltip;
    public float priority;
}
