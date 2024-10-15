using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    /// <summary>
    /// A Scriptable Object to hold a list of Cells
    /// </summary>
    public class SOCellPositionList : ScriptableObject
    {
        public List<Cell> cells = new List<Cell>();

       
    }

    /// <summary>
    /// A helper class defining a cell in 3D
    /// </summary>
    [Serializable]
    public class Cell
    {
        public Vector3 position;
        public string type;
        private string x, y, z;

        /// <summary>
        /// An constructor-style function to handle the setup of variables once the class has been instantiated
        /// </summary>
        /// <param name="xArg">x position</param>
        /// <param name="yArg">y position</param>
        /// <param name="labelArg">cell label</param>
        /// <param name="zArg">z position (0 by default)</param>
        public void Init(string xArg, string yArg, string labelArg, string zArg = "0.0")
        {
            //fill variables
            (x, y, type, z) = (xArg, yArg, labelArg, zArg);

            //make position
            position = new Vector3(float.Parse(x), float.Parse(y), float.Parse(z));
        }
    }

