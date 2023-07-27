using UnityEngine;
using System;
using Assets.Scripts.Data;
using System.Collections;
using HRAOrganGallery;

namespace Assets.Scripts.Data
{
    public class CellTypeLoader : MonoBehaviour, IApiResponseHandler
    {
        public static CellTypeLoader Instance;
        [SerializeField] private string _fileName;
        public EnrichedRuiLocationArray locations;

        private void Awake()
        {
            GetJsonFromWeb();

            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
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

            locations = JsonUtility.FromJson<EnrichedRuiLocationArray>(result);
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
