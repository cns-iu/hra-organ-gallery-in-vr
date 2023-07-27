using Assets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using Assets.Scripts.Scene;

namespace HRAOrganGallery
{
    public class SceneLoader : MonoBehaviour, IApiResponseHandler
    {
        public static SceneLoader Instance { get; private set; }
        public NodeArray nodeArray;

        private string _url;

        private void Start()
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

        public async Task<NodeArray> ShareData() {
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
}
