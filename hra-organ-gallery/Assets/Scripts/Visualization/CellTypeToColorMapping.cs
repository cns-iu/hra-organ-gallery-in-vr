using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    /// <summary>
    /// A helpder class to hold a list of mappings of cell types to colors
    /// </summary>
    [Serializable]
    public class CellTypeToColorMapping
    {
        public List<CellTypeColorPair> pairs = new List<CellTypeColorPair>();
    }

    /// <summary>
    /// A helper class to hold 1 cell type -> color
    /// </summary>
    [Serializable]
    public class CellTypeColorPair
    {

        public string cellType;
        public Color color;

        public CellTypeColorPair(string type, Color col) => (cellType, color) = (type, col);
    }
}
