using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using TheAAA.CalculateAngle;

namespace TheAAA.MeasuringTool
{
    /// <summary>
    /// scale megaring type
    /// </summary>
    public enum DistanceType
    {
        Meter = 1,
        Feet = 2,
        Inch = 3,
        Centimeter = 4,
        Yard = 5
    }

    /// <summary>
    /// Scale pevot type
    /// </summary>
    public enum WorldType
    {
        Global = 0,
        Local = 1
    }

    public enum CustomScale
    {
        TargetPosition = 0,
        TargetObjects = 1,
        BetweenObjects = 2
    }

    /// <summary>
    /// This script is for create a scaling tool in editor 3D space
    /// 
    /// </summary>
    public class MeasuringTool : MonoBehaviour
    {

        [HideInInspector] public WorldType worldType; // see enum 
        [HideInInspector] public DistanceType sizeType; // see enum

        [Range(0, 1000)]
        [HideInInspector] public int scaleSize = 100; // scale size

        [Range(1, 100)]
        [HideInInspector] public int unit = 1; // Show scale per Unit

        [HideInInspector] public bool showScaleUnit = false; // enable or disable scale 

        [HideInInspector] public bool xAxis = false; // enable or disable axis
        [HideInInspector] public bool xAxisPositive = true;
        [HideInInspector] public bool xAxisNegative = true;

        [HideInInspector] public bool yAxis = false; // enable or disable axis
        [HideInInspector] public bool yAxisPositive = true;
        [HideInInspector] public bool yAxisNegative = true;

        [HideInInspector] public bool zAxis = false; // enable or disable axis
        [HideInInspector] public bool zAxisPositive = true;
        [HideInInspector] public bool zAxisNegative = true;

        [HideInInspector] public CustomScale customScale;
        public Vector3 targetPosition { get { return m_TargetPosition; } set { m_TargetPosition = value; } }
        [HideInInspector] public Vector3 m_TargetPosition = new Vector3(0.3f, 0f, 0.5f);
        [HideInInspector] public List<Transform> targetObject;
        [HideInInspector] public List<Transform> betweenObjects;

        [HideInInspector] public bool useScaleingObject; // enable or disable castom resizable
        [HideInInspector] public Color scaleObjectScaleColor = Color.magenta; // Castom resizable scale color


        [HideInInspector] public GUIStyle textStyle; // scale text style
        [HideInInspector] public Color textColor = Color.white;
        [HideInInspector] public bool showAngle = false;
        [Range(1, 48)]
        [HideInInspector] public int textSize = 12;



        private static float feetToMeter = 0.3048f;
        private static float InchToMeter = 0.0254f;
        private static float cmToMeter = 0.01f;
        private static float ydToMeter = 0.9144f;

#if UNITY_EDITOR

        /// <summary>
        /// This function always call in editor update
        /// </summary>
        public void OnDrawGizmos()
        {
            if (worldType == WorldType.Global) transform.position = Vector3.zero;
            if (showScaleUnit | useScaleingObject)
            {
                textStyle.normal.textColor = textColor;
                textStyle.fontSize = textSize;
            }
            transform.eulerAngles = Vector3.zero;
            transform.localScale = Vector3.one;
            XAxisLine();
            YAxisLine();
            ZAxisLine();

            ScaleObject();


        }
        float convartValue = 0;
        Vector3 tempTragetPosition = Vector3.one;
        /// <summary>
        /// Castom resizable scale system
        /// </summary>

        private void ScaleObject()
        {
            if (!useScaleingObject) return;
            // if (scalingObject == null) return;
            if (customScale == CustomScale.TargetObjects && 0 < targetObject.Count)
            {
                foreach (Transform obj in targetObject)
                {
                    if (!obj) continue;
                    ScaleObejectCalculation(transform.position, obj.position);
                }
                return;
            }
            else if (customScale == CustomScale.TargetPosition)
            {
                ScaleObejectCalculation(transform.position, targetPosition);
            }

            else if (customScale == CustomScale.BetweenObjects && betweenObjects.Count > 0)
            {
                BetweenObjects(betweenObjects);
            }


        }

        private void BetweenObjects(List<Transform> objects)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] == null) continue;
                Label(objects[i].position, "Index " + i);
            }
            for (int i = 1; i < objects.Count; i++)
            {
                if (!objects[i] || !objects[i - 1]) continue;

                ScaleObejectCalculation(objects[i - 1].position, objects[i].position);
            }
        }

        private void ScaleObejectCalculation(Vector3 start, Vector3 end)
        {


            targetPosition = end;

            float dis = Vector3.Distance(start, m_TargetPosition);

            string type = "";
            if (sizeType == DistanceType.Meter) { convartValue = 1; type = "m"; }
            else if (sizeType == DistanceType.Feet) { convartValue = feetToMeter; type = "ft"; }
            else if (sizeType == DistanceType.Inch) { convartValue = InchToMeter; type = "in"; }
            else if (sizeType == DistanceType.Centimeter) { convartValue = cmToMeter; type = "cm"; }
            else if (sizeType == DistanceType.Yard) { convartValue = ydToMeter; type = "yd"; }
            dis = dis / convartValue;

            string angle = "";
            if (showAngle)
            {
                float XY = 180 + TheAAA.CalculateAngle.CalculateAngle.XY(start, m_TargetPosition);
                float YZ = 180 + TheAAA.CalculateAngle.CalculateAngle.YZ(start, m_TargetPosition);
                float XZ = 180 + TheAAA.CalculateAngle.CalculateAngle.XZ(start, m_TargetPosition);
                                 
                float YX = 180 + TheAAA.CalculateAngle.CalculateAngle.YX(start, m_TargetPosition);
                float ZY = 180 + TheAAA.CalculateAngle.CalculateAngle.ZY(start, m_TargetPosition);
                float ZX = 180 + TheAAA.CalculateAngle.CalculateAngle.ZX(start, m_TargetPosition);

                XY = XY == 360 ? 0 : XY;
                YZ = YZ == 360 ? 0 : YZ;
                XZ = XZ == 360 ? 0 : XZ;

                YX = YX == 360 ? 0 : YX;
                ZY = ZY == 360 ? 0 : ZY;
                ZX = ZX == 360 ? 0 : ZX;

                angle = "\n  Angle Between:";
                angle += "\n   XY: " + XY + "° YX: " + YX;
                angle += "°\n   YZ: " + YZ + "° ZY: " + ZY;
                angle += "°\n   ZX: " + ZX + "° XZ: " + XZ;
                angle += "°";
            }
            Label(m_TargetPosition, "  \nDistance: " + dis.ToString() + type + angle);
            Gizmos.color = scaleObjectScaleColor;
            Gizmos.DrawLine(start, m_TargetPosition);

            Handles.Label(m_TargetPosition, "▪", gUI);
        }

        /// <summary>
        /// Scale Z Axis
        /// </summary>
        private void ZAxisLine()
        {

            if (zAxis)
            {
                Gizmos.color = Color.blue;
                Line(Vector3.forward, Vector3.back, transform.position, zAxisPositive, zAxisNegative);
                LineScaleLabel(Vector3.forward, Vector3.back, transform.position, zAxisPositive, zAxisNegative);
            }
        }


        /// <summary>
        /// Scale Y Axis
        /// </summary>
        private void YAxisLine()
        {
            if (yAxis)
            {
                Gizmos.color = Color.green;
                Line(Vector3.up, Vector3.down, transform.position, yAxisPositive, yAxisNegative);
                LineScaleLabel(Vector3.up, Vector3.down, transform.position, yAxisPositive, yAxisNegative);
            }
        }


        /// <summary>
        /// Scale X Axis
        /// </summary>
        private void XAxisLine()
        {
            if (xAxis)
            {
                Gizmos.color = Color.red;
                Line(Vector3.right, Vector3.left, transform.position, xAxisPositive, xAxisNegative);
                LineScaleLabel(Vector3.right, Vector3.left, transform.position, xAxisPositive, xAxisNegative);
            }
        }

        /// <summary>
        /// Generate Gizmoz Line
        /// </summary>
        /// <param name="v1">Direction 1</param>
        /// <param name="v2">Direction 2</param>
        /// <param name="start">Start point</param>
        private void Line(Vector3 v1, Vector3 v2, Vector3 start, bool positive, bool negative)
        {

            float tempDis = 0;
            if (sizeType == DistanceType.Meter) tempDis = scaleSize;
            else if (sizeType == DistanceType.Feet) tempDis = scaleSize * feetToMeter;
            else if (sizeType == DistanceType.Inch) tempDis = scaleSize * InchToMeter;
            else if (sizeType == DistanceType.Centimeter) tempDis = scaleSize * cmToMeter;
            else if (sizeType == DistanceType.Yard) tempDis = scaleSize * ydToMeter;
            if (positive) Gizmos.DrawLine(start, start + (v1 * tempDis));
            if (negative) Gizmos.DrawLine(start, start + (v2 * tempDis));

        }

        /// <summary>
        /// Calling Scale Text in Gizmoz
        /// </summary>
        /// <param name="v1">Direction 1</param>
        /// <param name="v2">Direction 2</param>
        /// <param name="start">Start Point</param>
        private void LineScaleLabel(Vector3 v1, Vector3 v2, Vector3 start, bool positive, bool negative)
        {
            if (!showScaleUnit) return;
            if (sizeType == DistanceType.Meter) LineMeter(v1, v2, start, positive, negative);
            else if (sizeType == DistanceType.Feet) LineFoot(v1, v2, start, positive, negative);
            else if (sizeType == DistanceType.Inch) LineInch(v1, v2, start, positive, negative);
            else if (sizeType == DistanceType.Centimeter) LineCM(v1, v2, start, positive, negative);
            else if (sizeType == DistanceType.Yard) LineYard(v1, v2, start, positive, negative);
        }

        /// <summary>
        /// Calling Scale text convart to Meter
        /// </summary>
        /// <param name="v1">Direction 1</param>
        /// <param name="v2">Direction 2</param>
        /// <param name="start">Start Point</param>
        private void LineMeter(Vector3 v1, Vector3 v2, Vector3 start, bool positive, bool negative)
        {
            for (int i = 0; i <= scaleSize; i += (1 * unit))
            {
                if (negative) Label(start + v2 * i, -i + "m");
                if (positive) Label(start + v1 * i, i + "m");
            }
        }

        /// <summary>
        /// Calling Scale text convart to Feet
        /// </summary>
        /// <param name="v1">Direction 1</param>
        /// <param name="v2">Direction 2</param>
        /// <param name="start">Start Point</param>
        private void LineFoot(Vector3 v1, Vector3 v2, Vector3 start, bool positive, bool negative)
        {
            int ft = 0;
            for (float i = 0; ft <= scaleSize; i += (feetToMeter * unit))
            {
                if (positive) Label(start + v1 * i, ft + "ft");
                if (negative) Label(start + v2 * i, -ft + "ft");
                ft += unit;
            }
        }

        /// <summary>
        /// Calling Scale text convart to Inch
        /// </summary>
        /// <param name="v1">Direction 1</param>
        /// <param name="v2">Direction 2</param>
        /// <param name="start">Start Point</param>
        private void LineInch(Vector3 v1, Vector3 v2, Vector3 start, bool positive, bool negative)
        {
            int inch = 0;
            for (float i = 0; inch <= scaleSize; i += (InchToMeter * unit))
            {
                if (positive) Label(start + v1 * i, inch + "in");
                if (negative) Label(start + v2 * i, -inch + "in");
                inch += unit;
            }
        }
        /// <summary>
        /// Calling Scale text convart to CM
        /// </summary>
        /// <param name="v1">Direction 1</param>
        /// <param name="v2">Direction 2</param>
        /// <param name="start">Start Point</param>
        private void LineCM(Vector3 v1, Vector3 v2, Vector3 start, bool positive, bool negative)
        {
            int cm = 0;
            for (float i = 0; cm <= scaleSize; i += (cmToMeter * unit))
            {
                if (positive) Label(start + v1 * i, cm + "cm");
                if (negative) Label(start + v2 * i, -cm + "cm");
                cm += unit;
            }
        }

        /// <summary>
        /// Calling Scale text convart to Yard
        /// </summary>
        /// <param name="v1">Direction 1</param>
        /// <param name="v2">Direction 2</param>
        /// <param name="start">Start Point</param>
        private void LineYard(Vector3 v1, Vector3 v2, Vector3 start, bool positive, bool negative)
        {
            int yd = 0;
            for (float i = 0; yd <= scaleSize; i += (ydToMeter * unit))
            {
                if (positive) Label(start + v1 * i, yd + "yd");
                if (negative) Label(start + v2 * i, -yd + "yd");

                yd += unit;
            }
        }


        GUIStyle gUI = new GUIStyle();
        /// <summary>
        /// Drow Gizmoz Text
        /// </summary>
        /// <param name="point">Point</param>
        /// <param name="str">scale number string</param>
        private void Label(Vector3 point, object str)
        {
            gUI.alignment = TextAnchor.MiddleCenter;
            gUI.normal = textStyle.normal;
            gUI.fontSize = 20;
            Handles.Label(point, "▪", gUI);

            Handles.Label(point, "   " + str.ToString(), textStyle);

        }
#endif
    }
}