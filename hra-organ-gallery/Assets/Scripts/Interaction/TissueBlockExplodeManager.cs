using Assets.Scripts.Data;
using HRAOrganGallery;
using UnityEngine;

public enum ExplodeState { Collapsed, Expanded }
public class TissueBlockExplodeManager : MonoBehaviour
{
    [field: SerializeField] public Vector3 DefaultPosition { get; set; }
    public float ExplodeValue { get; set; }
    private LineRenderer _renderer;
    [SerializeField] private Material lineMaterial;

    [SerializeField] private bool isOrganPicked = false;
    [SerializeField] private bool hasReadyBeenPicked = false;
    [SerializeField] ExplodeState explodeState = ExplodeState.Collapsed;

    private void OnEnable()
    {
        OrganCaller.OnOrganPicked += HandleOrganPick;

        AfterInteractResetOrgan.OnOrganResetClicked += () =>
        {
            ResetPosition();
        };

        //get default position once tissue blocke explode mode is on
        UserInputStateManager.OnStateChanged += (newState) =>
        {
            switch (newState)
            {
                case UserInputState.Movement:
                    ResetPosition();
                    break;
                case UserInputState.TissueBlockExplode:
                    GetDefault();
                    _renderer.enabled = true;
                    break;
                default:
                    break;
            }
        };
    }

    private void ResetPosition()
    {
        transform.position = DefaultPosition;
    }

    private void HandleOrganPick(OrganData data)
    {
        if (OrganCaller.Instance.TissueBlocks.Contains(transform))
        {
            //get defaults
            GetDefault();
        }
        // }

    }

    private void OnDestroy()
    {

    }

    //overload
    private void GetDefault()
    {
        DefaultPosition = transform.position;
    }

    private void Awake()
    {
        SetUpLines();
    }

    private void Update()
    {
        switch (explodeState)
        {
            // case ExplodeState.Collapsed:
            //     break;
            // case ExplodeState.Expanded:
            //     break;
            // default:
            //     break;
        }
        _renderer.SetPositions(new Vector3[] { transform.position, DefaultPosition });
    }

    private void SetUpLines()
    {
        _renderer = gameObject.AddComponent<LineRenderer>();
        _renderer.startWidth = .001f;
        _renderer.endWidth = .001f;
        _renderer.material = lineMaterial;
    }

}
