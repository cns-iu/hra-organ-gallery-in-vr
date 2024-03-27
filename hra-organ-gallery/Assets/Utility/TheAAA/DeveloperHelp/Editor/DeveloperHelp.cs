using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace THeAAA.DevloperHelp
{
    public class DeveloperHelp
    {


        /// <summary>
        /// Clear PlayerPrefs
        /// </summary>
        [MenuItem("Tools / The AAA Tools / Clear All PlayerPrefs")]
        public static void ClearAllPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            //Show.LogGray("Clear All PlayerPrefs.");
        }


        [MenuItem("Tools / The AAA Tools / Clear All EditorPrefs")]
        public static void ClearAllEditorPrefs()
        {
            EditorPrefs.DeleteAll();
            //Show.LogGray("Clear All PlayerPrefs.");
        }


    }
}
