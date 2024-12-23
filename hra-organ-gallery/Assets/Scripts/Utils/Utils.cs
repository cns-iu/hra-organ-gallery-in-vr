using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Shared
{
    /// <summary>
    /// An enum to capture cell type annotation tools
    /// </summary>
    public enum CellTypeAnnotationTool
    {
        Azimuth,
        CellTypist,
        PopV
    }
    public static class Utils
    {
        /// <summary>
        /// A method to find leaves in a nested game object and append them all to a list
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="result"></param>
        public static void FindLeaves(Transform parent, List<Transform> result)
        {
            if (parent.childCount == 0)
            {
                result.Add(parent.gameObject.transform);
            }
            else
            {
                foreach (Transform child in parent)
                {
                    FindLeaves(child, result);
                }
            }
        }

        /// <summary>
        /// A static method to reflect a transform matrix along the z-axis
        /// </summary>
        /// <returns>A reflected Matrix4v4</returns>
        public static Matrix4x4 ReflectZ()
        {
            var result = new Matrix4x4(
                new Vector4(1, 0, 0, 0),
                new Vector4(0, 1, 0, 0),
                new Vector4(0, 0, -1, 0),
                new Vector4(0, 0, 0, 1)
            );
            return result;
        }

        /// <summary>
        /// A method to set the opacity of an organ to a specific alpha value
        /// </summary>
        /// <param name="organ">The organ GameObject</param>
        /// <param name="alpha">The desiored alpha value</param>
        public static void SetOrganOpacity(GameObject organ, float alpha)
        {
            List<Transform> list = new List<Transform>();
            list = LeavesFinder.FindLeaves(organ.transform, list);

            foreach (var item in list)
            {
                Renderer renderer = item.GetComponent<MeshRenderer>();

                if (renderer == null) continue;
                Color updatedColor = renderer.material.color;
                updatedColor.a = alpha;
                renderer.material.color = updatedColor;

                Shader standard;
                //standard = Shader.Find("UniversalRenderPipeline/Lit");
                standard = Shader.Find("GLTFUtility/URP/Standard Transparent (Metallic)");
                renderer.material.shader = standard;
                MaterialExtensions.ToFadeMode(renderer.material);
            }


        }

        /// <summary>
        /// A class to read CSV files even when running on Android
        /// </summary>
        /// <param name="fileName">The name of the file to be read</param>
        /// <returns>A StreamReader</returns>
        public static StreamReader ReadTextFile(string fileName)
        {
            var asset = Resources.Load<TextAsset>(fileName);
            return new StreamReader(new MemoryStream(asset.bytes));
        }

        public static bool TryCast<T>(this object obj, out T result)
        {
            if (obj is T)
            {
                result = (T)obj;
                return true;
            }

            result = default;
            return false;
        }

        //A series of extension methods for strings (to print in console)
        public static string Bold(this string str) => "<b>" + str + "</b>";
        public static string Color(this string str, string clr) => string.Format("<color={0}>{1}</color>", clr, str);
        public static string Italic(this string str) => "<i>" + str + "</i>";
        public static string Size(this string str, int size) => string.Format("<size={0}>{1}</size>", size, str);

        /// <summary>
        /// Remaps a value from one range to another
        /// </summary>
        /// <param name="value">the value to remap</param>
        /// <param name="from1">min of range 1</param>
        /// <param name="to1">max of range 1</param>
        /// <param name="from2">min of range 2</param>
        /// <param name="to2">max of range 2</param>
        /// <returns>a remapped float</returns>
        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        /// <summary>
        /// A static function to mirror a game object
        /// </summary>
        /// <param name="go">Game object to mirror</param>
        /// <returns>An updated position</returns>
        public static Vector3 AdjustPosition(GameObject go)
        {
            Matrix4x4 reflected = Utils.ReflectX() * Matrix4x4.TRS(
                go.transform.position,
                go.transform.rotation,
                go.transform.localScale
                );

            return reflected.GetPosition();
        }

        /// <summary>
        /// A class to remove version prefixes from organ names
        /// </summary>
        /// <param name="name">A string for the reference organ name</param>
        /// <returns></returns>
        public static string CleanReferenceOrganName(string name)
        {
            if (name.Contains("V1."))
            {
                return name.Remove(name.IndexOf("V1."));
            }
            else
            {
                return name;
            }
        }

        /// <summary>
        /// A static function to mirror a transform matrix
        /// </summary>
        /// <returns>an updated Matrix4x4</returns>
        public static Matrix4x4 ReflectX()
        {
            var result = new Matrix4x4(
                new Vector4(-1, 0, 0, 0),
                new Vector4(0, 1, 0, 0),
                new Vector4(0, 0, 1, 0),
                new Vector4(0, 0, 0, 1)
            );
            return result;
        }

        public static Vector3 ComputeCentroid(List<Transform> list)
        {
            Vector3 total = Vector3.zero;
            for (int i = 0; i < list.Count; i++)
            {
                total += list[i].position;
            }
            Vector3 result = total / list.Count;
            return result;
        }
        /// <summary>
        /// Extension method to clamp float between min and max
        /// </summary>
        /// <param name="value">value to clamp</param>
        /// <param name="min">min</param>
        /// <param name="max">min</param>
        /// <returns>float</returns>
        public static float ClampFloat(this float value, float min, float max)
        {
            //return value > max ? max : (value < min ? min : value);
            if (value > max)
            {
                return max;
            }
            else if (value < min)
            {
                return min;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// A class that returns a style for showing descriptions in `EditorWindows`s
        /// </summary>
        /// <returns>A GUIStyle for descriptions in 1EditorWindows`s</returns>
        public static GUIStyle GetStyleForDescription()
        {

            // Define a custom GUIStyle for word wrapping
            GUIStyle descriptionStyle = new GUIStyle(
                 //EditorStyles.label
                );
            descriptionStyle.wordWrap = true;
            return descriptionStyle;
        }

        /// <summary>
        /// Get cell type frequency as a Scritpable Object
        /// </summary>
        /// <param name="list">a SOCellPositionList</param>
        /// <returns>SODatasetCellTypeFrequency as SO</returns>
        public static SODatasetCellTypeFrequency
        GetCellTypeFrequency(SOCellPositionList list)
        {
            Dictionary<string, int> frequencyDictionary =
                new Dictionary<string, int>();

            //go through all cells and populate dict with kvps
            list
                .cells
                .ForEach(c =>
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