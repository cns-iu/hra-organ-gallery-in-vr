using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class SceneBuilder : MonoBehaviour
{
    [SerializeField] private string url = "https://ccf-api.hubmapconsortium.org/v1/scene?sex=male&ontology-terms=http%3A%2F%2Fpurl.obolibrary.org%2Fobo%2FUBERON_0004538";
    // [SerializeField] private string urlSpatialPlacement = "https://ccf-api.hubmapconsortium.org/#/operations/get-spatial-placement";
    [SerializeField] private GameObject pre_TissueBlock;
    [SerializeField] private DataFetcher dataFetcher;
    [SerializeField] NodeArray _nodeArray;
    [SerializeField] NodeArray NodeArray { get; }
    [SerializeField] private List<GameObject> _tissueBlocks;
    [SerializeField] private ModelLoader modelLoader;
    private GameObject organ;

    private void Start()
    {
        GetNodes(url);
    }

    public async void GetNodes(string url)
    {
        DataFetcher httpClient = dataFetcher;
        _nodeArray = await httpClient.Get(url);
        CreateAndPlaceTissueBlocks();
        LoadOrgan(); //organ is currently added in ModelLoader.cs
    }

    void LoadOrgan()
    {
        organ = modelLoader.GetModel(_nodeArray.nodes[0].scenegraph);
        PlaceOrgan();
    }

    void PlaceOrgan() //-1, 1, -1 -> for scale
    {
        Matrix4x4 reflected = ReflectZ() * MatrixExtensions.BuildMatrix(_nodeArray.nodes[0].transformMatrix);
        organ.transform.position = reflected.GetPosition();
        organ.transform.rotation = reflected.rotation;
        organ.transform.localScale = new Vector3(
            reflected.lossyScale.x,
            reflected.lossyScale.y,
            -reflected.lossyScale.z
        );
        // reflected.lossyScale;
        Debug.Log("ReflectZ(): " + ReflectZ());
        Debug.Log("BuildMAtrix(): " + MatrixExtensions.BuildMatrix(_nodeArray.nodes[0].transformMatrix));
        Debug.Log("reflected: " + reflected);
        Debug.Log(reflected.GetPosition());
        Debug.Log(reflected.rotation);
        Debug.Log(reflected.lossyScale);
    }

    void CreateAndPlaceTissueBlocks()
    {
        for (int i = 1; i < _nodeArray.nodes.Length; i++)
        {
            Debug.Log(_nodeArray.nodes[0]);
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
        obj.AddComponent<TissueBlockData>().EntityId = node.entityId;
        obj.GetComponent<TissueBlockData>().TransformMatrix = node.transformMatrix;

        TestMatrix(obj.GetComponent<TissueBlockData>().TransformMatrix, obj.GetComponent<TissueBlockData>().EntityId);
    }

    void TestMatrix(float[] transformMatrix, string id)
    {
        Matrix4x4 matrix = MatrixExtensions.BuildMatrix(transformMatrix);
        Vector3 pos = matrix.GetPosition();
        Quaternion rot = matrix.rotation;
        Vector3 s = matrix.lossyScale;
        Matrix4x4 m = new Matrix4x4();
        m.SetTRS(pos, rot, s);
    }

}
