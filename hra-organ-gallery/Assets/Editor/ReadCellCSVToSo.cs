using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using PlasticGui.WorkspaceWindow.PendingChanges;

namespace HRAOrganGallery
{
    /// <summary>
    /// A editor class to read in a CSV file with nodes (cells), saving them into a ScriptableObject of type SOCellPositionList
    /// </summary>
    public class ReadCellCSVToSo : MonoBehaviour
    {
        //***Adjust this dependeing on the number of columns in the CSV you wish to ingest***
        private static int _numerOfColumns = 3;

        //***filln the file name here***
        private static string sourceFileName = "layer_29-nodes";

        //***decrease** to read in **more** rows from the cell position CSV file***
        private static int _readIterator = 1; 

        private static string sourceFolder = "Assets/Resources/SingleCellsNodesEdges";
        private static string frequencyPostfix = "_frequency";
        private static bool _createDatasetCellTypeFrequency = true;
        private static string savedAssetFolder = "Assets/Resources";

        /// <summary>
        /// A static method that is called when the corresponding menu emtry is selected
        /// </summary>
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
                        if (line.Split(',').Length == 4)
                        {
                            string x = line.Split(',')[0];
                            string y = line.Split(',')[1];
                            string z = line.Split(',')[2];
                            string label = line.Split(',')[3];


                            SOCellPositionList.Cell newCell = new SOCellPositionList.Cell();
                            newCell.Init(x, y, label, z);

                            list.cells.Add(newCell);
                        }
                        else if (line.Split(',').Length == 3)
                        {
                            string x = line.Split(',')[0];
                            string y = line.Split(',')[1];
                            string label = line.Split(',')[2];


                            SOCellPositionList.Cell newCell = new SOCellPositionList.Cell();
                            newCell.Init(x, y, label);

                            list.cells.Add(newCell);
                        }

                    }
                }

                //increment counter
                iterator++;
            });

            if (_createDatasetCellTypeFrequency)
            {
                AssetDatabase.CreateAsset(GetCellTypeFrequency(list), $"{savedAssetFolder}/{sourceFileName}{frequencyPostfix}.asset");
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.CreateAsset(list, $"{savedAssetFolder}/{sourceFileName}.asset");
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// Get cell type frequency as a Scritpable Object
        /// </summary>
        /// <param name="list">a SOCellPositionList</param>
        /// <returns>SODatasetCellTypeFrequency as SO</returns>
        private static SODatasetCellTypeFrequency GetCellTypeFrequency(SOCellPositionList list)
        {
            Dictionary<string, int> frequencyDictionary = new Dictionary<string, int>();

            //go through all cells and populate dict with kvps
            list.cells.ForEach(c =>
            {
                if (frequencyDictionary.ContainsKey(c.label))
                {
                    frequencyDictionary[c.label]++;
                }
                else
                {
                    frequencyDictionary.Add(c.label, 1);
                }
            });

            //create Scriptable Object
            SODatasetCellTypeFrequency frequencySO = ScriptableObject.CreateInstance<SODatasetCellTypeFrequency>();

            //populate Scriptable Object from dict
            foreach (var kvp in frequencyDictionary)
            {
                CellTypeFrequencyPair pair = new CellTypeFrequencyPair();
                pair.Init(kvp.Key, kvp.Value);

                frequencySO.pairs.Add(pair);
            }

            //sort list by cell frequency
            frequencySO.SortByCellFrequency();

            return frequencySO;
        }
    }
}
