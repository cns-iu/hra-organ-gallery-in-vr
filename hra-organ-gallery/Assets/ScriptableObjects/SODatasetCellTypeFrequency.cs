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

        /// <summary>
        /// SOrt the dict-style object by cell frequency in descending order
        /// </summary>
        public void SortByCellFrequency()
        {
            pairs = pairs.OrderBy(o => o.frequency).Reverse().ToList();
        }

        /// <summary>
        /// Get a ratioed dict-style object with cell type -> frequency * ratio
        /// </summary>
        /// <param name="maxNumberOfCells">The total number of desired cells</param>
        /// <returns></returns>
        public SODatasetCellTypeFrequency GetRatioedPairs(float maxNumberOfCells)
        {
            SODatasetCellTypeFrequency ratioed = new SODatasetCellTypeFrequency();

            //count all cells in the dict
            float total = 0;
            pairs.ForEach(p =>
            {
                Debug.Log($"original frequency for{p.type} is: {p.frequency}");
                total += p.frequency;
            });

            Debug.Log($"total original frequency: {total}");

            //calculate ratio
            float ratio = (float)maxNumberOfCells / (float)total;
            Debug.Log(ratio);

            //make deep copy of original pairs to ratioed pairs
            pairs.ForEach(original =>
            {
                CellTypeFrequencyPair newPair = new CellTypeFrequencyPair();
                newPair.type = original.type;
                newPair.frequency = original.frequency;
                ratioed.pairs.Add(newPair);
            });

            //reduce cell frquency in dict by ratio
            ratioed.pairs.ForEach(p =>
            {
                float newFrequency = p.frequency * ratio;
                p.frequency = Mathf.RoundToInt(newFrequency);
            });

            ratioed.pairs.ForEach(p =>
            {
                Debug.Log($"Before return, ratioed has {p.type} and {p.frequency}");
            });



            return ratioed;
        }

    }

    /// <summary>
    /// A class to hold cell type -> frequency pairs in a dict-style object that can be serialized in the Unity editor
    /// </summary>
    [Serializable]
    public class CellTypeFrequencyPair
    {
        public string type;
        public int frequency;

        public void Init(string cellType, int cellFrequency) => (type, frequency) = (cellType, cellFrequency);
    }
}
