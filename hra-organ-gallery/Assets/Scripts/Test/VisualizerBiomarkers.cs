using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HRAOrganGallery
{
    /// <summary>
    /// A class to visualize cells in 3D with "biomarker trees" as bars
    /// </summary>
    public class VisualizerBiomarkers : MonoBehaviour
    {
        [Header("3D Objects")]
        [SerializeField] private Transform prefabDot;
        [SerializeField] private Transform prefabBar;
        [SerializeField] private float radius = .1f;
        List<(float, float)> points = new List<(float, float)>();

        [Header("Data")]
        [SerializeField] private SOCellPositionList list;
        [SerializeField] private Dictionary<string, float> biomarkerDict = new Dictionary<string, float> { { "Biomarker 1", 0.3f } };
        private int numberOfBiomarkerSets = 8;

        private void Awake()
        {
            float cx = 0; // X coordinate of the center
            float cy = 0; // Y coordinate of the center
            int n = 8; // Number of points (e.g., 8 points for equal division)

            points = GetCirclePoints(cx, cy, radius, n);
        }

        private void Update()
        {
            points.ForEach(p => { 
                Debug.DrawRay(prefabDot.position, new Vector3(p.Item1, 0, p.Item2), Color.red);
                GameObject.Instantiate(prefabBar, new Vector3(p.Item1, 0, p.Item2), Quaternion.identity);
            });
        
        }

       
        private float[] GetAngle(Transform dot, int i, int numberOfPoints, float radius)
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

        private List<(float, float)> GetCirclePoints(float cx, float cy, float radius, int n)
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
                points.Add((x, y));
            }

            return points;
        }
    }
}
