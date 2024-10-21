using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using UnityEditor;
using UnityEngine;
using Assets.Scripts.Shared;

/// <summary>
/// A class for an EditorWindow to load a CSV file in the project and serialize it into a ScriptableObject. In this case, we do this for the

/// CSV file we previously downloaded via grlc/SPARQL and the HRA API. This contains data for ALL organs.
/// </summary>
public class TransformCsv : EditorWindow
{
    private static string csvPath = "Assets/Resources/as-ct-hra-pop.csv";

    private static string savePath = "Assets/ScriptableObjects";

    private static string saveName = "AsCellSummaries";

    private string
        description =
            "Use this window to read in a CSV from grlc/SPARQL, then" +
            "give a SAVE PATH and a SAVE NAME for the resulting ScriptableObject.";

    [MenuItem("Tools/1. Update hra-pop Data/2. Transform CSV to ScriptableObject")]
    public static void ShowWindow()
    {
        GetWindow<TransformCsv>("Transform to Scriptable Object");
    }

    private void OnGUI()
    {
        GUILayout.Label("Transformer", EditorStyles.boldLabel);

        // Display the description with word wrapping
        EditorGUILayout.LabelField(description, Utils.GetStyleForDescription());

        csvPath = EditorGUILayout.TextField("CSV Path", csvPath);
        savePath = EditorGUILayout.TextField("Save To", savePath);
        saveName = EditorGUILayout.TextField("Name", saveName);

        if (GUILayout.Button("Transform to Scriptable Object"))
        {
            BuildScriptableObject (csvPath);
        }
    }

    static void BuildScriptableObject(string filePath)
    {
        //initialize the Scriptable Object to hold the serialzed CSV file
        SOHraApiAsCellSummaries list =
            ScriptableObject.CreateInstance<SOHraApiAsCellSummaries>();

        //load the CSV file
        if (!File.Exists(filePath))
        {
            Debug.LogError("CSV file not found at path: " + filePath);
            return;
        }

        //declare the List of Dict to hold the content
        var records = new List<Dictionary<string, string>>();

        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csv.Read();
            csv.ReadHeader();
            var headers = csv.HeaderRecord;
            while (csv.Read())
            {
                var row =
                    new CellSummaryRow
                    {
                        organ = csv.GetField<string>("organ"),
                        asId = csv.GetField<string>("as"),
                        asLabel = csv.GetField<string>("as_label"),
                        sex = csv.GetField<string>("sex"),
                        tool = csv.GetField<string>("tool"),
                        modality = csv.GetField<string>("modality"),
                        cellId = csv.GetField<string>("cell_id"),
                        cellLabel = csv.GetField<string>("cell_label"),
                        cellCount = csv.GetField<float>("cell_count"),
                        cellPercentage = csv.GetField<float>("cell_percentage")
                    };

                list.rows.Add(row);
            }
        }


        AssetDatabase.CreateAsset(list, $"{savePath}/{saveName}.asset");
        AssetDatabase.SaveAssets();
        Debug.Log($"CSV transformed into SO and saved as {savePath}/{saveName}.asset.");
    }
}
