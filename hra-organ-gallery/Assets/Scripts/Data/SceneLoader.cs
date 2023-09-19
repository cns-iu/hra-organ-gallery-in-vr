using Assets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using Assets.Scripts.Scene;

namespace HRAOrganGallery
{
    /// <summary>
    /// A class to load a scene from the CCF API at https://ccf-api.hubmapconsortium.org/v1/scene
    /// </summary>
    public class SceneLoader : MonoBehaviour, IApiResponseHandler<NodeArray>
    {
        public static SceneLoader Instance { get; private set; }
        public NodeArray nodeArray;

        private string _url;

        private void Awake()
        {
            _url = SceneConfiguration.Instance.BuildUrl();
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        public async Task<NodeArray> ShareData()
        {
            await GetNodes();
            return nodeArray;
        }

        public async Task GetNodes()
        {
            WebLoader httpClient = new WebLoader();
            string response = await httpClient.Get(_url);
            Deserialize(response);
        }

        public void Deserialize(string rawWebResponse)
        {
            var result = rawWebResponse
             .Replace("@id", "jsonLdId")
               .Replace("@type", "jsonLdType")
               .Replace("\"object\":", "\"glbObject\":");

            nodeArray = JsonUtility.FromJson<NodeArray>(
                "{ \"nodes\":" +
                result
                + "}"
                );
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
}
