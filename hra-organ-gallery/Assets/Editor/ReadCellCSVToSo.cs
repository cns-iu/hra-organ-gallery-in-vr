using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;

namespace HRAOrganGallery
{
    /// <summary>
    /// A editor class to read in a CSV file with nodes (cells), saving them into a ScriptableObject of type SOCellPositionList
    /// </summary>
    public class ReadCellCSVToSo : MonoBehaviour
    {
        private static string sourceFolder = "Assets/Resources/SingleCellsNodesEdges";
        private static string sourceFileName = "phenotypes_melanoma_in_situ-nodes";
        private static int _readIterator = 1; //**decrease** to read in **more** rows from the cell position CSV file
        private static string savedAssetFolder = "Assets/Resources";

        //comment out to avoid accidental trigger; uncomment for usage
        [MenuItem("Utilities/IngestCellPositions")]


        public static void ReadCellCSV()
        {
            List<string> allLines = File.ReadAllLines($"{sourceFolder}/{sourceFileName}.csv").ToList();
            SOCellPositionList list = ScriptableObject.CreateInstance<SOCellPositionList>();

            // initialize counter to name each asset individually
            int iterator = 0;

            allLines.ForEach(line =>
            {
                if (line.Split(',')[0] != "x")
                {
                    if (iterator % _readIterator == 0)
                    {
                        string x = line.Split(',')[0];
                        string y = line.Split(',')[1];
                        string z = line.Split(',')[2];
                        string label = line.Split(',')[3];


                        SOCellPositionList.Cell newCell = new SOCellPositionList.Cell();
                        newCell.Init(new Vector3(float.Parse(x), float.Parse(y), float.Parse(z)), label);

                        Debug.Log($"Got: position = {newCell._position}, label = {newCell._label}");

                        list.cells.Add(newCell);
                    }
                }

                //increment counter
                iterator++;
            });


            AssetDatabase.CreateAsset(list, $"Assets/Resources/{sourceFileName}.asset");
            AssetDatabase.SaveAssets();
        }
    }
}
