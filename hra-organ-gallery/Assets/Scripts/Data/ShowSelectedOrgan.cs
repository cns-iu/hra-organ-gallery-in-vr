using Assets.Scripts.Data;
using Assets.Scripts.Scene;
using HRAOrganGallery.Assets.Scripts.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public class ShowSelectedOrgan : MonoBehaviour
    {
        [SerializeField] private Color32 _outlineColor = new Color32(255, 0, 67, 255);
        private void Awake()
        {
            OrganCaller.OnOrganPicked += ShowHide;
        }

        private void OnDestroy()
        {
            OrganCaller.OnOrganPicked -= ShowHide;
        }

        private void ShowHide(OrganData data)
        {
            SceneSetup.Instance.OrgansLowRes.ForEach(organ =>
            {
                //guarding clause: if a default organ, return
                if (SceneConfiguration.Instance.DefaultOrgansToShow.Contains(organ.GetComponent<OrganData>().RepresentationOf)) return;

                //set currently selected organ active in low res version
                organ.SetActive(data.ReferenceOrgan == organ.GetComponent<OrganData>().ReferenceOrgan);
                

                if (organ.activeSelf)
                {
                    Outline o = organ.AddComponent<Outline>();
                    o.OutlineColor = _outlineColor;
                }
                else
                {
                    Destroy(organ.GetComponent<Outline>());
                }
            });
        }
    }
}
