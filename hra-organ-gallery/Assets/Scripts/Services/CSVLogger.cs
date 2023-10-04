using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace HRAOrganGallery.Assets.Scripts
{
    public class CSVLogger : MonoBehaviour
    {
        public static CSVLogger Instance;
        private StringBuilder sb = new StringBuilder();

        /// <summary>
        /// A method to add new content to the StringBuilder
        /// </summary>
        /// <param name="add">The string to add</param>
        public void UpdateContent(string add)
        {
            sb.Append(add+",");
            SaveToFile(sb.ToString());
        }

        /// <summary>
        /// A static method to write a string to a csv file
        /// </summary>
        /// <param name="s">A string</param>
        public static void SaveToFile(string s)
        {
            // Use the CSV generation from before
            string content = s;

            // The target file path e.g.
#if UNITY_EDITOR
            var folder = Application.streamingAssetsPath;
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
#else
            var folder = Application.persistentDataPath;
#endif
            var filePath = Path.Combine(folder, $"no_match.csv");

            using (var writer = new StreamWriter(filePath, false))
            {
                writer.WriteLine(content);
            }

            // Or just
            //File.WriteAllText(content);

            Debug.Log($"CSV file written to \"{filePath}\"");

#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }
    }
}
