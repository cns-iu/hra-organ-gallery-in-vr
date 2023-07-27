using UnityEngine;
using System;
using Assets.Scripts.Data;

namespace HRAOrganGallery
{
    public class CellTypeLoader : MonoBehaviour, IApiResponseHandler
    {
        [SerializeField] private string _fileName;
        [SerializeField] private EnrichedRuiLocationArray _locations;

        private void Awake()
        {
            GetJsonFromWeb();
            Web();
        }

        async void Web()
        {
            WebLoader http = new WebLoader();
            string s = await http.Get("https://ccf-api.hubmapconsortium.org/v1/scene");
                Debug.Log(s);
        }

        public void GetJsonFromWeb()
        {
            //This code should be changed later to get enriched RUI locations from the web
            //It should use a DataFetcher (or WebLoader) to get the data from the web
            var asset = Resources.Load<TextAsset>(_fileName);
            Deserialize(asset.text);
        }

        public void Deserialize(string raw)
        {
            var result = raw
              .Replace("@graph", "graph")
              .Replace("@id", "jsonLdId")
              .Replace("@type", "jsonLdType");

            _locations = JsonUtility.FromJson<EnrichedRuiLocationArray>(result);
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
}
