using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellData : MonoBehaviour
{
    [field: SerializeField]
    public string CellType { get; set; }

    [field: SerializeField]
    public Color Color { get; set; }

    private string _type;

    private Color _color;
}
