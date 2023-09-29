using Assets.Scripts.Data;
using HRAOrganGallery.Assets.Scripts.Scene;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace HRAOrganGallery
{
    public class OrganCaller : MonoBehaviour
    {
        [SerializeField] private NodeArray _highResOrganNodeArray;
        [SerializeField] private Transform _platform;

        private async void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))

            {
                await PlaceOrgan();
            }
        }

        async Task PlaceOrgan()
        {
            _highResOrganNodeArray = await HighResOrganLoader.Instance.ShareData(IdentifyOrgan());
            foreach (var organ in SceneSetup.Instance.OrgansHighRes)
            {
                //if names match, fetch it
            }
        }

        string IdentifyOrgan()
        {
            return "http://purl.obolibrary.org/obo/UBERON_0004539";
        }

    }
}
