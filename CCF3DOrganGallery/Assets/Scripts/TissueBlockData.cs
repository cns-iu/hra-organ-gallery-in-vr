using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TissueBlockData : MonoBehaviour
{
    [field: SerializeField]
    public string EntityId { get; set; }

    [field: SerializeField]
    public float[] TransformMatrix { get; set; }
}
