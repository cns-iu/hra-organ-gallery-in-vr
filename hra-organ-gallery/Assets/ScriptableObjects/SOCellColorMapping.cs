using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOCellColorMapping : ScriptableObject
{
    public List<CellTypeColorCount> listOfTypeColorCountTriple = new List<CellTypeColorCount>();
}

[Serializable]
public class CellTypeColorCount
{
    public string cellType;
    public Color cellColor;
    public int cellCount;

    public CellTypeColorCount(string type, Color color, int count) => (cellType, cellColor, cellCount) = (type, color, count);
}

//[Serializable]
//public class CellTypeColorPair
//{
//    public string cellType;
//    public Color cellColor;


//    public CellTypeColorPair(string cell, string color)
//    {
//        cellType = cell;
//        ColorUtility.TryParseHtmlString(color, out cellColor);
//    }
//}