using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HRAOrganGallery
{
    public class SODatasetCellTypeFrequency : ScriptableObject
    {
        public List<CellTypeFrequencyPair> pairs = new List<CellTypeFrequencyPair>();

        public void SortByCellFrequency()
        {
            pairs = pairs.OrderBy(o => o.frequency).Reverse().ToList();
        }

       
    }

    [Serializable]
    public class CellTypeFrequencyPair
    {
        public string type;
        public int frequency;

        public void Init(string cellType, int cellFrequency) => (type, frequency) = (cellType, cellFrequency);
    }
}
