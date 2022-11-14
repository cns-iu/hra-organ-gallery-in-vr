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
    ///  Call this method on the parent wrapper and not the actual organ model containing the component models.
    /// </summary>
    /// <param name="m"></param>
    public static void FitToChildren(GameObject m)
    {
        BoxCollider bc = m.AddComponent<BoxCollider>() as BoxCollider;
        //bc.isTrigger = true;
        if (m.GetComponent<Collider>() is BoxCollider)
        {
            bool hasBounds = false;
            Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
            for (int i = 0; i < m.transform.childCount; ++i)
            {
                Renderer childRenderer = m.transform.GetChild(i).GetComponent<Renderer>();
                if (childRenderer != null)
                {
                    if (hasBounds)
                    {
                        bounds.Encapsulate(childRenderer.bounds);
                    }
                    else
                    {
                        bounds = childRenderer.bounds;
                        hasBounds = true;
                    }
                }
            }
            //Debug.Log(bounds.size);
            BoxCollider collider = (BoxCollider)m.GetComponent<Collider>();
            collider.center = bounds.center - m.transform.position;
            collider.size = bounds.size;
        }
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
