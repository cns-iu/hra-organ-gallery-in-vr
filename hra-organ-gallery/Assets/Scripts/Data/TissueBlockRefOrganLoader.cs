using Assets.Scripts.Data;
using Assets.Scripts.Scene;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace HRAOrganGallery
{
    public class TissueBlockRefOrganLoader : MonoBehaviour, IApiResponseHandler<RuiLocationOrganMapping>
    {
        public static TissueBlockRefOrganLoader Instance { get; private set; }
        public RuiLocationOrganMapping ruiLocationMapping;

        private string _url = "grlc.io/api-git/hubmapconsortium/ccf-grlc/subdir/ccf//rui-location-to-ref-organ?endpoint=https%3A%2F%2Fccf-api.hubmapconsortium.org%2Fv1%2Fsparql?format=application/json";

        public void Deserialize(string rawWebResponse)
        {
            var result = rawWebResponse;

            ruiLocationMapping = JsonUtility.FromJson<RuiLocationOrganMapping>(
               "{ \"mappings\":" +
               result
               + "}"
                );
        }

        public async Task GetNodes()
        {
            WebLoader httpClient = new WebLoader();
            string response = await httpClient.Get(_url);
            Deserialize(response);
        }

        public async Task<RuiLocationOrganMapping> ShareData()
        {
            await GetNodes();
            return ruiLocationMapping;
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }
    }

    [Serializable]
    public class RuiLocationOrganMapping
    {
        [SerializeField] public RuiToOrgan[] mappings;
    }

    [Serializable]
    public class RuiToOrgan
    {
        public string rui_location;
        public string reference_organ;
    }
}
