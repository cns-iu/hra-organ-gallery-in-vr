using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Assets.Scripts.Shared;

namespace HRAOrganGallery
{
    /// <summary>
    /// A editor class to read in a CSV file with nodes (cells), saving them into a ScriptableObject of type SOCellPositionList
    /// </summary>
    public class ReadCellCSVToSo : EditorWindow
    {
        //Adjust this depending on the number of columns in the CSV you wish to ingest***
        private static int _numberOfColumns = 4;

        //fill in the file name here
        private static string sourceFileName = "3d_cell_positions";

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
                "Read a CSV file with 3D cell positions and types to a Scriptable Object.";

        /// <summary>
        /// A static method that is called when the corresponding menu emtry is selected
        /// </summary>
        [MenuItem("Tools/2. Visualize hra-pop Data/2. IngestCellPositions")]
        public static void ShowWindow()
        {
            GetWindow
            <ReadCellCSVToSo>("Ingest Cell Positions as Scriptable Object");
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
                    .ReadAllLines($"{sourceFolder}/{sourceFileName}.csv")
                    .ToList();
            SOCellPositionList list =
                ScriptableObject.CreateInstance<SOCellPositionList>();

            // initialize counter to name each asset individually
            int iterator = 0;

            allLines
                .ForEach(line =>
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

                                Cell newCell = new Cell();
                                newCell.Init(x, y, label, z);

                                list.cells.Add(newCell);
                            }
                            else if (line.Split(',').Length == 3)
                            {
                                string x = line.Split(',')[0];
                                string y = line.Split(',')[1];
                                string label = line.Split(',')[2];

                                Cell newCell = new Cell();
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
                AssetDatabase
                    .CreateAsset(Utils.GetCellTypeFrequency(list),
                    $"{savedAssetFolder}/{sourceFileName}{frequencyPostfix}.asset");
                AssetDatabase.SaveAssets();
            }

            AssetDatabase
                .CreateAsset(list,
                $"{savedAssetFolder}/{sourceFileName}.asset");
            AssetDatabase.SaveAssets();
        }
    }
}
