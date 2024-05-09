using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOCellPosition : ScriptableObject
{
    public Vector3 position;
    public string cellLabel;

    public void Init(float x, float y, float z, string label) => (this.position, this.cellLabel) = (new Vector3(x, y, z), label);
}
