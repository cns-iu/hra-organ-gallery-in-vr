using Assets.Scripts.Data;
using Assets.Scripts.Scene;
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
        public List<GameObject> OrgansLowRes;
        public List<GameObject> OrgansHighRes;
        [SerializeField] private Transform _parentOrgansLowRes;
        [SerializeField] private Transform _parentOrgansHighRes;
        [SerializeField] private Transform _adjustedOrgansLowResOrigin;

        [Header("Prefabs and Setup")]
        [SerializeField] private GameObject _preTissueBlock;

        [field: SerializeField] public OrganSexMapping OrganSexMapping { get; private set; } //mapping to hold organ to sex mapping
        [field: SerializeField] public RuiLocationOrganMapping RuiLocationMapping { get; private set; }//mapping to hold rui location to ref organ mapping
        [field: SerializeField] public NodeArray NodeArray { get; private set; } //node array to hold CCF API response for /scene

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

            OrgansLowRes = AddToList(_parentOrgansLowRes, OrgansLowRes);
            OrgansHighRes = AddToList(_parentOrgansHighRes, OrgansHighRes);

        }

        private List<GameObject> AddToList(Transform parent, List<GameObject> list)
        {
            //add organs to list
            for (int i = 0; i < parent.childCount; i++)
            {
                list.Add(parent.GetChild(i).gameObject);
            }

            return list;
        }


        /// <summary>
        /// Runs during first frame
        /// </summary>
        private async void Start()
        {
            //get scene from CCF API
            NodeArray = await SceneLoader.Instance.ShareData();
            OrganSexMapping = await OrganSexLoader.Instance.ShareData();

            //commented out for now to save time in editor
            //RuiLocationMapping = await TissueBlockRefOrganLoader.Instance.ShareData();

            //loop through organs in scene and response, add data, and place organs already in scene (match by scenegraphNode)
            OrgansLowRes = EnrichOrgans(OrgansLowRes);
            OrgansHighRes = EnrichOrgans(OrgansHighRes);

            //create and place tissue blocks, then parent them to organs
            CreateAndPlaceTissueBlocks();

            //ParentTissueBlocksToOrgans(TissueBlocks, Organs);
            ParentTissueBlocksToOrgans(TissueBlocks, OrgansLowRes);

            //move all organs to central platform
            _parentOrgansLowRes.SetPositionAndRotation(_adjustedOrgansLowResOrigin.position, _adjustedOrgansLowResOrigin.rotation);

            //only show default organs to save FPS
            HideUndesiredOrgansAndTissueBlocks(OrgansLowRes, TissueBlocks);
        }

        private void HideUndesiredOrgansAndTissueBlocks(List<GameObject> lowResOrgans, List<GameObject> tissueBlocks)
        {
            for (int i = 0; i < lowResOrgans.Count; i++)
            {
                lowResOrgans[i].SetActive(SceneConfiguration.Instance.DefaultOrgansToShow.Contains(lowResOrgans[i].GetComponent<OrganData>().RepresentationOf));
            }

            for (int i = 0; i < tissueBlocks.Count; i++)
            {
                TissueBlockData data = tissueBlocks[i].GetComponent<TissueBlockData>();
                tissueBlocks[i].SetActive(data.CcfAnnotations.Intersect(SceneConfiguration.Instance.DefaultOrgansToShow).Any());
            }
        }

        /// <summary>
        /// A method to enrich organs with node data and place them accordingly
        /// </summary>
        /// <param name="organs">A list of GameObjects</param>
        /// <returns>An enriched list</returns>
        private List<GameObject> EnrichOrgans(List<GameObject> organs)
        {
            for (int i = 0; i < organs.Count; i++)
            {
                GameObject current = organs[i];
                //Debug.Log(current.name);

                Node node = new Node();

                try
                {
                    node = NodeArray.nodes
                    .First(
                    //catches case when using low-LOD models
                    n => n.scenegraph.Split("/")[n.scenegraph.Split("/").Length - 1].Replace(".glb", string.Empty).ToLower() == current.name.Replace("_0.2", "").ToLower()
                    );

                    current.AddComponent<OrganData>().Init(node);
                }
                catch (Exception)
                {

                    Debug.Log($"could not find data for {current}");
                }




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

            organ.GetComponent<OrganData>().DefaultPosition = organ.transform.position;
            //Debug.Log("done for :" + organ.name);
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
                block.transform.parent = _parentOrgansLowRes;
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
                for (int j = 0; j < OrgansLowRes.Count; j++)
                {
                    // match by reference_organ
                    if (TissueBlocks[i].GetComponent<TissueBlockData>().ReferenceOrgan == OrgansLowRes[j].GetComponent<OrganData>().ReferenceOrgan)
                    {
                        TissueBlocks[i].transform.parent = OrgansLowRes[j].transform;
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

    }
}

