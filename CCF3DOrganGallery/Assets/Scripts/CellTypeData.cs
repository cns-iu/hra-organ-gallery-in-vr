using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class CellTypeData : MonoBehaviour
{
    [SerializeField] public List<CellCount> cellCounts = new List<CellCount>();
}

[Serializable]
public class CellCount
{
    public string cellType;
    public int count;
    public float percentage;
    public string cat;
    public string sex;
    public string exp;
    public int age;
    public float yPos;
}


