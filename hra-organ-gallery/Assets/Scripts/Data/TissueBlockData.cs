using HRAOrganGallery;
using HRAOrganGallery.Assets.Scripts.Scene;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Data
{
    public class TissueBlockData : MonoBehaviour, IApiSettable
    {
        [field: SerializeField]
        public string JsonLdId { get; set; }

        [field: SerializeField]
        public string EntityId { get; set; }

        [field: SerializeField]
        public string Name { get; set; }

        [field: SerializeField]
        public string Tooltip { get; set; }

        [field: SerializeField]
        public string[] CcfAnnotations { get; set; }

        [field: SerializeField]
        public string HubmapId { get; set; }

        [field: SerializeField]
        public string DonorSex;

        [field: SerializeField]
        public string ReferenceOrgan;

        public void Init(Node node, string sex = "")
       => (EntityId, Name, CcfAnnotations)
       = (node.entityId, node.name, node.ccf_annotations);

        public void Init(Node node)
        {
            JsonLdId = node.jsonLdId; 
            EntityId = node.entityId;
            Name = node.name;
            CcfAnnotations = node.ccf_annotations;
            ReferenceOrgan = GetReferenceOrgan(JsonLdId, SceneSetup.Instance.RuiLocationMapping);
        }

        private string GetReferenceOrgan(string jsonId, RuiLocationOrganMapping mapping) {
            return mapping.mappings.First(m => m.rui_location == jsonId).reference_organ;
        }



        //private string GetSex() { }

    }


}