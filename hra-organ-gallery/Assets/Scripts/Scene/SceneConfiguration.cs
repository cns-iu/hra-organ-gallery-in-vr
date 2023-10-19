using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Scene
{
    public class SceneConfiguration : MonoBehaviour
    {
        public static SceneConfiguration Instance;

        [Header("Filters")]
        public List<string> IdsOrgansToShow = new List<string>();
        public string Url;
        [SerializeField] private string ccfApiEndPoint = "https://ccf-api.hubmapconsortium.org/v1/scene"; //staging: https://ccf-api--staging.herokuapp.com/v1/scene
        [SerializeField]
        private List<string> uberonIds = new List<string>();
        [SerializeField] const string ontologyQueryString = "&ontology-terms=http://purl.obolibrary.org/obo/UBERON_";
        [SerializeField] private List<string> _defaultOrgansToShow = new List<string>();
        public List<string> DefaultOrgansToShow { get { return _defaultOrgansToShow; } set { } }

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

        public string BuildUrl()
        {
            if (uberonIds.Count > 0)
            {
                Url = ccfApiEndPoint + ontologyQueryString + uberonIds[0];

                for (int i = 1; i < uberonIds.Count; i++)
                {
                    Url += ontologyQueryString + uberonIds[i];
                }
            }
            else
            {
                Url = ccfApiEndPoint;
            }
            return Url;
        }
    }
}