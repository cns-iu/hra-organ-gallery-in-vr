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
        public void OnSelect(GameObject tissueBlock) {

            string entityId = tissueBlock.GetComponent<TissueBlockData>().EntityId;
            Debug.Log(entityId);
        }
    }
}
