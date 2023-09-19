using UnityEngine;
using System;
using Assets.Scripts.Data;
using System.Collections;
using HRAOrganGallery;
using System.Threading.Tasks;

namespace Assets.Scripts.Data
{
    public class CellTypeLoader : MonoBehaviour, IApiResponseHandler<TextAsset>
    {
        public static CellTypeLoader Instance;
        [SerializeField] private string _fileName;
        public EnrichedRuiLocationArray locations;

        private string _url;

        private void Awake()
        {
            GetNodes();

            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        public async Task GetNodes()
        {
            //Will implement later to get data directly from GitHub
            //Something like:
            //WebLoader httpClient = new WebLoader();
            //string response = await httpClient.Get(_url);
            //Deserialize(response);
            var asset = Resources.Load<TextAsset>(_fileName);
            Deserialize(asset.text);
        }

        public void Deserialize(string raw)
        {
            var result = raw
              .Replace("@graph", "graph")
              .Replace("@id", "jsonLdId")
              .Replace("@type", "jsonLdType");

            locations = JsonUtility.FromJson<EnrichedRuiLocationArray>(result);
        }

        public Task<TextAsset> ShareData()
        {
            throw new NotImplementedException();
        }
    }



    [Serializable]
    public class EnrichedRuiLocationArray
    {
        public EnrichedRuiLocation[] graph;
    }

    [Serializable]
    public class EnrichedRuiLocation
    {
        public string jsonLdId;
        public Sample[] samples;
    }

    [Serializable]
    public class Sample
    {
        public string jsonLdId;
        public RuiLocation rui_location;
    }

    [Serializable]
    public class RuiLocation
    {
        public Summaries[] summaries;
    }

    [Serializable]
    public class Summaries
    {
        public string jsonLdType;
        public string annotation_method;
        public Summary[] summary;
    }

    [Serializable]
    public class Summary
    {
        public string jsonLdType;
        public string cell_id;
        public string cell_label;
        public int count;
        public float percentage;
    }
}
