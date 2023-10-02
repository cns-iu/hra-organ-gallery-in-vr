using Assets.Scripts.Data;
using Assets.Scripts.Shared;
using HRAOrganGallery.Assets.Scripts.Scene;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

namespace HRAOrganGallery
{
    public class OrganCaller : MonoBehaviour
    {
        [SerializeField] private GameObject pre_TissueBlock;
        [SerializeField] private NodeArray _highResOrganNodeArray;
        [SerializeField] private Transform _platform;
        private string _requestedOrgan;
        private string _requestedSex;

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



        async Task PickOrgan()
        {
            _highResOrganNodeArray = await HighResOrganLoader.Instance.ShareData(IdentifyOrgan(), IdentifySex());

            //set flags
            bool isOrgan, isSex;

            foreach (var organ in SceneSetup.Instance.OrgansHighRes)
            {
                foreach (var node in _highResOrganNodeArray.nodes)
                {

                    isOrgan = organ.GetComponent<OrganData>().RepresentationOf == node.representation_of;
                    isSex = organ.GetComponent<OrganData>().Sex.ToLower() == IdentifySex();

                    //fetch the right organ
                    if (isOrgan && isSex)
                    {
                        //get the right sex (also from button)
                        organ.gameObject.SetActive(true);
                        CreateTissueBlocks(_highResOrganNodeArray, organ.transform);
                        organ.transform.position = _platform.position;
                    }
                }
            }
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

        string IdentifyOrgan()
        {
            // fill in code later where we get the ID associated with a button
            _requestedOrgan = "http://purl.obolibrary.org/obo/UBERON_0004538";
            return _requestedOrgan;
        }

        string IdentifySex()
        {
            _requestedSex = "female";
            return _requestedSex;
        }

        private void Awake()
        {
            _platform = transform;
        }

    }
}
