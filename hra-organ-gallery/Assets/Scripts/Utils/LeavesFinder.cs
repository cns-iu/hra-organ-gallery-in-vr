using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavesFinder : MonoBehaviour
{


    public static List<Transform> FindLeaves(Transform parent, List<Transform> leafArray)
    {
        // Debug.Log(parent.childCount);
        if (parent.childCount == 0)
        {
            // Debug.Log("no children");
            leafArray.Add(parent);
            // Debug.Log(parent.gameObject.name);
        }
        else
        {
            foreach (Transform child in parent)
            {
                FindLeaves(child, leafArray);
                leafArray.Add(parent);
                // Debug.Log(child.gameObject.name);
            }
        }

        return leafArray;
    }
}
