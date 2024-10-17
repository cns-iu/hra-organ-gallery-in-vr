using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;

using Random = UnityEngine.Random;
using static Assets.Scripts.Interaction.CCFAPISPARQLQuery;

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

        List<Transform> cellsObjects = new List<Transform>();

        List<(float, float)> pointsOnCircle = new List<(float, float)>();

        [SerializeField] private Material lineMaterial;
        [SerializeField] private Mesh lineMesh;
        private Vector3[] vertices;

        [Header("Data")]
        [SerializeField]
        private SOCellPositionListWithBiomarkers cellList;

        [SerializeField] private bool showAllCells = false;

        [SerializeField] private int numberOfCellsShown = 10;

        [SerializeField]
        private int numberOfBiomarkersToDisplay = 8;

        private void Awake()
        {
            BuildVisualization();
        }

        public override void PrepareScaling()
        {
            throw new NotImplementedException();
        }

        public override void BuildVisualization()
        {
            CreateCells(cellList); //creates a list of cell game objects
            CreateBars(cellsObjects); //uses those game objects to draw biomarker trees
        }

        private void CreateCells(SOCellPositionListWithBiomarkers cellsWithBiomarkers)
        {
            numberOfCellsShown = showAllCells ? cellsWithBiomarkers.cells.Count : numberOfCellsShown;

            int M = cellsWithBiomarkers.cells.Count;  // Total size
            int k = M / numberOfCellsShown;     // Fractional divisor
            int timesToLoop = M / k; // Number of times to loop
            int step = k;  // Step size

            for (int i = 0; i < M; i += step)
            {
                CellWithBiomarkers currentCell = cellList.cells[i];

                //refactor this so it now uses data from the CellWithBiomarkers class from currentCell
                Transform newCell =
                    GameObject
                        .Instantiate(prefabDot,
                       currentCell.position * _scalingFactor,
                        Quaternion.identity);


                cellsObjects.Add(newCell);
            }
        }

        void CreateBars(List<Transform> cells)
        {
            //initialize array for vertices
            vertices = new Vector3[numberOfBiomarkersToDisplay * numberOfCellsShown * 2]; // Two vertices per line

            // Create a new mesh
            lineMesh = new Mesh();

            //create iterator for adding vertices
            int iterator = 0;

            cells
                    .ForEach(c =>
                    {
                        //vertices[iterator] = new Vector3(p.Item1, 0, p.Item2);
                        //vertices[iterator + 1] = new Vector3(p.Item1, 1, p.Item2);

                        // vertices[iterator] = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
                        vertices[iterator] = c.transform.position;
                        vertices[iterator+1] = c.transform.position + new Vector3(0,1,0);
                        //vertices[iterator] = new Vector3(0,0,0);
                        //vertices[iterator + 1] = new Vector3(0, 1, 0);

                        iterator+=2;

                        float cx = c.position.x; // X coordinate of the center
                        float cz = c.position.z; // Y coordinate of the center

                        int n = numberOfBiomarkersToDisplay; // Number of points (e.g., 8 points for equal division)

                        pointsOnCircle = GetCirclePoints(cx, cz, radius, n);

                        pointsOnCircle
                            .ForEach(p =>
                            {
                                //vertices[i] = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
                                //vertices[iterator] = new Vector3(p.Item1, 0, p.Item2);
                                //vertices[iterator + 1] = new Vector3(p.Item1, 1, p.Item2);
                                //iterator += 2;
                            });
                    });

            // Generate random line vertices
            for (int i = 0; i < vertices.Length; i++)
            {
                //vertices[i] = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
            }

            // Assign vertices to mesh
            Debug.Log($"iterator: {iterator}");
            Debug.Log($"vertices.Length: {vertices.Length}");
            lineMesh.vertices = vertices;

            // Set mesh to use line topology
            int[] indices = new int[numberOfBiomarkersToDisplay * numberOfCellsShown * 2];
            for (int i = 0; i < indices.Length; i++) indices[i] = i;

            lineMesh.SetIndices(indices, MeshTopology.Lines, 0);

            //old implementation
            //cells
            //        .ForEach(c =>
            //        {
            //            float cx = c.position.x; // X coordinate of the center
            //            float cz = c.position.z; // Y coordinate of the center

            //            int n = numberOfBiomarkersToDisplay; // Number of points (e.g., 8 points for equal division)

            //            pointsOnCircle = GetCirclePoints(cx, cz, radius, n);

            //            pointsOnCircle
            //                .ForEach(p =>
            //                {
            //                    Transform bar =
            //                        GameObject
            //                            .Instantiate(prefabBar,
            //                            new Vector3(p.Item1, 0, p.Item2),
            //                            Quaternion.identity);
            //                });
            //        });
        }

        void OnRenderObject()
        {
            // Set the material and draw the mesh
            lineMaterial.SetPass(0);
            Graphics.DrawMeshNow(lineMesh, Matrix4x4.identity);
        }


        private List<(float, float)>
        GetCirclePoints(float cx, float cy, float radius, int n)
        {
            //List<(float, float)> points = new List<(float, double)>();
            for (int i = 0; i < n; i++)
            {
                // Calculate the angle for each point
                float angle = 2 * Mathf.PI * i / numberOfBiomarkersToDisplay;

                // Calculate x and y coordinates
                float x = cx + radius * Mathf.Cos(angle);
                float y = cy + radius * Mathf.Sin(angle);

                // Add the point to the list
                pointsOnCircle.Add((x, y));
            }

            return pointsOnCircle;
        }
    }
}
