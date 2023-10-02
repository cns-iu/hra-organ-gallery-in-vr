using Assets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace HRAOrganGallery
{
    public class HighResOrganLoader : MonoBehaviour, IApiResponseHandler<NodeArray>
    {
        public static HighResOrganLoader Instance;
        [field: SerializeField] public NodeArray T { get; set; }

        [field: SerializeField] public string Url { get; set; }

        [SerializeField] private string _baseUrl = "https://ccf-api.hubmapconsortium.org/v1/reference-organ-scene";
        [SerializeField] private string organQuery = "?organ-iri=";
        [SerializeField] private string sexQuery = "&sex=";

        public void Deserialize(string rawWebResponse)
        {
            var result = rawWebResponse
             .Replace("@id", "jsonLdId")
               .Replace("@type", "jsonLdType")
               .Replace("\"object\":", "\"glbObject\":");

            T = JsonUtility.FromJson<NodeArray>(
                "{ \"nodes\":" +
                result
                + "}"
                );
        }

        public async Task GetNodes()
        {
            WebLoader httpClient = new WebLoader();
            string response = await httpClient.Get(Url);
            Deserialize(response);
        }

        public async Task<NodeArray> ShareData()
        {
            await GetNodes();
            return T;
        }

        //overload to set new URL depending on organ to load
        public async Task<NodeArray> ShareData(string organ_iri, string sex = "")
        {
            Url = _baseUrl + organQuery+ organ_iri + sexQuery + sex;
            await GetNodes();
            return T;
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
}
