using UnityEngine;

/// <summary>
/// A base class to read and process data for visualizations in 2D or 3D (through derived classes)
/// </summary>
public abstract class VisualizerBase : MonoBehaviour
{
    [Header("Cells and Data")]
    [SerializeField]
    public SODatasetCellTypeFrequency cellTypeFrequency;

    [SerializeField]
    protected Transform _parent; //parent to hold all cells

    [Header("Scaling")]
    [SerializeField]
    protected float _maxDesiredWidth = 1f; //set this yourself

    [SerializeField]
    protected float _scalingFactor = 1f; //set this yourself

    [SerializeField]
    protected Transform _adjustedParentPosition; //set this yourself

    [Header("Colors")]
    [SerializeField]
    protected SOColorValues _colorScheme; //SO with hex code and Color

    public abstract void PrepareScaling();

    public abstract void BuildVisualization();
}
