using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class SceneBuilder : MonoBehaviour
{
    public delegate void SceneBuilt();
    public static event SceneBuilt OnSceneBuilt;

    [SerializeField] private string url = "https://ccf-api--staging.herokuapp.com/v1/reference-organ-scene?ontology-terms=http%3A%2F%2Fpurl.obolibrary.org%2Fobo%2FUBERON_0004538&organ-iri=http%3A%2F%2Fpurl.obolibrary.org%2Fobo%2FUBERON_0002097&sex=male";
    // use SCENE: https://ccf-api.hubmapconsortium.org/v1/scene
    //old: https://ccf-api--staging.herokuapp.com/v1/reference-organ-scene?ontology-terms=http%3A%2F%2Fpurl.obolibrary.org%2Fobo%2FUBERON_0004538&organ-iri=http%3A%2F%2Fpurl.obolibrary.org%2Fobo%2FUBERON_0004538&sex=male
    //ontology terms: tissue block; organ iri: organ
    // heart: 0000948
    //left kidney: 0004538
    //skin: 0002097
    //choose organ iri: skin for correct placement and ontology term for which tissue blocks you want to show

    [SerializeField] private GameObject pre_TissueBlock;
    [SerializeField] private DataFetcher dataFetcher;
    [SerializeField] NodeArray _nodeArray;
    [SerializeField] NodeArray NodeArray { get; }
    [SerializeField] private List<GameObject> _tissueBlocks;
    [SerializeField] private ModelLoader modelLoader;
    // private GameObject organ;

    private void Start()
    {
        GetNodes(url);
    }

    public async void GetNodes(string url)
    {
        DataFetcher httpClient = dataFetcher;
        _nodeArray = await httpClient.Get(url);
        LoadOrgans(); //organ is currently added in ModelLoader.cs
        CreateAndPlaceTissueBlocks();
        OnSceneBuilt?.Invoke();
    }

    void LoadOrgans()
    {
        foreach (var node in _nodeArray.nodes)
        {
            if (node.scenegraph == null) return;
            GameObject organ = modelLoader.GetModel(node.scenegraph);
            PlaceOrgan(organ, node);
        }
    }

    void PlaceOrgan(GameObject organ, Node node) //-1, 1, -1 -> for scale
    {
        Matrix4x4 reflected = ReflectZ() * MatrixExtensions.BuildMatrix(_nodeArray.nodes[0].transformMatrix);
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
        for (int i = 1; i < _nodeArray.nodes.Length; i++)
        {
            if (_nodeArray.nodes[i].scenegraph != null) continue;
            Matrix4x4 reflected = ReflectZ() * MatrixExtensions.BuildMatrix(_nodeArray.nodes[i].transformMatrix);
            GameObject block = Instantiate(
                pre_TissueBlock,
                reflected.GetPosition(),
                reflected.rotation
       );
            block.transform.localScale = reflected.lossyScale * 2f;
            SetData(block, _nodeArray.nodes[i]);
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

    void SetData(GameObject obj, Node node)
    {
        TissueBlockData dataComponent = obj.AddComponent<TissueBlockData>();
        dataComponent.EntityId = node.entityId;
        dataComponent.Name = node.name;
        dataComponent.Tooltip = node.tooltip;
        dataComponent.CcfAnnotations = node.ccf_annotations;
    }
}
