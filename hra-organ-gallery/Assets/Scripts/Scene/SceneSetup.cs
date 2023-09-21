using Assets.Scripts.Data;
using Assets.Scripts.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace HRAOrganGallery.Assets.Scripts.Scene
{
    public class SceneSetup : MonoBehaviour
    {

        public static SceneSetup Instance;
        public static event Action OnSceneBuilt;

        [Header("Scene")]
        public List<GameObject> TissueBlocks;
        public List<GameObject> Organs;
        [SerializeField] private Transform _parent;
        [SerializeField] private Transform _adjustedOrganOrigin;

        [Header("Prefabs and Setup")]
        [SerializeField] private GameObject _preTissueBlock;

        [field: SerializeField] public OrganSexMapping OrganSexMapping { get; private set; } //mapping to hold organ to sex mapping
        [field: SerializeField] public RuiLocationOrganMapping RuiLocationMapping { get; private set; }//mapping to hold rui location to ref organ mapping
        [field: SerializeField] public NodeArray NodeArray { get; private set; } //node array to hold CCF API response for scene

        [SerializeField] private Logger _logger;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            //add organs to list
            for (int i = 0; i < _parent.childCount; i++)
            {
                Organs.Add(_parent.GetChild(i).gameObject);
            }

        }


        /// <summary>
        /// Runs during first frame
        /// </summary>
        private async void Start()
        {
            //get scene from CCF API
            NodeArray = await SceneLoader.Instance.ShareData();
            OrganSexMapping = await OrganSexLoader.Instance.ShareData();
            RuiLocationMapping = await TissueBlockRefOrganLoader.Instance.ShareData();


            //loop through organs in scene and response, add data, and place organs already in scene (match by scenegraphNode)
            Organs = EnrichOrgans(Organs);

            //create and place tissue blocks, then parent them to organs
            CreateAndPlaceTissueBlocks();

            //ParentTissueBlocksToOrgans(TissueBlocks, Organs);
            ParentTissueBlocksToOrgans(TissueBlocks, Organs);

            //move all organs to central platform
            _parent.SetPositionAndRotation(_adjustedOrganOrigin.position, _adjustedOrganOrigin.rotation);
        }

        /// <summary>
        /// A method to enrich organs with node data and place them accordingly
        /// </summary>
        /// <param name="organs">A list of GameObjects</param>
        /// <returns>An enriched list</returns>
        List<GameObject> EnrichOrgans(List<GameObject> organs)
        {
            for (int i = 0; i < organs.Count; i++)
            {
                GameObject current = organs[i];

                Node node = NodeArray.nodes
                    .First(
                    n => n.scenegraph.Split("/")[n.scenegraph.Split("/").Length - 1].Replace(".glb", string.Empty) == current.name
                    );

                current.AddComponent<OrganData>().Init(node);

                //place organ by transform matrix from Node
                PlaceOrgan(current, node);
            }
            return organs;
        }


        void PlaceOrgan(GameObject organ, Node node) //-1, 1, -1 -> for scale
        {
            Matrix4x4 reflected = Utils.ReflectZ() * MatrixExtensions.BuildMatrix(node.transformMatrix);
            organ.transform.position = reflected.GetPosition();
            organ.transform.rotation = new Quaternion(0f, 0f, 0f, 1f); //hard-coded to avoid bug when running natively on Quest 2
            organ.transform.localScale = new Vector3(
                reflected.lossyScale.x,
                reflected.lossyScale.y,
                -reflected.lossyScale.z
            );

        }

        void CreateAndPlaceTissueBlocks()
        {
            for (int i = 0; i < NodeArray.nodes.Length; i++)
            {
                if (NodeArray.nodes[i].scenegraph != null) continue;
                Matrix4x4 reflected = Utils.ReflectZ() * MatrixExtensions.BuildMatrix(NodeArray.nodes[i].transformMatrix);
                GameObject block = Instantiate(
                    _preTissueBlock,
                    reflected.GetPosition(),
                    reflected.rotation
           );
                block.transform.localScale = reflected.lossyScale * 2f;
                block.AddComponent<TissueBlockData>().Init(NodeArray.nodes[i]);
                TissueBlocks.Add(block);
                block.transform.parent = _parent;
            }
        }

        /// <summary>
        /// A method to parent tissue blocks from a list to organs from a list
        /// </summary>
        /// <param name="tissueBlocks">A List of GameObjects holding tissue blocks</param>
        /// <param name="organs">A List of GameObjects holding organs</param>
        private void ParentTissueBlocksToOrgans(List<GameObject> tissueBlocks, List<GameObject> organs)
        {


            for (int i = 0; i < TissueBlocks.Count; i++)
            {
                for (int j = 0; j < Organs.Count; j++)
                {
                    // match by reference_organ
                    if (TissueBlocks[i].GetComponent<TissueBlockData>().ReferenceOrgan == Organs[j].GetComponent<OrganData>().ReferenceOrgan)
                    {
                        TissueBlocks[i].transform.parent = Organs[j].transform;
                    }
                    else //alt: match by UBERON ID; won't work, because it will parent the tissue block to an organ regardless of sex!
                    {
                        //if (TissueBlocks[i].GetComponent<TissueBlockData>().CcfAnnotations.Contains(Organs[j].GetComponent<OrganData>().RepresentationOf))
                        //{
                        //    TissueBlocks[i].transform.parent = Organs[j].transform;
                        //}
                    }
                }
            }
            // trigger OnSceneBuilt event
            OnSceneBuilt?.Invoke();
        }


        //public async Task<List<string>> GetEntityIdsBySex(string url)
        //{
        //    List<string> result = new List<string>();
        //    DataFetcher httpClient = _dataFetcher;
        //    NodeArray nodeArray = await httpClient.Get(url);
        //    foreach (var node in nodeArray.nodes)
        //    {
        //        result.Add(node.jsonLdId);
        //    }
        //    return result;
        //}

        public async Task GetOrganSex()
        {
            //DataFetcher httpClient = _dataFetcher;
            //NodeArray nodeArray = await httpClient.Get("https://ccf-api.hubmapconsortium.org/v1/reference-organs");
            //// Debug.Log(nodeArray.nodes.Length);
            //foreach (var organ in Organs)
            //{
            //    OrganData organData = organ.GetComponent<OrganData>();

            //    foreach (var node in nodeArray.nodes)
            //    {
            //        // Debug.Log("file: " + node.reference_organ);
            //        if (organData.SceneGraph == node.glbObject.file)
            //        {
            //            organData.DonorSex = node.sex;
            //        }
            //    }
            //}
        }
    }
}

