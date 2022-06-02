using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class SceneBuilder : MonoBehaviour
{
    [SerializeField] private string url = "https://ccf-api.hubmapconsortium.org/v1/scene?sex=male&ontology-terms=http%3A%2F%2Fpurl.obolibrary.org%2Fobo%2FUBERON_0004538";
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
            MatrixExtensions.BuildMatrix(_nodeArray.nodes[i].transformMatrix).ExtractPosition(),
            MatrixExtensions.BuildMatrix(_nodeArray.nodes[i].transformMatrix).rotation
            );
            block.transform.localScale = MatrixExtensions.ExtractScale(MatrixExtensions.BuildMatrix(_nodeArray.nodes[i].transformMatrix));
            _tissueBlocks.Add(block);
        }
    }
}
