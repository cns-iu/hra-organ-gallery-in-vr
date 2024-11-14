using Assets.Scripts.Data;
using Assets.Scripts.Shared;
using HRAOrganGallery;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TissueBlockExploder : MonoBehaviour
{
    [Header("Sphere")]
    [SerializeField] private Transform _sphere;

    [Header("Explode tissue blocks")]
    [SerializeField] private InputActionReference _explodeTissueBlocks;
    [SerializeField] private List<Transform> _explodeTissueBlocksList = new List<Transform>();
    public Vector3 Centroid { get { return _centroid; } }
    [SerializeField] private Vector3 _centroid;
    [SerializeField] private List<Transform> _tissueBlocks = new List<Transform>();
    private float _min = 0f;
    private float _max = .2f;
    [SerializeField] private float _rate = .1f;
    [SerializeField] private float _explodingValue;
    private Dictionary<GameObject, TissueBlockExplodeManager> _dictObjPos = new Dictionary<GameObject, TissueBlockExplodeManager>();

    private void OnEnable()
    {
        _explodeTissueBlocks.action.performed += ExplodeTissueBlocks;
        OrganCaller.OnOrganPicked += GetAllTissueBlocks;
        AfterInteractResetOrgan.OnOrganResetClicked += ResetAllExplosion;
        UserInputStateManager.OnStateChanged += HandleModeSwitch;

        //get current organ etc.
        GetAllTissueBlocks();
    }

    private void OnDestroy()
    {
        _explodeTissueBlocks.action.performed -= ExplodeTissueBlocks;
        OrganCaller.OnOrganPicked -= GetAllTissueBlocks;
        AfterInteractResetOrgan.OnOrganResetClicked -= ResetAllExplosion;
        UserInputStateManager.OnStateChanged -= HandleModeSwitch;
    }

    private void ResetAllExplosion()
    {
        //reset explode value
        _explodingValue = 0;

        //reset sphere size
        ResetSphere();

        //reset tissue blocks
        _tissueBlocks.ForEach(
            t =>
            {
                t.position = t.gameObject.GetComponent<TissueBlockExplodeManager>().DefaultPosition;
            }
        );
    }

    private void HandleModeSwitch(UserInputState newState)
    {
        if (newState == UserInputState.Movement)
        {
            ResetAllExplosion();
        }

    }

    //overload GetAllTissueBlocks(OrganData data) so it can be called independently of the event from OrganCaller
    private void GetAllTissueBlocks()
    {
        //get all tissue blocks and add them to _tissueBlocks
        _tissueBlocks = OrganCaller.Instance.TissueBlocks;
        _centroid = Utils.ComputeCentroid(_tissueBlocks);
    }

    private void GetAllTissueBlocks(OrganData data)
    {
        //get all tissue blocks and add them to _tissueBlocks
        _tissueBlocks = OrganCaller.Instance.TissueBlocks;
        _centroid = Utils.ComputeCentroid(_tissueBlocks);
    }

    private void ResetSphere()
    {
        _sphere.GetComponent<AdjustSphereSize>().ResetSphere();
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

            _explodeTissueBlocksList[i].GetComponent<TissueBlockExplodeManager>().explodeState = ExplodeState.Expanded;
        }
    }

    private void Awake()
    {
        _sphere = GetComponent<Transform>();
        _max = GetComponent<AdjustSphereSize>().SphereSizeMax;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TissueBlockData>() != null)
        {
            _explodeTissueBlocksList.Add(other.gameObject.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<TissueBlockData>() != null)
        {
            _explodeTissueBlocksList.Remove(other.gameObject.transform);
        }
    }

}
