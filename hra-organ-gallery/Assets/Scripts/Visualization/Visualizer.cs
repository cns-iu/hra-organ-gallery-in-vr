using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    /// <summary>
    /// A class to visualize an asset of type SOCellPositionList in 3D as a point cloud
    /// </summary>
    public class Visualizer : MonoBehaviour
    {
        public static Visualizer Instance;


        [SerializeField] private SOCellPositionList _cellList;
        [SerializeField] private Transform _parent;
        [SerializeField] private GameObject _preDot;

        private void Awake()
        {
            //get color mapping with counts
            //_cellColorMapping = Resources.LoadAll<SOCellColorMapping>("")[0];

            BuildVisualization();

            //code for singleton instance
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }



        private void BuildVisualization()
        {
            _cellList.cells.ForEach(
                cell =>
                    {
                        GameObject cellObj = MakeCell(cell);
                        cellObj.transform.parent = _parent;
                    }
                );
        }


        private GameObject MakeCell(SOCellPositionList.Cell cell)
        {
            GameObject cellObj = Instantiate(_preDot, cell.position, Quaternion.identity);
            //cellObj.GetComponent<Renderer>().material.color = AssignColor(cell.cellLabel);
            return cellObj;
        }

        //private Color AssignColor(string type)
        //{
        //    return _cellColorMapping.listOfTypeColorCountTriple.Where(t => t.cellType == type).ToList()[0].cellColor;
        //}
    }
}
