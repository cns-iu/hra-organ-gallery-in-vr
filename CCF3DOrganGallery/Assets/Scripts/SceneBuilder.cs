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

    private void Start()
    {
        GetNodes(url);
        GetOrgan(); //organ is currently added in ModelLoader.cs
    }

    public async void GetNodes(string url)
    {
        var httpClient = dataFetcher;
        _nodeArray = await httpClient.Get(url);
        CreateTissueBlocks();
    }

    void GetOrgan()
    {
       
    }

    void CreateTissueBlocks()
    {

        for (int i = 0; i < _nodeArray.nodes.Length; i++)
        {
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
