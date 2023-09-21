using HRAOrganGallery;
using HRAOrganGallery.Assets.Scripts.Scene;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.Shared;

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

        public void Init(Node node)
        {
            JsonLdId = node.jsonLdId.Replace("\"", "");
            EntityId = node.entityId;
            CcfAnnotations = node.ccf_annotations;
            ReferenceOrgan = Utils.CleanReferenceOrganName(GetReferenceOrgan(JsonLdId, SceneSetup.Instance.RuiLocationMapping));
        }

        private string GetReferenceOrgan(string jsonId, RuiLocationOrganMapping mapping)
        {
            try
            {
                //the colon was renamed to large intestine! Hence the missing matches
                Debug.Log(jsonId.Color("green"));
                return mapping.mappings.First(m => m.rui_location.Replace("\"", "") == jsonId).reference_organ.Replace("\"", "");
            }
            catch
            {
                Debug.Log(jsonId.Color("red"));
                return "NO MATCH FOUND";

            }

        }



        //private string GetSex() { }

    }


}