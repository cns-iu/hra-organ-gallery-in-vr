using Assets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    /// <summary>
    /// A unitility class to ingest a CSV file with query search results from https://triplydb.com/bherr/-/queries/dataset-ct-counts/3
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
            
            //loop through the root directory, get every file
            foreach (string file in Directory.GetFiles(root, "*.csv"))
            {
                //make all lines available as string array
                string[] allLines = File.ReadAllLines(file);

                //create a new instance of a CellCount SO
                CellCount cellCount = ScriptableObject.CreateInstance<CellCount>();

                //loop through lines
                for (int i = 1; i < allLines.Length; i++)
                {
                    //split line into four values
                    string[] splitLine = allLines[i].Split(',');

                    //parsing the count
                    int count;
                    Debug.Log(i);
                    int.TryParse(splitLine[4], out count);

                    //adding rows to the CellCount SO
                    cellCount.rows.Add(new Row(
                        splitLine[0], splitLine[1], splitLine[2], splitLine[3], count
                        ));
                }

                //saving the SO
                AssetDatabase.CreateAsset(cellCount, $"Assets/Resources/CellCounts/CellCounts.asset");
                AssetDatabase.SaveAssets();
            }
        }
    }
}