using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOColorValues : ScriptableObject
{
   [SerializeField] public List<HexColorPair> values = new List<HexColorPair>();
}

[Serializable]
public class HexColorPair {
    public string hexValue;
    public Color color;

    public HexColorPair(string hex, Color col) => (hexValue, color) = (hex, col);
}
