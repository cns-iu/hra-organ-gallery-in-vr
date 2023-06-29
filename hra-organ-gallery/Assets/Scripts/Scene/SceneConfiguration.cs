using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;
using Assets.Scripts.Data;
using Assets.Scripts.UI;

namespace Assets.Scripts.Scene
{
    public class SceneConfiguration : MonoBehaviour
    {
        [Header("Filters")]
        public List<string> IdsOrgansToShow = new List<string>();
        [SerializeField] private SceneBuilder sceneBuilder;


        [Header("URL")]
        public string Url;
        [SerializeField] private string baseUrl = "https://ccf-api.hubmapconsortium.org/v1/scene"; //staging: https://ccf-api--staging.herokuapp.com/v1/scene
        [SerializeField]
        private List<string> uberonIds = new List<string>();
        [SerializeField] private string sex;
        [SerializeField] const string ontologyQueryString = "&ontology-terms=http://purl.obolibrary.org/obo/UBERON_";
        [SerializeField] const string sexQueryString = "?sex=";

        private void OnEnable()
        {
            FilterEventHandler.OnFilterCompleteWithOrgans += OnFilterSetOrganVisibility;
        }

        private void OnDestroy()
        {
            FilterEventHandler.OnFilterCompleteWithOrgans -= OnFilterSetOrganVisibility;
        }

        void OnFilterSetOrganVisibility(List<string> organs)
        {
            for (int i = 0; i < sceneBuilder.Organs.Count; i++)
            {
                OrganData data = sceneBuilder.Organs[i].GetComponent<OrganData>();
                sceneBuilder.Organs[i].gameObject.SetActive(organs.Contains(data.tooltip));
            }
        }

        private void Awake()
        {
            sceneBuilder = GetComponent<SceneBuilder>();
        }

        public string BuildUrl()
        {
            if (uberonIds.Count > 0)
            {
                Url = baseUrl + sexQueryString + sex + ontologyQueryString + uberonIds[0];

                for (int i = 1; i < uberonIds.Count; i++)
                {
                    Url += ontologyQueryString + uberonIds[i];
                }
            }
            else
            {
                Url = baseUrl + sexQueryString + sex;
            }
            return Url;
        }
    }
}