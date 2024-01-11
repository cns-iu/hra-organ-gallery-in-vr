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
        //singleton implementation
        public static OrganCaller Instance;

        //a series of properties to expose private variables to the keyboard buttons
        public Sex RequestedSex { get { return _requestedSex; } private set { } }
        public Laterality RequestedLaterality { get { return _requestedLaterality; } private set { } }
        public string RequestedOrgan { get { return _requestedOrgan; } private set { } }

        public List<string> FemaleOnlyOrgans { get { return _femaleOnly; } private set { } }
        public List<string> MaleOnlyOrgans { get { return _maleOnly; } private set { } }
        public List<string> TwoSidedOrgans { get { return _twoSidedOrgans; } private set { } }

        [Header("3D Objects")]
        [SerializeField] private Transform _currentOrgan;
        [SerializeField] private GameObject pre_TissueBlock;
        [SerializeField] private Transform _platform;
        [SerializeField] private Transform _defaultLocation;
        [SerializeField] private List<GameObject> _organsLowRes;
        [SerializeField] private float _organOpacity;

        [Header("Data")]
        [SerializeField] private NodeArray _highResOrganNodeArray;
        [SerializeField] private string _requestedOrgan = null;
        [SerializeField] private Sex _requestedSex = Sex.Male;
        [SerializeField] private Laterality _requestedLaterality = Laterality.Left;
        [SerializeField] private List<string> _possibleOrgans;

        [Header("Sex-exclusive Organs")]
        [SerializeField] private List<string> _femaleOnly;
        [SerializeField] private List<string> _maleOnly;

        [Header("Two-sided Organs")]
        [SerializeField] private List<string> _twoSidedOrgans;


        private void Awake()
        {
            //subscribe to all keyboard buttons
            OrganCallButton.OnClick += async (possibleOrgans) => { _possibleOrgans = possibleOrgans; await PickOrgan(); };
            SexCallButton.OnClick += async (sex) =>
            {
                _requestedSex = sex;
                //EnableDisableButtons(sex.ToString().ToLower()); 
                await PickOrgan();
            };
            LaterialityCallButton.OnClick += async (laterality) => { _requestedLaterality = laterality; await PickOrgan(); };

            //get low res organs from SceneSetup
            _organsLowRes = SceneSetup.Instance.OrgansLowRes;

            //implement singleton instance

            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
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

        //currently unused
        private void EnableDisableButtons(string currentSex)
        {
            //loop through low res organs and enable only the ones whose sex is selected
            for (int i = 0; i < _organsLowRes.Count; i++)
            {
                GameObject _currentOrgan = _organsLowRes[i];
                OrganData data = _currentOrgan.GetComponent<OrganData>();
                _currentOrgan.gameObject.SetActive(data.Sex.ToLower() == currentSex);
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
            if (_femaleOnly.Contains(_requestedOrgan)) { _requestedSex = Sex.Female; }
            if (_maleOnly.Contains(_requestedOrgan)) { _requestedSex = Sex.Male; }

            //preparing the API call
            _highResOrganNodeArray = await HighResOrganLoader.Instance.ShareData(_requestedOrgan, _requestedSex.ToString().ToLower());

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
                        //uncomment if by API response
                        //Utils.SetOrganOpacity(organ, organ.GetComponent<OrganData>().Opacity);

                        //uncomment if set here
                        Utils.SetOrganOpacity(organ, _organOpacity);

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
                Debug.Log("tissue block created");
                block.transform.parent = organ;
            }
        }





    }
}
