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
        public NodeArray T { get;set;}
        public void Deserialize(string rawWebResponse)
        {
            throw new System.NotImplementedException();
        }

        public async Task GetNodes()
        {
            WebLoader httpClient = new WebLoader();
            string response = await httpClient.Get("https://ccf-api.hubmapconsortium.org/v1/reference-organ-scene?organ-iri=http://purl.obolibrary.org/obo/UBERON_0004539");
            Deserialize(response);
        }

        public async Task<NodeArray> ShareData()
        {
            await GetNodes();
            return T;
        }

        private async void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))

            {
               
            }
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
