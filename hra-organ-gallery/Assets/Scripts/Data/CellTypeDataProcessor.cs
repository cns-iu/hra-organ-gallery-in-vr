using HRAOrganGallery;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace Assets.Scripts.Data
{
    public class CellTypeDataProcessor : MonoBehaviour, ISelectionResponse
    {
        public Sample currentSample;
        public void OnSelect(GameObject tissueBlock)
        {
            GetData(tissueBlock.GetComponent<TissueBlockData>());
        }

        void GetData(TissueBlockData tissueBlockData)
        {
            foreach (var donor in CellTypeLoader.Instance.locations.graph)
            {
                foreach (var sample in donor.samples)
                {
                    if (sample.jsonLdId == tissueBlockData.EntityId)
                    {
                        currentSample = sample;
                        break;
                    }
                }
            }
        }
    }
}
