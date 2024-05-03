using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public class SOCellPositionList : ScriptableObject
    {
        public List<Cell> cells =  new List<Cell>();

        [Serializable]
        public class Cell
        {
            public Vector3 _position;
            public string _label;

            public void Init(Vector3 position, string label) => (_position, _label) = (position, label);
        }
    }
    
}
