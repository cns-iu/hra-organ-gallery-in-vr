using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganData : MonoBehaviour
{
    [field: SerializeField]
    public string representationOf { get; set; }

    [field: SerializeField]
    public string sceneGraph { get; set; }
}
