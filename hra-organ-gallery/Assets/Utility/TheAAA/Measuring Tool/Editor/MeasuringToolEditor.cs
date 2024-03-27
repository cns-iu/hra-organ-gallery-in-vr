using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GridBrushBase;

namespace TheAAA.MeasuringTool
{

    [CustomEditor(typeof(MeasuringTool)), CanEditMultipleObjects]
    public class MeasuringToolEditor : Editor
    {

        SerializedProperty xAxis;
        SerializedProperty xAxisPositive;
        SerializedProperty xAxisNegative;

        SerializedProperty yAxis;
        SerializedProperty yAxisPositive;
        SerializedProperty yAxisNegative;

        SerializedProperty zAxis;
        SerializedProperty zAxisPositive;
        SerializedProperty zAxisNegative;

        SerializedProperty textColor;
        SerializedProperty textSize;

        SerializedProperty scaleType;
        SerializedProperty m_TargetPosition;
        SerializedProperty useScaleingObject;
        SerializedProperty targetObject;
        SerializedProperty betweenObjects;
        SerializedProperty scaleObjectScaleColor;

        SerializedProperty worldType;
        SerializedProperty sizeType;
        SerializedProperty scaleSize;
        SerializedProperty unit;
        SerializedProperty showScaleUnit;
        SerializedProperty showAngle;


        Color red = new Color(1f, 0.0f, .2f, 1f);
        Color green = new Color(0f, 0.9f, 0.5f, 1f);
        Color blue = new Color(0f, 0.5f, 0.9f, 1f);
        bool axisControlFolding = false;
        bool textStyleFolding = false;
        //bool customScaleObjectFolding = false;
        private void OnEnable()
        {
            xAxis = serializedObject.FindProperty("xAxis");
            xAxisPositive = serializedObject.FindProperty("xAxisPositive");
            xAxisNegative = serializedObject.FindProperty("xAxisNegative");

            yAxis = serializedObject.FindProperty("yAxis");
            yAxisPositive = serializedObject.FindProperty("yAxisPositive");
            yAxisNegative = serializedObject.FindProperty("yAxisNegative");

            zAxis = serializedObject.FindProperty("zAxis");
            zAxisPositive = serializedObject.FindProperty("zAxisPositive");
            zAxisNegative = serializedObject.FindProperty("zAxisNegative");


            textColor = serializedObject.FindProperty("textColor");
            textSize = serializedObject.FindProperty("textSize");

            scaleType = serializedObject.FindProperty("customScale");
            m_TargetPosition = serializedObject.FindProperty("m_TargetPosition");
            targetObject = serializedObject.FindProperty("targetObject");
            betweenObjects = serializedObject.FindProperty("betweenObjects");

            useScaleingObject = serializedObject.FindProperty("useScaleingObject");
            scaleObjectScaleColor = serializedObject.FindProperty("scaleObjectScaleColor");

            worldType = serializedObject.FindProperty("worldType");
            sizeType = serializedObject.FindProperty("sizeType");
            scaleSize = serializedObject.FindProperty("scaleSize");
            unit = serializedObject.FindProperty("unit");
            showScaleUnit = serializedObject.FindProperty("showScaleUnit");
            showAngle = serializedObject.FindProperty("showAngle");


        }

        protected virtual void OnSceneGUI()
        {
            if (!useCustomObject) return;

            MeasuringTool example = (MeasuringTool)target;

            EditorGUI.BeginChangeCheck();
            Vector3 newTargetPosition = example.m_TargetPosition;
            if (scaleType.enumValueIndex == (int)CustomScale.TargetPosition)
            {
                newTargetPosition = Handles.PositionHandle(example.targetPosition, Quaternion.identity);
            }


            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(example, "Change Look At Target Position");
                example.targetPosition = newTargetPosition;

            }
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Draw default inspector
            DrawDefaultInspector();
            //GUILayout.FlexibleSpace();
            // Add custom Inspector content here
            // Draw foldable section at the top

            GUILayout.BeginVertical();

            BaseicControl();



            if (showScaleUnit.boolValue) { EditorGUILayout.PropertyField(unit, new GUIContent("Unit Multiplier")); }
            CustomObjectControl();
            if (showScaleUnit.boolValue || useScaleingObject.boolValue)
            {
                TextStyleControl();

            }
            AxisControl();


            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }

        private void BaseicControl()
        {

            EditorGUILayout.PropertyField(worldType, new GUIContent("World Type"));
            EditorGUILayout.PropertyField(sizeType, new GUIContent("Size Type"));
            EditorGUILayout.PropertyField(scaleSize, new GUIContent("Scale Size"));

            EditorGUILayout.PropertyField(showScaleUnit, new GUIContent("Scale Unit"));
        }
        bool useCustomObject = false;
        private void CustomObjectControl()
        {
            useScaleingObject.boolValue = EditorGUILayout.Toggle("Custom Scale", useScaleingObject.boolValue);
            if (useScaleingObject.boolValue)
            {
                // customScaleObjectFolding = EditorGUILayout.Foldout(customScaleObjectFolding, "Custom Scale");
                //  if (customScaleObjectFolding)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.PropertyField(showAngle, new GUIContent("Angles"));

                    EditorGUILayout.PropertyField(scaleType, new GUIContent("Scale Type"));

                    if (scaleType.enumValueIndex == (int)CustomScale.TargetPosition)
                    {
                        EditorGUILayout.PropertyField(m_TargetPosition, new GUIContent("Target Position"));
                        EditorGUILayout.PropertyField(scaleObjectScaleColor, new GUIContent("Color"));
                    }
                    else if (scaleType.enumValueIndex == (int)(CustomScale.TargetObjects))
                    {
                        // Add a GameObject field
                        EditorGUILayout.PropertyField(targetObject, new GUIContent("Target Object"));
                        EditorGUILayout.PropertyField(scaleObjectScaleColor, new GUIContent("Color"));
                    }
                    else if (scaleType.enumValueIndex == (int)(CustomScale.BetweenObjects))
                    {
                        // Add a GameObject field
                        EditorGUILayout.PropertyField(betweenObjects, new GUIContent("Between Objects"));
                        EditorGUILayout.PropertyField(scaleObjectScaleColor, new GUIContent("Color"));
                    }
                    else
                    {
                        // Show.LogBlue("Else");
                        return;
                    }

                    EditorGUI.indentLevel--;
                }
                useCustomObject = true;
            }
            else
            {
                useCustomObject = false;
            }
        }

        private void TextStyleControl()
        {
            textStyleFolding = EditorGUILayout.Foldout(textStyleFolding, "Text Style");
            if (textStyleFolding)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(textSize, new GUIContent("Size"));
                EditorGUILayout.PropertyField(textColor, new GUIContent("Color"));
                EditorGUI.indentLevel--;
            }
        }

        private void AxisControl()
        {
            axisControlFolding = EditorGUILayout.Foldout(axisControlFolding, "Axis Control");
            if (axisControlFolding)
            {
                EditorGUI.indentLevel++;
                GUILayout.BeginHorizontal();
                xAxis.boolValue = EditorGUILayout.Toggle("X Axis", xAxis.boolValue);
                if (xAxis.boolValue)
                {
                    GUILayout.BeginVertical();
                    xAxisPositive.boolValue = EditorGUILayout.Toggle("Positive Scale", xAxisPositive.boolValue);
                    xAxisNegative.boolValue = EditorGUILayout.Toggle("Negative Scale", xAxisNegative.boolValue);
                    GUILayout.EndVertical();

                }
                GUILayout.EndHorizontal();

                DrawLine(red);


                GUILayout.BeginHorizontal();
                yAxis.boolValue = EditorGUILayout.Toggle("Y Axis", yAxis.boolValue);
                if (yAxis.boolValue)
                {
                    GUILayout.BeginVertical();
                    yAxisPositive.boolValue = EditorGUILayout.Toggle("Positive Scale", yAxisPositive.boolValue);
                    yAxisNegative.boolValue = EditorGUILayout.Toggle("Negative Scale", yAxisNegative.boolValue);
                    GUILayout.EndVertical();

                }
                GUILayout.EndHorizontal();

                DrawLine(green);

                GUILayout.BeginHorizontal();
                zAxis.boolValue = EditorGUILayout.Toggle("Z Axis", zAxis.boolValue);
                if (zAxis.boolValue)
                {
                    GUILayout.BeginVertical();
                    zAxisPositive.boolValue = EditorGUILayout.Toggle("Positive Scale", zAxisPositive.boolValue);
                    zAxisNegative.boolValue = EditorGUILayout.Toggle("Negative Scale", zAxisNegative.boolValue);
                    GUILayout.EndVertical();

                }
                GUILayout.EndHorizontal();

                DrawLine(blue);

                EditorGUI.indentLevel--;

            }
        }

        public void DrawLine(Color color)
        {
            // Draw a colored separator line
            Rect rect = EditorGUILayout.GetControlRect(false, 1);
            EditorGUI.DrawRect(rect, color);

        }

    }
}