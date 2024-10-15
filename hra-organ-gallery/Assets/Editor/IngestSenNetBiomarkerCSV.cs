using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using Assets.Scripts.Shared;
using System.Linq;

namespace HRAOrganGallery
{
    public class IngestSenNetBiomarkerCSV : EditorWindow
    {
        //Adjust this depending on the number of columns in the CSV you wish to ingest***
        private static int _numberOfColumns = 4;

        //fill in the file name here
        private static string sourceFileName = "CU034-U54-HRA-058-A_coords_scores";

        //decrease to read in **more** rows from the cell position CSV file
        private static int _readIterator = 1;

        private static string sourceFolder = "Assets/Resources/";

        private static string frequencyPostfix = "_frequency";

        private static string savedAssetFolder = "Assets/Resources";

        private static bool _createDatasetCellTypeFrequency = true;
        private int selectedOption = 0; // Index of the selected option
        private string[] options = new string[] { "TRUE", "FALSE" };

        private string
            description =
                "Ingest a list of cells with columns for different senescence biomarkers.";

        /// <summary>
        /// A static method that is called when the corresponding menu emtry is selected
        /// </summary>
        [MenuItem("Tools/4. Visualize Biomarkers/1. Ingest SenNet Biomarker CSV")]
        public static void ShowWindow()
        {
            GetWindow
            <IngestSenNetBiomarkerCSV>("Ingest Cell and Senescence Biomarkers as Scriptable Object");
        }

        private void OnGUI()
        {
            GUILayout.Label("Read CSV", EditorStyles.boldLabel);

            // Display the description with word wrapping
            EditorGUILayout
                .LabelField(description, Utils.GetStyleForDescription());

            sourceFileName = EditorGUILayout.TextField("Source File Name", sourceFileName);
            sourceFolder = EditorGUILayout.TextField("Source Folder", sourceFolder);
            frequencyPostfix = EditorGUILayout.TextField("Frequency Postfix", frequencyPostfix);
            savedAssetFolder = EditorGUILayout.TextField("Save to Folder", savedAssetFolder);

            //not super elegant and safe yet --needs fix
            _numberOfColumns = int.Parse(EditorGUILayout.TextField("Number of Columns", _numberOfColumns.ToString()));

            //not super elegant and safe yet --needs fix
            _readIterator = int.Parse(EditorGUILayout.TextField("Read Iterator", _readIterator.ToString()));

            // Display the dropdown menu
            selectedOption = EditorGUILayout.Popup("Select Option", selectedOption, options);
            // Convert the selected option to a boolean
            _createDatasetCellTypeFrequency = selectedOption == 0;
            // Display the current selection as a label
            EditorGUILayout.LabelField("Create Frequency for Cell Types (Generates Scriptable Object)?" + (_createDatasetCellTypeFrequency ? "TRUE" : "FALSE"));

            if (GUILayout.Button("Transform to Scriptable Object"))
            {
                ReadCellCSV();
            }
        }

        public static void ReadCellCSV()
        {
            List<string> allLines =
                File
                    .ReadAllLines($"{sourceFolder}/{sourceFileName}.csv").ToList();
            SOCellPositionList list =
                ScriptableObject.CreateInstance<SOCellPositionList>();

            // initialize counter to name each asset individually
            int iterator = 0;

            allLines
                .ForEach(line =>
                {
                    if (line.Split(',')[0] != "barcode")
                    {
                        if (iterator % _readIterator == 0)
                        {
                            string x = line.Split(',')[2];
                            string y = "0";
                            string z = line.Split(',')[3];
                            string label = line.Split(',')[1];

                            Cell newCell = new Cell();
                            newCell.Init(x, y, label, z);

                            list.cells.Add(newCell);
                        }
                    }

                    //increment counter
                    iterator++;
                });

            if (_createDatasetCellTypeFrequency)
            {
                AssetDatabase
                    .CreateAsset(GetCellTypeFrequency(list),
                    $"{savedAssetFolder}/{sourceFileName}{frequencyPostfix}.asset");
                AssetDatabase.SaveAssets();
            }

            AssetDatabase
                .CreateAsset(list,
                $"{savedAssetFolder}/{sourceFileName}.asset");
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// Get cell type frequency as a Scritpable Object
        /// </summary>
        /// <param name="list">a SOCellPositionList</param>
        /// <returns>SODatasetCellTypeFrequency as SO</returns>
        private static SODatasetCellTypeFrequency
        GetCellTypeFrequency(SOCellPositionList list)
        {
            Dictionary<string, int> frequencyDictionary =
                new Dictionary<string, int>();

            //go through all cells and populate dict with kvps
            list
                .cells
                .ForEach(c =>
                {
                    if (frequencyDictionary.ContainsKey(c.type))
                    {
                        frequencyDictionary[c.type]++;
                    }
                    else
                    {
                        frequencyDictionary.Add(c.type, 1);
                    }
                });

            //create Scriptable Object
            SODatasetCellTypeFrequency frequencySO =
                ScriptableObject.CreateInstance<SODatasetCellTypeFrequency>();

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

