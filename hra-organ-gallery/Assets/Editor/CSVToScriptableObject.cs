using Assets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    /// <summary>
    /// A utility class to ingest a CSV file with query search results from https://triplydb.com/bherr/-/queries/dataset-ct-counts/3
    /// </summary>
    public class CSVToScriptableObject : MonoBehaviour
    {
        //The folder with the CSV cell counts 
        private static string root = "Assets/Resources/CellCounts";

        /// <summary>
        /// Static method to create a new menu item in the Unity editor. When clicked, it executes the code below
        /// </summary>
        [MenuItem("Utilities/IngestCellCounts")]
        public static void GetCellCounts()
        {
            //instantiate the SO
            CellCount cellCount = ScriptableObject.CreateInstance<CellCount>();

            //loop through the Resources/CellCounts folder and get all CSVs
            foreach (string file in Directory.GetFiles(root, "*.csv"))
            {
                string[] allLines = File.ReadAllLines(file);

                for (int i = 1; i < allLines.Length; i++)
                {
                    //split line into four values
                    string[] splitLine = allLines[i].Split(',');

                    //parsing the count
                    int count;

                    int.TryParse(splitLine[4].Replace("\"", ""), out count);

                    //adding rows to the CellCount SO
                    cellCount.rows.Add(new Row(
                        splitLine[0].Replace("\"", ""),
                        splitLine[1].Replace("\"", ""), 
                        splitLine[2].Replace("\"", ""), 
                        splitLine[3].Replace("\"", ""), 
                        count
                        ));



                }
                //saving the SO
                AssetDatabase.CreateAsset(cellCount, $"Assets/Resources/CellCounts/CellCounts.asset");
                AssetDatabase.SaveAssets();
            }
        }
    }
}
