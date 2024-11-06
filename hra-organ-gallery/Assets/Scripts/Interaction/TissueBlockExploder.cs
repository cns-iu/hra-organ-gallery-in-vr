using Assets.Scripts.Data;
using Assets.Scripts.Shared;
using HRAOrganGallery;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.CullingGroup;

public class TissueBlockExploder : MonoBehaviour
{
    [Header("Sphere")]
    [SerializeField] private Transform _sphere;

    [Header("Explode tissue blocks")]
    [SerializeField] private InputActionReference _explodeTissueBlocks;
    [SerializeField] private List<Transform> _explodeTissueBlocksList = new List<Transform>();
    public Vector3 Centroid { get { return _centroid; } }
    [SerializeField] private Vector3 _centroid;
    [SerializeField] private List<GameObject> _tissueBlocks = new List<GameObject>();
    private float _min = 0f;
    private float _max = .2f;
    [SerializeField] private float _rate = .1f;
    [SerializeField] private float _explodingValue;
    private Dictionary<GameObject, TissueBlockExplodeManager> _dictObjPos = new Dictionary<GameObject, TissueBlockExplodeManager>();

    private void OnEnable()
    {
        _explodeTissueBlocks.action.performed += ExplodeTissueBlocks;
    }

    private void OnDestroy()
    {
        _explodeTissueBlocks.action.performed -= ExplodeTissueBlocks;
    }

    private void ExplodeTissueBlocks(InputAction.CallbackContext ctx)
    {
        _explodingValue += ctx.action.ReadValue<Vector2>().y * _rate * Time.deltaTime;
        _explodingValue = _explodingValue.ClampFloat(_min, _max);

        for (int i = 0; i < _explodeTissueBlocksList.Count; i++)
        {
            _explodeTissueBlocksList[i].GetComponent<TissueBlockExplodeManager>().ExplodeValue += ctx.action.ReadValue<Vector2>().y * _rate * Time.deltaTime;
            _explodeTissueBlocksList[i].GetComponent<TissueBlockExplodeManager>().ExplodeValue = _explodeTissueBlocksList[i].GetComponent<TissueBlockExplodeManager>().ExplodeValue.ClampFloat(_min, _max);

            Vector3 destination = -Vector3.Normalize(_centroid - _explodeTissueBlocksList[i].GetComponent<TissueBlockExplodeManager>().DefaultPosition);
            _explodeTissueBlocksList[i].transform.position = Vector3.Lerp(
             _explodeTissueBlocksList[i].GetComponent<TissueBlockExplodeManager>().DefaultPosition,
              destination + _centroid,
              _explodeTissueBlocksList[i].GetComponent<TissueBlockExplodeManager>().ExplodeValue
          );
        }
    }

    private void Awake()
    {
        _sphere = GetComponent<Transform>();
        _max = GetComponent<AdjustSphereSize>().SphereSizeMax;
        _centroid = Utils.ComputeCentroid(_explodeTissueBlocksList);
    }

    //find all GOs with TB data under parent, compute centroid once!
    void GetAllTissueBlocksInOrgan() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TissueBlockData>() != null)
        {
            _explodeTissueBlocksList.Add(other.gameObject.transform);
            _centroid = Utils.ComputeCentroid(_explodeTissueBlocksList);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<TissueBlockData>() != null)
        {
            _explodeTissueBlocksList.Remove(other.gameObject.transform);
            _centroid = Utils.ComputeCentroid(_explodeTissueBlocksList);
        }
    }

}
