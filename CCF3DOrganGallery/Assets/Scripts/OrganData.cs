using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganData : MonoBehaviour
{
    [field: SerializeField]
    public string RepresentationOf { get; set; }

    [field: SerializeField]
    public string SceneGraph { get; set; }

    [field: SerializeField]
    public string DonorSex { get; set; }
}
