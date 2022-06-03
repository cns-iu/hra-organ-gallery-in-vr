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
    }

    public async void GetNodes(string url)
    {
        var httpClient = dataFetcher;
        _nodeArray = await httpClient.Get(url);
        CreateTissueBlocks();
    }

    void CreateTissueBlocks()
    {
        for (int i = 0; i < _nodeArray.nodes.Length; i++)
        {
            GameObject block = Instantiate(
       pre_TissueBlock,
       MatrixExtensions.BuildMatrix(_nodeArray.nodes[i].transformMatrix).GetPosition(), //use Unity's built-in functionality, matrix col1 != pos etc.
       MatrixExtensions.BuildMatrix(_nodeArray.nodes[i].transformMatrix).rotation
       );
            block.transform.localScale = MatrixExtensions.BuildMatrix(_nodeArray.nodes[i].transformMatrix).lossyScale;
            block.transform.position = new Vector3(block.transform.position.x, block.transform.position.y, -block.transform.position.z);

            SetData(block, _nodeArray.nodes[i]);
        }
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
        Debug.Log(id);
        Debug.Log("Before: " + matrix);

        Vector3 pos = matrix.GetPosition();
        Quaternion rot = matrix.rotation;
        Vector3 s = matrix.lossyScale;

        Matrix4x4 m = new Matrix4x4();
        m.SetTRS(pos, rot, s);

        Debug.Log("matrix with pos/rot/s : " + m);

        Debug.Log(m == matrix);
    }

}
