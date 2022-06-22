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

    [SerializeField] private SceneConfiguration sceneConfiguration;
    [SerializeField] private GameObject pre_TissueBlock;
    [SerializeField] private DataFetcher dataFetcher;
    [SerializeField] public NodeArray nodeArray;
    [SerializeField] private ModelLoader modelLoader;

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
        OnSceneBuilt?.Invoke();
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
        dataComponent.sceneGraph = node.scenegraph;
        dataComponent.representationOf = node.representation_of;
    }

    void ParentTissueBlocksToOrgans(List<GameObject> tissueBlocks, List<GameObject> organs)
    {
        for (int i = 0; i < tissueBlocks.Count; i++)
        {
            for (int j = 0; j < organs.Count; j++)
            {
                foreach (var url in tissueBlocks[i].GetComponent<TissueBlockData>().CcfAnnotations)
                {
                    if (url == organs[j].GetComponent<OrganData>().representationOf)
                    {
                        tissueBlocks[i].transform.parent = organs[j].transform;
                    }
                }
            }
        }
    }
}
