using System;
using System.Collections.Generic;
using UnityEngine;

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

        [SerializeField]
        private Material lineMaterial;

        [SerializeField]
        private Mesh lineMesh;

        private List<Vector3> vertices;

        [Header("Data")]
        [SerializeField]
        private SOCellPositionListWithBiomarkers cellList;

        [SerializeField]
        private bool showAllCells = false;

        [SerializeField]
        private int numberOfCellsShown = 10;

        [SerializeField]
        private int numberOfBiomarkersToDisplay = 8;

        private void Start()
        {
            BuildVisualization();
        }

        public override void PrepareScaling()
        {
            throw new NotImplementedException();
        }

        public override void BuildVisualization()
        {
            CreateCells (cellList); //creates a list of cell game objects
            CreateBars (cellsObjects); //uses those game objects to draw biomarker trees
        }

        private void CreateCells(
            SOCellPositionListWithBiomarkers cellsWithBiomarkers
        )
        {
            numberOfCellsShown =
                showAllCells
                    ? cellsWithBiomarkers.cells.Count
                    : numberOfCellsShown;

            int M = cellsWithBiomarkers.cells.Count; // Total size
            int k = M / numberOfCellsShown; // Fractional divisor
            int timesToLoop = M / k; // Number of times to loop
            int step = k; // Step size

            for (int i = 0; i < M; i += step)
            {
                CellWithBiomarkers currentCell = cellList.cells[i];

                Transform newCell =
                    GameObject
                        .Instantiate(prefabDot,
                        currentCell.position * _scalingFactor,
                        Quaternion.identity);

                newCell
                    .gameObject
                    .AddComponent<CellDataWithBiomarkers>()
                    .Init("No Known Cell Type",
                    Color.white,
                    currentCell.biomarkers);
                cellsObjects.Add (newCell);
            }
        }

        void CreateBars(List<Transform> cells)
        {
            //initialize array for vertices
            vertices = new List<Vector3>(); // Two vertices per line

            // Create a new mesh
            lineMesh = new Mesh();

            //draw biomarker trees for each cell
            cells
                .ForEach(c =>
                {
                    float cx = c.position.x; // X coordinate of the center
                    float cz = c.position.z; // Z coordinate of the center

                    int n = numberOfBiomarkersToDisplay; // Number of points
                    
                    //initialize list to hold x and z values
                    List<(float, float)> pointsOnCircle =
                        new List<(float, float)>();

                    //get List of tuples with coords
                    pointsOnCircle = GetCirclePoints(cx, cz, radius, n);

                    // make counter for iterating through biomarkers
                    int counter = 0;

                    //draw trees for each biomarker around cell
                    pointsOnCircle
                        .ForEach(p =>
                        {
                            CellDataWithBiomarkers data =
                                c.GetComponent<CellDataWithBiomarkers>();
                            float height = data.biomarkers[counter].value;

                            Vector3 v0 = new Vector3(p.Item1, 0, p.Item2);
                            Vector3 v1 =
                                new Vector3(p.Item1, 0, p.Item2) +
                                new Vector3(0, height, 0);

                            //add vertices to list that holds all vertices
                            vertices.Add (v0);
                            vertices.Add (v1);

                            counter++;
                        });
                });

            // Assign vertices to mesh
            lineMesh.vertices = vertices.ToArray();

            // Set mesh to use line topology
            int[] indices = new int[vertices.Count];

            // Debug.Log(indices.Length);
            for (int i = 0; i < indices.Length; i++) indices[i] = i;

            lineMesh.SetIndices(indices, MeshTopology.Lines, 0);
        }

        void OnRenderObject()
        {
            // Set the material and draw the mesh
            lineMaterial.SetPass(0);
            Graphics.DrawMeshNow(lineMesh, Matrix4x4.identity);
        }

        private List<(float, float)>
        GetCirclePoints(float cx, float cz, float radius, int n)
        {
            List<(float, float)> result = new List<(float, float)>();
            for (int i = 0; i < n; i++)
            {
                // Calculate the angle for each point
                float angle = 2 * Mathf.PI * i / numberOfBiomarkersToDisplay;

                // Calculate x and z coordinates
                float x = cx + radius * Mathf.Cos(angle);
                float z = cz + radius * Mathf.Sin(angle);

                // Add the point to the list
                result.Add((x, z));
            }
            return result;
        }
    }
}
