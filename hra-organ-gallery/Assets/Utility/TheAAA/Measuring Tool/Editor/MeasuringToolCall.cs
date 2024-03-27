using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace TheAAA.MeasuringTool
{
    public class MeasuringToolCall
    {

        /// <summary>
        /// Create and a object and enable SceneScale system in it.
        /// </summary>
        [MenuItem("Tools / The AAA Tools / Measuring Tool")]
        public static void CallCreateMeasuringToolFromTools()
        {
            CreateMeasuringTool();
        }

        [MenuItem("GameObject / Measuring Tool")]
        public static void CallCreateMeasuringToolFromGameObject()
        {
            CreateMeasuringTool();
        }
        public static void CreateMeasuringTool()
        {
            GameObject obj = new GameObject("Measuring Tool");
            /*GameObject obj2 = new GameObject("ScalePoint");
            obj2.transform.parent = obj.transform;*/
            obj.AddComponent<MeasuringTool>();
            MeasuringTool measuringTool = obj.GetComponent<MeasuringTool>();

            measuringTool.xAxis = true;
            measuringTool.yAxis = true;
            measuringTool.zAxis = true;
            measuringTool.showScaleUnit = false;
            measuringTool.sizeType = DistanceType.Meter;

            /*measuringTool.scalingObject = obj2.transform;*/

            measuringTool.scaleObjectScaleColor = Color.white;

            measuringTool.textColor = Color.white;
            measuringTool.textSize = 8;

        }



    }
}