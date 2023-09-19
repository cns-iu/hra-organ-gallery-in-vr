using Assets.Scripts.Data;
using Assets.Scripts.Scene;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace HRAOrganGallery
{
    public class OrganSexLoader : MonoBehaviour, IApiResponseHandler<OrganSexMapping>
    {
        public static OrganSexLoader Instance { get; private set; }

        public OrganSexMapping organSexMapping;

        private string _url = "grlc.io/api-git/hubmapconsortium/ccf-grlc/subdir/ccf//ref-organ-to-sex?endpoint=https%3A%2F%2Fccf-api.hubmapconsortium.org%2Fv1%2Fsparql?format=application/json";

        public void Deserialize(string rawWebResponse)
        {
            var result = rawWebResponse;

            organSexMapping = JsonUtility.FromJson<OrganSexMapping>(
               "{ \"pairs\":" +
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

        public async Task<OrganSexMapping> ShareData()
        {
            await GetNodes();
            return organSexMapping;
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
    public class OrganSexMapping
    {
        public OrganSex[] pairs;
    }

    [Serializable]
    public class OrganSex
    {
        public string reference_organ;
        public string sex;
    }
}
