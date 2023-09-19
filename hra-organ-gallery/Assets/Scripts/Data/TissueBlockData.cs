using HRAOrganGallery;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Data
{
    public class TissueBlockData : MonoBehaviour, IApiSettable
    {
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

        public void Init(Node node, string sex = "")
       => (EntityId, Name, CcfAnnotations)
       = (node.entityId, node.name, node.ccf_annotations);
    }


}