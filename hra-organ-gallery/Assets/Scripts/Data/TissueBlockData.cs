using HRAOrganGallery;
using HRAOrganGallery.Assets.Scripts.Scene;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.Shared;
using HRAOrganGallery.Assets.Scripts;

namespace Assets.Scripts.Data
{
    public class TissueBlockData : MonoBehaviour, IApiSettable
    {
        [field: SerializeField]
        public string JsonLdId { get; set; }

        [field: SerializeField]
        public string EntityId { get; set; }

        [field: SerializeField]
        public string[] CcfAnnotations { get; set; }

        [field: SerializeField]
        public string HubmapId { get; set; }

        [field: SerializeField]
        public string ReferenceOrgan;

        [SerializeField] private ConsoleLogger _logger;

        public void Init(Node node)
        {
            JsonLdId = node.jsonLdId.Replace("\"", "");
            EntityId = node.entityId;
            CcfAnnotations = node.ccf_annotations;
            ReferenceOrgan = Utils.CleanReferenceOrganName(GetReferenceOrgan(JsonLdId, SceneSetup.Instance.RuiLocationMapping));
        }


        private void Awake()
        {
            try
            {
                _logger = FindObjectsOfType<ConsoleLogger>().Where(c => c.GetComponent<ConsoleLogger>().type == LoggerType.Data).First();
            }
            catch (System.Exception)
            {

            }

        }

        private string GetReferenceOrgan(string jsonId, RuiLocationOrganMapping mapping)
        {
            try
            {
                //the colon was renamed to large intestine! Hence the missing matches?

                var result = mapping.mappings.First(m => m.rui_location.Replace("\"", "") == jsonId).reference_organ.Replace("\"", "");
                string message = jsonId.Color("green");
                _logger.Log(message, this);
                return result.ToString();
            }
            catch
            {
                string message = jsonId.Color("red");
                _logger.Log(message, this);
                return "NO MATCH FOUND";

            }

        }



        //private string GetSex() { }

    }


}