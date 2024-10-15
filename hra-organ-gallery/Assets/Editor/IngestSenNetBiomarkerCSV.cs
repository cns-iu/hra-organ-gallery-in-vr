using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using Assets.Scripts.Shared;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.UIElements;

namespace HRAOrganGallery
{
    public class IngestSenNetBiomarkerCSV : EditorWindow
    {

        //fill in the file name here
        private static string sourceFileName = "CU034-U54-HRA-058-A_coords_scores";

        //decrease to read in **more** rows from the cell position CSV file
        private static int _readIterator = 1;

        private static string sourceFolder = "Assets/Resources/";

        private static string savedAssetFolder = "Assets/Resources";

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
            savedAssetFolder = EditorGUILayout.TextField("Save to Folder", savedAssetFolder);

            //not super elegant and safe yet --needs fix
            _readIterator = int.Parse(EditorGUILayout.TextField("Read Iterator", _readIterator.ToString()));

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
            SOCellPositionListWithBiomarkers list =
                ScriptableObject.CreateInstance<SOCellPositionListWithBiomarkers>();

            // initialize counter to name each asset individually
            int iterator = 0;

            allLines
                .ForEach(line =>
                {
                    if (line.Split(',')[0] != "barcode")
                    {
                        if (iterator % _readIterator == 0)
                        {
                            //extract individual strings from CSV
                            string x = line.Split(',')[1];
                            string y = "0";
                            string z = line.Split(',')[2];
                            string label = line.Split(',')[0];
                            //List<string> linesWithBiomarkers = line.Split(',')[3..11].ToList();
                            //linesWithBiomarkers.ForEach(bioMarker => { Debug.Log($"{bioMarker}"); });

                            //instantiate new cell
                            CellWithBiomarkers newCell = new CellWithBiomarkers();

                            //construct biomarker value pair list
                            List<BiomarkerValuePair> biomarkers = new List<BiomarkerValuePair>();

                            //determine label based on column
                            for (int i = 3; i <= 10; i++)
                            {
                                BiomarkerValuePair newPair = new BiomarkerValuePair(newCell.biomarkerColumnLookup[i], float.Parse(line.Split(',')[i]));
                                biomarkers.Add(newPair);
                            }


                            //init new cell
                            newCell.Init(x, y, label, biomarkers, z);

                            //add to list of cells
                            list.cells.Add(newCell);
                        }
                    }

                    //increment counter
                    iterator++;
                });

            AssetDatabase
                .CreateAsset(list,
                $"{savedAssetFolder}/{sourceFileName}.asset");
            AssetDatabase.SaveAssets();
        }

        private void LookupBiomarkerLabel()
        {

        }
    }
}

