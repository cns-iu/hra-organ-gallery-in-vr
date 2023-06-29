using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class Utils
    {
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

        public static StreamReader ReadCsv(string fileName)
        {
            TextAsset asset = Resources.Load<TextAsset>(fileName);
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

        public static void SetOrganOpacity(GameObject organWrapper, float alpha)
        {
            List<Transform> list = new List<Transform>();
            list = LeavesFinder.FindLeaves(organWrapper.transform.GetChild(0), list);

            foreach (var item in list)
            {
                Renderer renderer = item.GetComponent<MeshRenderer>();

                if (renderer == null) continue;
                Color updatedColor = renderer.material.color;
                updatedColor.a = alpha;
                renderer.material.color = updatedColor;

                Shader standard;
                standard = Shader.Find("Standard");
                renderer.material.shader = standard;
                renderer.material.ToFadeMode();
            }


        }
        public static Vector3 ComputeCentroid(List<GameObject> list)
        {
            Vector3 total = Vector3.zero;
            for (int i = 0; i < list.Count; i++)
            {
                total += list[i].transform.position;
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
    }
}