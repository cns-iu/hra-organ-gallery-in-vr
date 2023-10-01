using Assets.Scripts.Data;
using HRAOrganGallery.Assets.Scripts.Scene;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace HRAOrganGallery
{
    public class OrganCaller : MonoBehaviour
    {
        [SerializeField] private NodeArray _highResOrganNodeArray;
        [SerializeField] private Transform _platform;
        private string _requestedOrgan;
        private string _requestedSex;

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

            //set flags
            bool isOrgan = false;
            bool isSex = false;

            foreach (var organ in SceneSetup.Instance.OrgansHighRes)
            {
                foreach (var node in _highResOrganNodeArray.nodes)
                {
                    isOrgan = organ.GetComponent<OrganData>().RepresentationOf == node.representation_of;
                    isSex = organ.GetComponent<OrganData>().Sex == IdentifySex();

                    //fetch the right organ
                    if (isOrgan && isSex)
                    {
                        //get the right sex (also from button)
                        Debug.Log("found");
                        organ.gameObject.SetActive(true);
                        organ.transform.position = _platform.position;
                    }
                }



            }
        }

        string IdentifyOrgan()
        {
            // fill in code later where we get the ID associated with a button
            _requestedOrgan = "http://purl.obolibrary.org/obo/UBERON_0004539";
            return _requestedOrgan;
        }

        string IdentifySex()
        {
            _requestedSex = "Female";
            return _requestedSex;
        }

        private void Awake()
        {
            _platform = transform;
        }

    }
}
