using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class SceneBuilder : MonoBehaviour
{
    public delegate void SceneBuilt();
    public static event SceneBuilt OnSceneBuilt;
    public List<GameObject> TissueBlocks;
    public List<GameObject> Organs;
    public List<string> MaleEntityIds;
    public List<string> FemaleEntityIds;

    [SerializeField] private SceneConfiguration sceneConfiguration;
    [SerializeField] private GameObject pre_TissueBlock;
    [SerializeField] private DataFetcher dataFetcher;
    [SerializeField] public NodeArray nodeArray;
    [SerializeField] private ModelLoader modelLoader;
    [SerializeField] private string maleLetters;
    [SerializeField] private string femaleLetters;

    private void Start()
    {
        sceneConfiguration = GetComponent<SceneConfiguration>();
        GetNodes(sceneConfiguration.BuildUrl());
    }

    public async void GetNodes(string url)
    {
        DataFetcher httpClient = dataFetcher;
        nodeArray = await httpClient.Get(url);
        LoadOrgans(); //organ is currently added in ModelLoader.cs
        CreateAndPlaceTissueBlocks();
        ParentTissueBlocksToOrgans(TissueBlocks, Organs);
    }

    void LoadOrgans()
    {
        foreach (var node in nodeArray.nodes)
        {
            if (node.scenegraph == null) return;
            GameObject organ = modelLoader.GetModel(node.scenegraph);
            PlaceOrgan(organ, node);
            SetOrganData(organ, node);
            Organs.Add(organ);
        }
    }

    void PlaceOrgan(GameObject organ, Node node) //-1, 1, -1 -> for scale
    {
        Matrix4x4 reflected = ReflectZ() * MatrixExtensions.BuildMatrix(node.transformMatrix);
        organ.transform.position = reflected.GetPosition();
        organ.transform.rotation = reflected.rotation;
        organ.transform.localScale = new Vector3(
            reflected.lossyScale.x,
            reflected.lossyScale.y,
            -reflected.lossyScale.z
        );

        SetOrganOpacity(organ, node.opacity);
    }

    void SetOrganOpacity(GameObject organWrapper, float alpha)
    {

        List<Transform> list = new List<Transform>();
        list = LeavesFinder.FindLeaves(organWrapper.transform.GetChild(0), list);

        foreach (var item in list)
        {
            Renderer renderer = item.GetComponent<MeshRenderer>();

            if (renderer == null) continue;
            Color updatedColor = renderer.material.color;
            updatedColor.a = alpha;
            renderer.material.color = updatedColor;

            Shader standard;
            standard = Shader.Find("Standard");
            renderer.material.shader = standard;
            MaterialExtensions.ToFadeMode(renderer.material);
        }
    }

    void CreateAndPlaceTissueBlocks()
    {
        for (int i = 1; i < nodeArray.nodes.Length; i++)
        {
            if (nodeArray.nodes[i].scenegraph != null) continue;
            Matrix4x4 reflected = ReflectZ() * MatrixExtensions.BuildMatrix(nodeArray.nodes[i].transformMatrix);
            GameObject block = Instantiate(
                pre_TissueBlock,
                reflected.GetPosition(),
                reflected.rotation
       );
            block.transform.localScale = reflected.lossyScale * 2f;
            SetTissueBlockData(block, nodeArray.nodes[i]);
            SetCellTypeData(block);
            TissueBlocks.Add(block);
        }
    }

    Matrix4x4 ReflectZ()
    {
        var result = new Matrix4x4(
            new Vector4(1, 0, 0, 0),
            new Vector4(0, 1, 0, 0),
            new Vector4(0, 0, -1, 0),
            new Vector4(0, 0, 0, 1)
        );
        return result;
    }

    void SetTissueBlockData(GameObject obj, Node node)
    {
        TissueBlockData dataComponent = obj.AddComponent<TissueBlockData>();
        dataComponent.EntityId = node.entityId;
        dataComponent.Name = node.name;
        dataComponent.Tooltip = node.tooltip;
        dataComponent.CcfAnnotations = node.ccf_annotations;
    }

    void SetCellTypeData(GameObject obj)
    {
        obj.AddComponent<CellTypeData>();
        obj.AddComponent<CellTypeDataFetcher>();
    }

    void SetOrganData(GameObject obj, Node node)
    {
        OrganData dataComponent = obj.AddComponent<OrganData>();
        dataComponent.SceneGraph = node.scenegraph;
        dataComponent.RepresentationOf = node.representation_of;
    }

    async void ParentTissueBlocksToOrgans(List<GameObject> tissueBlocks, List<GameObject> organs)
    {
        // Add back to AssignEntityIdsToDonorSexLists if delay bug
        // AssignEntityIdsToDonorSexLists();
        MaleEntityIds = await GetEntityIdsBySex("https://ccf-api.hubmapconsortium.org/v1/tissue-blocks?sex=male");
        FemaleEntityIds = await GetEntityIdsBySex("https://ccf-api.hubmapconsortium.org/v1/tissue-blocks?sex=female");

        // assign donor sex to organ
        for (int i = 0; i < Organs.Count; i++)
        {
            OrganData data = Organs[i].GetComponent<OrganData>();

            for (int n = 0; n < MaleEntityIds.Count; n++)
            {
                if (data.SceneGraph.Contains(maleLetters))
                {
                    data.DonorSex = "male";
                }
                else
                {
                    data.DonorSex = "female";
                }
            }
        }

        // assign donor sex to tissue block and parent to organ
        for (int i = 0; i < TissueBlocks.Count; i++)
        {
            TissueBlockData tissueData = TissueBlocks[i].GetComponent<TissueBlockData>();
            if (MaleEntityIds.Contains(tissueData.EntityId))
            {
                tissueData.DonorSex = "male";
            }
            else
            {
                tissueData.DonorSex = "female";
            }

            // parenting should happen here, still leaves some tissue blocks unparented
            for (int j = 0; j < Organs.Count; j++)
            {
                OrganData organData = Organs[j].GetComponent<OrganData>();

                foreach (var annotation in tissueData.CcfAnnotations)
                {
                    if (tissueData.DonorSex == organData.DonorSex && organData.RepresentationOf == annotation)
                    {
                        TissueBlocks[i].transform.parent = Organs[j].transform;
                        break;
                    }
                }
            }
        }

        // trigger OnSceneBuilt event
        OnSceneBuilt?.Invoke();
    }

    public async Task<List<string>> GetEntityIdsBySex(string url)
    {
        List<string> result = new List<string>();
        DataFetcher httpClient = dataFetcher;
        nodeArray = await httpClient.Get(url);
        foreach (var item in nodeArray.nodes)
        {
            result.Add(item.jsonLdId);
        }
        return result;
    }

    // async void AssignEntityIdsToDonorSexLists()
    // {

    // }
}
