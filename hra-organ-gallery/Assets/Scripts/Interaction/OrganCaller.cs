using Assets.Scripts.Data;
using Assets.Scripts.Shared;
using HRAOrganGallery.Assets.Scripts.Scene;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace HRAOrganGallery
{
    public class OrganCaller : MonoBehaviour
    {
        [Header("3D Objects")]
        [SerializeField] private Transform _currentOrgan;
        [SerializeField] private GameObject pre_TissueBlock;
        [SerializeField] private Transform _platform;
        [SerializeField] private Transform _defaultLocation;

        [Header("Data")]
        [SerializeField] private NodeArray _highResOrganNodeArray;
        [SerializeField] private string _requestedOrgan;
        [SerializeField] private Sex _requestedSex = Sex.Male;
        [SerializeField] private Laterality _requestedLaterality = Laterality.Left;
        [SerializeField] private List<string> _possibleOrgans;

        [Header("Sex-exclusive Organs")]
        [SerializeField] private List<string> _femaleOnly;
        [SerializeField] private List<string> _maleOnly;

        private void Awake()
        {
            //subscribe to all keyboard buttons
            OrganCallButton.OnCLick += async (possibleOrgans) => { _possibleOrgans = possibleOrgans; await PickOrgan(); };
            SexCallButton.OnClick += async (sex) => { _requestedSex = sex; await PickOrgan(); };
            LaterialityCallButton.OnClick += async (laterality) => { _requestedLaterality = laterality; await PickOrgan(); };
        }



        private async void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))

            {
                //OrganList l = ScriptableObject.CreateInstance<OrganList>();

                //foreach (var item in SceneSetup.Instance.OrgansHighRes)
                //{
                //    Pairs p = new Pairs();
                //    p.name = item.GetComponent<OrganData>().Tooltip;
                //    p.iri = item.GetComponent<OrganData>().RepresentationOf;
                //    Debug.Log(p.name);
                //    l.OrganNames.Add(p);

                //}

                //AssetDatabase.CreateAsset(l, $"Assets/ScriptableObjects/OrganList.asset");
                //AssetDatabase.SaveAssets();

                await PickOrgan();
            }
        }

        private string DetermineByLateriality(List<string> iris)
        {
            if (iris.Count == 0) return iris[0];
            else
            {
                if (_requestedLaterality == Laterality.Left)
                {
                    return iris[0];
                }
                else
                {
                    return iris[1];
                }
            }
        }

        void RemoveCurrentOrgan()
        {
            if (_currentOrgan != null) _currentOrgan.transform.position = _currentOrgan.GetComponent<OrganData>().DefaultPosition;

        }

        async Task PickOrgan()
        {
            //remove the current organ
            RemoveCurrentOrgan();

            //determine the new one based on user input
            _requestedOrgan = DetermineByLateriality(_possibleOrgans);
            _requestedSex = _femaleOnly.Contains(_requestedOrgan) | _maleOnly.Contains(_requestedOrgan) ? Sex.None : _requestedSex;

            //preparing the API call
            string sexQueryParameter = _requestedSex == Sex.None ? "" : _requestedSex.ToString().ToLower();
            _highResOrganNodeArray = await HighResOrganLoader.Instance.ShareData(_requestedOrgan, sexQueryParameter);

            //set flags
            bool isOrgan, isSex;

            foreach (var organ in SceneSetup.Instance.OrgansHighRes)
            {
                foreach (var node in _highResOrganNodeArray.nodes)
                {

                    isOrgan = organ.GetComponent<OrganData>().RepresentationOf == node.representation_of;
                    isSex = organ.GetComponent<OrganData>().Sex.ToLower() == _requestedSex.ToString().ToLower();

                    //fetch the right organ
                    if (isOrgan && isSex)
                    {
                        //get the right sex (also from button)
                        organ.gameObject.SetActive(true);

                        //set organ opacity
                        Utils.SetOrganOpacity(organ, organ.GetComponent<OrganData>().Opacity);

                        CreateTissueBlocks(_highResOrganNodeArray, organ.transform);
                        organ.transform.position = _platform.position;
                        _currentOrgan = organ.transform;
                    }
                }
            }
        }

        private void CreateTissueBlocks(NodeArray nodeArray, Transform organ)
        {
            for (int i = 0; i < nodeArray.nodes.Length; i++)
            {
                if (nodeArray.nodes[i].scenegraph != null) continue;
                if (!nodeArray.nodes[i].ccf_annotations.Contains(nodeArray.nodes[0].representation_of)) continue;

                Node corresponding = new Node();

                foreach (var other in SceneSetup.Instance.NodeArray.nodes)
                {
                    if (other.entityId == nodeArray.nodes[i].entityId) corresponding = other;
                }


                Matrix4x4 reflected = Utils.ReflectZ() * MatrixExtensions.BuildMatrix(corresponding.transformMatrix);
                GameObject block = Instantiate(
                    pre_TissueBlock,
                    reflected.GetPosition(),
                    reflected.rotation
           );
                block.transform.localScale = reflected.lossyScale * 2f;
                block.AddComponent<TissueBlockData>().Init(nodeArray.nodes[i]);
                block.transform.parent = organ;
            }
        }





    }
}
