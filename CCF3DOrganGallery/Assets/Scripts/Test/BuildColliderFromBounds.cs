using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildColliderFromBounds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Utils.FitToChildren(this.gameObject);
    }

   
}
