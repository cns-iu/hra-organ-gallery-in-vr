using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Assets.Scripts.Shared;

using static UnityEditor.EditorWindow;

public class ReadColorScheme : EditorWindow
{
    //enter the file name here
    private static string fileName = "hra_colors_ushma";

    private static string colorTextFilePath = "Assets/Resources/ColorSchemes/";

    private static string extension = ".txt";

    private string
        description =
            "Provide a path to a TXT or CSV file with hex codes to read it in a ScriptableObject.";

    [MenuItem("Tools/3. Organize Visualization/ReadColorScheme")]
    public static void ShowWindow()
    {
        GetWindow<ReadColorScheme>("Read Color Scheme");
    }

    private void OnGUI()
    {
        GUILayout.Label("Read Color Scheme", EditorStyles.boldLabel);

        // Display the description with word wrapping
        EditorGUILayout.LabelField(description, Utils.GetStyleForDescription());

        fileName = EditorGUILayout.TextField("File Name", fileName);
        colorTextFilePath =
            EditorGUILayout.TextField("File Name", colorTextFilePath);
        extension = EditorGUILayout.TextField("File Name", extension);

        if (GUILayout.Button("Transform to Scriptable Object"))
        {
            ReadColorFile();
        }
    }

    public static void ReadColorFile()
    {
        string text =
            File.ReadAllText(colorTextFilePath + fileName + extension);
        List<string> colors = text.Split(',').ToList();

        SOColorValues values = ScriptableObject.CreateInstance<SOColorValues>();
        colors
            .ForEach(c =>
            {
                string hexValue = c.Trim(new Char[] { '\'', '[', ']' });
                Color colorValue;

                if (ColorUtility.TryParseHtmlString(hexValue, out colorValue))
                {
                    values.values.Add(new HexColorPair(hexValue, colorValue));
                }
            });

        AssetDatabase.CreateAsset(values, $"Assets/Resources/ColorSchemes/{fileName}.asset");
        AssetDatabase.SaveAssets();
    }
}
