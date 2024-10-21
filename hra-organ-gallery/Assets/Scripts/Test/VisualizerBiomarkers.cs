using System;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using static Assets.Scripts.Interaction.CCFAPISPARQLQuery;
using Random = UnityEngine.Random;

namespace HRAOrganGallery
{
    /// <summary>
    /// A class to visualize cells in 3D with "biomarker trees" as bars
    /// </summary>
    public class VisualizerBiomarkers : VisualizerBase
    {
        public static VisualizerBiomarkers Instance;

        [Header("3D Objects")]
        [SerializeField]
        private Transform prefabDot;

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

        [Header("Visual Encoding")]
        [SerializeField]
        private float scalingFactorHeight = 3f;

        [SerializeField] private List<(string, HexColorPair)> legend = new List<(string, HexColorPair)>();

        private Dictionary<string, int> biomarkerColorLookup = new Dictionary<string, int>();

        [SerializeField] public CellTypeToColorMapping mapping = new CellTypeToColorMapping();

        private List<Color> treeColors = new List<Color>();

        private void Start()
        {
            BuildVisualization();
        }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        public override void PrepareScaling()
        {
            throw new NotImplementedException();
        }

        public override void BuildVisualization()
        {
            InvertBiomarkerLookup(); //create a color lookup by biomarker
            CreateColorMap();
            CreateCells(cellList); //creates a list of cell game objects
            AdjustParent(_parent); //moves all cells to preselected position
            CreateBars(cellsObjects); //uses those game objects to draw biomarker trees
        }

        private void InvertBiomarkerLookup()
        {
            // Invert the dictionary
            foreach (var kvp in CellWithBiomarkers.biomarkerColumnLookup)
            {
                biomarkerColorLookup.Add(kvp.Value, kvp.Key);
            }

        }

        private void CreateColorMap()
        {

            foreach (var kvp in biomarkerColorLookup)
            {
                mapping.pairs.Add(new CellTypeColorPair(kvp.Key, _colorScheme.values[kvp.Value-3].color));
            }
        }

        void AdjustParent(Transform parent)
        {
            _parent.position = _adjustedParentPosition.position;
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
                    currentCell.biomarkers, currentCell.meetsThresholds);

                //set parent
                newCell.parent = _parent;

                //add to list of cell objects
                cellsObjects.Add(newCell);
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
                    if (!c.GetComponent<CellDataWithBiomarkers>().meetsThresholds) return;
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
                            float height = data.biomarkers[counter].value * scalingFactorHeight;

                            GenerateVertices(p, height, data.biomarkers[counter].label);

                            counter++;
                        });
                });

            // Assign vertices to mesh
            lineMesh.vertices = vertices.ToArray();
            lineMesh.colors = treeColors.ToArray();

            // Set mesh to use line topology
            int[] indices = new int[vertices.Count];

            // Debug.Log(indices.Length);
            for (int i = 0; i < indices.Length; i++) indices[i] = i;

            lineMesh.SetIndices(indices, MeshTopology.Lines, 0);
        }

        void GenerateVertices((float, float) point, float height, string biomarkerLabel)
        {

            Vector3 v0 = new Vector3(point.Item1, 0, point.Item2);
            Vector3 v1 =
                new Vector3(point.Item1, 0, point.Item2) +
                new Vector3(0, height, 0);

            // Assign random colors for each vertex (line start and end)
            //treeColors.Add(Random.ColorHSV());
            //treeColors.Add(Random.ColorHSV());

            //get color by biomarker
            //get index from dict
            int index = biomarkerColorLookup[biomarkerLabel];

            //get color from inverted lookup
            Color color = _colorScheme.values[index-3].color;

            legend.Add((biomarkerLabel, new HexColorPair("", color)));

            //add colors to treeColors list
            treeColors.Add(color);
            treeColors.Add(color);

            //add vertices to list that holds all vertices
            vertices.Add(v0);
            vertices.Add(v1);
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
