using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;

using Random = UnityEngine.Random;

namespace HRAOrganGallery
{
    /// <summary>
    /// A class to visualize cells in 3D with "biomarker trees" as bars
    /// </summary>
    public class VisualizerBiomarkers : VisualizerBase
    {
        [Header("3D Objects")]
        [SerializeField]
        private Transform prefabDot;

        [SerializeField]
        private Transform prefabBar;

        [SerializeField]
        private float radius = .1f;

        List<Transform> cells = new List<Transform>();

        List<(float, float)> pointsOnCircle = new List<(float, float)>();

        [Header("Data")]
        [SerializeField]
        private SOCellPositionListWithBiomarkers cellList;

        [SerializeField]
        private int numberOfCells = 1000;

        [SerializeField]
        private int numberOfBiomarkerSets = 8;

        private void Awake()
        {
            CreateCells();
            //CreateBars (cells);
        }

        private void CreateCells()
        {
            for (int i = 0; i < cellList.cells.Count; i++)
            {
                CellWithBiomarkers currentCell = cellList.cells[i];
                Debug.Log($"Now making {currentCell.label} at {currentCell.position} with {currentCell.biomarkers.Count}");

                //refactor this so it now uses data from the CellWithBiomarkers class from currentCell
                Transform newCell =
                    GameObject
                        .Instantiate(prefabDot,
                        new Vector3(Random.Range(-3.0f, 3.0f),
                            0,
                            Random.Range(-3.0f, 3.0f)),
                        Quaternion.identity);
                newCell.Rotate(new Vector3(-90, 0, 0));
                newCell.localScale = new Vector3(.15f, .15f, .15f);
                cells.Add(newCell);
            }
        }

        void CreateBars(List<Transform> cells)
        {
            cells
                .ForEach(c =>
                {
                    float cx = c.position.x; // X coordinate of the center
                    float cz = c.position.z; // Y coordinate of the center

                    int n = numberOfBiomarkerSets; // Number of points (e.g., 8 points for equal division)

                    pointsOnCircle = GetCirclePoints(cx, cz, radius, n);

                    pointsOnCircle
                        .ForEach(p =>
                        {
                            Transform bar =
                                GameObject
                                    .Instantiate(prefabBar,
                                    new Vector3(p.Item1, 0, p.Item2),
                                    Quaternion.identity);
                        });
                });
        }

        // private void Update()
        // {
        //     pointsOnCircle
        //         .ForEach(p =>
        //         {
        //             Debug
        //                 .DrawRay(prefabDot.position,
        //                 new Vector3(p.Item1, 0, p.Item2),
        //                 Color.red);
        //         });
        // }

        private float[]
        GetAngle(Transform dot, int i, int numberOfPoints, float radius)
        {
            //initialize float array to return x, y position of bar
            float[] result = new float[2];

            //compute angle
            float angle = (2 * Mathf.PI * i) / numberOfPoints;

            //compute coordinates
            float x = dot.position.x + radius * Mathf.Cos(angle);
            float y = dot.position.y + radius * Mathf.Sin(angle);

            //return result
            result[0] = x;
            result[1] = y;

            return result;
        }

        private List<(float, float)>
        GetCirclePoints(float cx, float cy, float radius, int n)
        {
            //List<(float, float)> points = new List<(float, double)>();
            for (int i = 0; i < n; i++)
            {
                // Calculate the angle for each point
                float angle = 2 * Mathf.PI * i / numberOfBiomarkerSets;

                // Calculate x and y coordinates
                float x = cx + radius * Mathf.Cos(angle);
                float y = cy + radius * Mathf.Sin(angle);

                // Add the point to the list
                pointsOnCircle.Add((x, y));
            }

            return pointsOnCircle;
        }

        public override void PrepareScaling()
        {
            throw new NotImplementedException();
        }

        public override void BuildVisualization()
        {
            throw new NotImplementedException();
        }
    }
}
