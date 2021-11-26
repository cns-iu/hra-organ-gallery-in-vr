using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSphere : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = Random.onUnitSphere;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
