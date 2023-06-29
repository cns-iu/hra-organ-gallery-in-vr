using Assets.Scripts.Data;
using Assets.Scripts.Scene;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class OrganToggleGroup : MonoBehaviour
    {
        public List<Toggle> Toggles;

        [SerializeField] private SceneBuilder sceneBuilder;
        [SerializeField] private SceneConfiguration sceneConfiguration;
        [SerializeField] private List<GameObject> activeOrgans;
        [SerializeField] private Toggle pre_OrganToggle;
        [SerializeField] private GameObject toggleParent;

        private void OnEnable()
        {
            SceneBuilder.OnSceneBuilt += CreateToggles;
            //FilterEventHandler.OnFilterComplete += SetToggles;
        }

        private void OnDestroy()
        {
            SceneBuilder.OnSceneBuilt -= CreateToggles;
            //FilterEventHandler.OnFilterComplete -= SetToggles;
        }

        void CreateToggles()
        {
            List<string> uniqueTooltips = new List<string>();

            for (int i = 0; i < sceneBuilder.Organs.Count; i++)
            {
                if (!uniqueTooltips.Contains(sceneBuilder.Organs[i].GetComponent<OrganData>().tooltip))
                    uniqueTooltips.Add(sceneBuilder.Organs[i].GetComponent<OrganData>().tooltip);
            }

            for (int i = 0; i < uniqueTooltips.Count; i++)
            {
                Toggle newToggle = Instantiate(pre_OrganToggle);
                Toggles.Add(newToggle);
                newToggle.GetComponentInChildren<TMP_Text>().text = uniqueTooltips[i];
                newToggle.gameObject.name = uniqueTooltips[i];
                newToggle.transform.parent = toggleParent.transform;
            }

            SetToggles();
        }

        void SetToggles()
        {
            activeOrgans = sceneBuilder.Organs.Where((v) => v.activeSelf).ToList();

            for (int i = 0; i < Toggles.Count; i++)
            {
                for (int j = 0; j < activeOrgans.Count; j++)
                {
                    if (Toggles[i].GetComponentInChildren<TMP_Text>().text.Contains(activeOrgans[j].GetComponent<OrganData>().tooltip))
                    {
                        Toggles[i].isOn = true;
                        break;
                    }
                }
            }
        }
    }
}