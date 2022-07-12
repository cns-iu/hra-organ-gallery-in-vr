using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TissueBlockData : MonoBehaviour
{
    [field: SerializeField]
    public string EntityId { get; set; }

    [field: SerializeField]
    public string Name { get; set; }

    [field: SerializeField]
    public string Tooltip { get; set; }

    [field: SerializeField]
    public string[] CcfAnnotations { get; set; }

    [field: SerializeField]
    public string HubmapId { get; set; }

    [field: SerializeField]
    public string DonorSex;
}
