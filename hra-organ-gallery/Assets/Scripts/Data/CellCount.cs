using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Data
{
    public class CellCount : ScriptableObject
    {
        [SerializeField]
        public List<Row> rows = new List<Row>();
    }

    [Serializable]
    public class Row
    {
        public string block;
        public string dataset;
        public string cellId;
        public string cellLabel;
        public int count;

        public Row(string b, string d, string i, string l, int c)
            => (block, dataset, cellId, cellLabel, count) = (b, d, i, l, c);
    }
}