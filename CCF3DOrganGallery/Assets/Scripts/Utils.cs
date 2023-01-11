using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Utils
{
    public static void FindLeaves(Transform parent, List<GameObject> result)
    {
        if (parent.childCount == 0)
        {
            result.Add(parent.gameObject);
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

        result = default(T);
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


}
