using Assets.Scripts.Data;
using Assets.Scripts.Interaction;
using Assets.Scripts.Scene;
using Assets.Scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.Windows;
using static Assets.Scripts.Scene.SceneBuilder;

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
        [SerializeField] private DataFetcher _dataFetcher;

        [Header("Data")]
        [SerializeField] private NodeArray _nodeArray;
        [SerializeField] private List<string> _maleEntityIds = new List<string>();
        [SerializeField] private List<string> _femaleEntityIds = new List<string>();
        [SerializeField] private int numberOfHubmapIds;

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

            //get reference to DataFetcher
            _dataFetcher = GetComponent<DataFetcher>();

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
            _nodeArray = await SceneLoader.Instance.ShareData();

            //loop through organs in scene and response, add data, and place organs already in scene (match by scenegraphNode)
            Organs = EnrichOrgans(Organs);

            //create and place tissue blocks, then parent them to organs
            CreateAndPlaceTissueBlocks();

            //ParentTissueBlocksToOrgans(TissueBlocks, Organs);

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

                Node node = _nodeArray.nodes
                    .First(
                    n => n.scenegraph.Split("/")[n.scenegraph.Split("/").Length - 1].Replace(".glb", string.Empty) == current.name
                    );

                //add OrganData component and store relevant data from Node
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
            for (int i = 0; i < _nodeArray.nodes.Length; i++)
            {
                if (_nodeArray.nodes[i].scenegraph != null) continue;
                Matrix4x4 reflected = Utils.ReflectZ() * MatrixExtensions.BuildMatrix(_nodeArray.nodes[i].transformMatrix);
                GameObject block = Instantiate(
                    _preTissueBlock,
                    reflected.GetPosition(),
                    reflected.rotation
           );
                block.transform.localScale = reflected.lossyScale * 2f;
                block.AddComponent<TissueBlockData>().Init(_nodeArray.nodes[i]);
                TissueBlocks.Add(block);
                block.transform.parent = _parent;
            }
        }

        async void ParentTissueBlocksToOrgans(List<GameObject> tissueBlocks, List<GameObject> organs)
        {
            // Add back to AssignEntityIdsToDonorSexLists if delay bug
            _maleEntityIds = await GetEntityIdsBySex("https://ccf-api.hubmapconsortium.org/v1/tissue-blocks?sex=male");
            _femaleEntityIds = await GetEntityIdsBySex("https://ccf-api.hubmapconsortium.org/v1/tissue-blocks?sex=female");

            // assign donor sex to organ
            await GetOrganSex();

            // assign donor sex to tissue block and parent to organ
            for (int i = 0; i < TissueBlocks.Count; i++)
            {
                TissueBlockData tissueData = TissueBlocks[i].GetComponent<TissueBlockData>();
                if (_maleEntityIds.Contains(tissueData.EntityId))
                {
                    tissueData.DonorSex = "Male";
                }
                else
                {
                    tissueData.DonorSex = "Female";
                }

                for (int j = 0; j < Organs.Count; j++)
                {
                    OrganData organData = Organs[j].GetComponent<OrganData>();

                    foreach (var annotation in tissueData.CcfAnnotations)
                    {
                        if (organData.RepresentationOf == annotation && organData.DonorSex == tissueData.DonorSex)
                        {
                            TissueBlocks[i].transform.parent = Organs[j].transform.GetChild(0).transform;
                            break;
                        }
                    }
                }
            }

            var tasks = new List<Task>();

            for (int i = 0; i < tissueBlocks.Count; i++)
            {
                var progressHubmapIds = new Progress<bool>((value) =>
                {
                    if (value) numberOfHubmapIds++;
                });

                tasks.Add(tissueBlocks[i].GetComponent<HuBMAPIDFetcher>().FromEntityIdGetHubmapId(progressHubmapIds));
            }


            //tasks.Add(GetTissueBlocksWithCellTypes());

            tasks.Add(CCFAPISPARQLQuery.Instance.GetAllCellTypes());

            await Task.WhenAll(tasks);

            // trigger OnSceneBuilt event
            OnSceneBuilt?.Invoke();
        }


        public async Task<List<string>> GetEntityIdsBySex(string url)
        {
            List<string> result = new List<string>();
            DataFetcher httpClient = _dataFetcher;
            NodeArray nodeArray = await httpClient.Get(url);
            foreach (var node in nodeArray.nodes)
            {
                result.Add(node.jsonLdId);
            }
            return result;
        }

        public async Task GetOrganSex()
        {
            DataFetcher httpClient = _dataFetcher;
            NodeArray nodeArray = await httpClient.Get("https://ccf-api.hubmapconsortium.org/v1/reference-organs");
            // Debug.Log(nodeArray.nodes.Length);
            foreach (var organ in Organs)
            {
                OrganData organData = organ.GetComponent<OrganData>();

                foreach (var node in nodeArray.nodes)
                {
                    // Debug.Log("file: " + node.reference_organ);
                    if (organData.SceneGraph == node.glbObject.file)
                    {
                        organData.DonorSex = node.sex;
                    }
                }
            }
        }
    }

    public class HubmapIdArray
    {
        [SerializeField] public HubmapIdHolder[] hubmapIdHolder;
    }

    public class HubmapIdHolder
    {
        public string hubmap_id;
    }
}

