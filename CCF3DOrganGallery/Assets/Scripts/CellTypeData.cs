using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CellTypeData : MonoBehaviour
{
    [FormerlySerializedAs("cell_type")] public List<string> cellType;
    public List<int> count;
    public List<float> percentage;
    public List<string> cat;
    public List<string> sex;
    public List<string> exp;
    public List<int> age;
    [FormerlySerializedAs("y_pos")] public List<float> yPos;
}
