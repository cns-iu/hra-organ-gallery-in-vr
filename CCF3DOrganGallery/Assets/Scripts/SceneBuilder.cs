using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBuilder : MonoBehaviour
{
    [SerializeField] private GameObject pre_TissueBlock;
    // Start is called before the first frame update
    [SerializeField] private DataFetcher dataFetcher;
    [SerializeField] NodeArray _nodeArray;
    [SerializeField] NodeArray NodeArray { get; }

    void Start()
    {
        _nodeArray = dataFetcher.NodeArray;
        Debug.Log(_nodeArray.nodes.Length);
    }
}
