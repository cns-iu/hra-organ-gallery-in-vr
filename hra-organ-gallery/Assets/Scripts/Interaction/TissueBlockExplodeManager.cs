using Assets.Scripts.Data;
using HRAOrganGallery;
using UnityEngine;

public class TissueBlockExplodeManager : MonoBehaviour
{
    [field: SerializeField] public Vector3 DefaultPosition { get; set; }
    public float ExplodeValue { get; set; }
    private LineRenderer _renderer;
    [SerializeField] private Material lineMaterial;

    [SerializeField] private bool isTissueBlockModeActive = false;
    [SerializeField] private bool isOrganPicked = false;

    private void OnEnable()
    {
        //get default position once tissue blocke explode mode is on
        UserInputStateManager.OnStateChanged += GetDefault;

        UserInputStateManager.OnStateChanged += (newState) =>
        {
            isTissueBlockModeActive = newState == UserInputState.TissueBlockExplode;
            _renderer.enabled = isTissueBlockModeActive;
        };
    }

    private void OnDestroy()
    {
        UserInputStateManager.OnStateChanged -= GetDefault;
    }

    private void GetDefault(UserInputState newState)
    {
        if (newState == UserInputState.TissueBlockExplode) DefaultPosition = transform.position;
    }

    private void Awake()
    {
        SetUpLines();
    }

    private void Update()
    {
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
