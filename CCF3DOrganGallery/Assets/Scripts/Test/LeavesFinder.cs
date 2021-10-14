using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavesFinder : MonoBehaviour
{
    public List<Transform> leafArray = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        FindLeaves(this.transform);
        foreach (var item in leafArray)
        {
            Debug.Log(item.gameObject.name);
            item.gameObject.AddComponent < ExplodingViewManager >();
        }



    }

    // Update is called once per frame
    void Update()
    {

    }


    void FindLeaves(Transform parent)
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
                FindLeaves(child);
                leafArray.Add(parent);
                // Debug.Log(child.gameObject.name);
            }
        }

       
    }
}
