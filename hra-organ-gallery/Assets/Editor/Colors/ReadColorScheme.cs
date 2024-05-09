using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;

public class ReadColorScheme : MonoBehaviour
{
    //enter the file name here
    private static string fileName = "material_500";

    private static string colorTextFilePath = "Assets/Resources/ColorSchemes/";
    private static string extension = ".txt";

    [MenuItem("Utilities/ReadColorScheme")]
    public static void ReadColorFile()
    {
        string text = File.ReadAllText(colorTextFilePath + fileName + extension);
        List<string> colors = text.Split(',').ToList();


        SOColorValues values = ScriptableObject.CreateInstance<SOColorValues>();
        colors.ForEach(c =>
        {

            string hexValue = c.Trim(new Char[] { '\'', '[', ']' });
            Color colorValue;

            if (ColorUtility.TryParseHtmlString(hexValue, out colorValue))
            {
                values.values.Add(new HexColorPair(hexValue, colorValue));
            }
        });

        AssetDatabase.CreateAsset(values, $"Assets/Resources/{fileName}.asset");
        AssetDatabase.SaveAssets();
    }
}
