using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using HRAOrganGallery.Assets.Scripts.Scene;

namespace HRAOrganGallery
{
    /// <summary>
    /// A class to update the monitor in the scene with information about the current scene
    /// </summary>
    public class MonitorUpdater : MonoBehaviour
    {
        [SerializeField] private List<TMP_Text> _texts = new List<TMP_Text>();

        private void Awake()
        {
            //get total number of tissue blocks in scene
            SceneSetup.OnSceneBuilt += () => { _texts[0].text = SceneSetup.Instance.TissueBlocks.Count.ToString(); };

            //get tooltip for currently selected organ
            OrganCaller.OnOrganPicked += (data) => { _texts[1].text = data.Tooltip; };

            //get number of tissue blocks in currently selected organ
            OrganCaller.OnOrganPicked += (data) => { _texts[2].text = OrganCaller.Instance.NumberOfTissueBlocksInSelected.ToString(); };
        }
    }
}
