using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using Assets.Scripts.Shared;
using HRAOrganGallery;

/// <summary>
/// A class to visualize an asset of type SOCellPositionList in 3D as a point cloud
/// </summary>
public class Visualizer3D : VisualizerBase
{
    [Header("Cells and Data")]
    [SerializeField]
    protected float _maxNumberOfCells = 4000;

    [SerializeField]
    protected SOCellPositionList _fullCellList; //SO with list of all cells

    [SerializeField]
    protected GameObject _preDot; //prefab for cells/dots

    [Header("Scaling")]
    [SerializeField]
    protected Vector3 _maxVectorOriginal;

    private void Awake()
    {
        PrepareScaling();

        BuildVisualization();
    }

    public override void PrepareScaling()
    {
        //get max 2D bounding
        _maxVectorOriginal = GetMaxDimensions(_fullCellList);

        //set width, height, depth
        _scalingFactor = _maxDesiredWidth / _maxVectorOriginal.x;
    }

    public override void BuildVisualization()
    {
        //instantiale cells for each entry in the cell list
        _fullCellList
            .cells
            .ForEach(cell =>
            {
                GameObject cellObj = MakeCell(cell);

                //get label, get rank in frequency
                int rank =
                    cellTypeFrequency.pairs.FindIndex(p => p.type == cell.label);

                //create new var to hold assigned color
                Color color;

                //get color by rank forem _colorScheme
                try
                {
                    color = _colorScheme.values[rank].color;
                }
                catch (System.Exception)
                {
                    color = Color.red;
                }

                //assign color
                cellObj.GetComponent<SpriteRenderer>().color = color;

                //scale position
                cellObj.transform.position = ScalePosition(cellObj.transform.position);
                //and parent it to the parent transform, then move the parent
                cellObj.transform.parent = _parent;
            });

        _parent.transform.position = _adjustedParentPosition.position;
    }

    /// <summary>
    /// Get the max x, y, z from all cells in the list
    /// </summary>
    /// <param name="list">A list of type SOCellPositionList</param>
    /// <returns>A max point in a 2D bounding box</returns>
    private Vector3 GetMaxDimensions(SOCellPositionList list)
    {
        Vector3 result = new Vector3();
        result.x = list.cells.Max(cell => cell.position.x);
        result.y = list.cells.Max(cell => cell.position.y);
        result.z = list.cells.Max(cell => cell.position.z);
        return result;
    }

    private Vector3 ScalePosition(Vector3 originalPosition)
    {
        Vector3 result = new Vector3();

        result.x =
            originalPosition
                .x
                .Remap(0,
                _maxVectorOriginal.x,
                0,
                _maxVectorOriginal.x * _scalingFactor);
        result.y =
            originalPosition
                .y
                .Remap(0,
                _maxVectorOriginal.y,
                0,
                _maxVectorOriginal.y * _scalingFactor);

        // result.z = 0f;
            originalPosition
                .z
                .Remap(0,
                _maxVectorOriginal.z,
                0,
                _maxVectorOriginal.z * _scalingFactor);

        return result;
    }

    private GameObject MakeCell(Cell cell)
    {
        GameObject cellObj =
            Instantiate(_preDot, cell.position, Quaternion.identity);

        //add CellData component to hold data, assign values to properties
        cellObj.AddComponent<CellData>();

        //set label from list entry
        cellObj.GetComponent<CellData>().CellType = cell.label;

        //set color from color scheme
        cellObj.GetComponent<CellData>().Color = Color.yellow;

        //add self increase
        cellObj.AddComponent<OnConditionIncreaseSeIf>().Init(cell.label);

        //cellObj.GetComponent<Renderer>().material.color = AssignColor(cell.cellLabel);
        return cellObj;
    }
}
