using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public class SOCellPositionListWithBiomarkers : ScriptableObject
    {
        public List<CellWithBiomarkers> cells = new List<CellWithBiomarkers>();
    }

    public class CellWithBiomarkers : Cell
    {
        //public Vector3 position;
        //public string label;
        //private string x, y, z;
        public List<BiomarkerValuePair> biomarkers = new List<BiomarkerValuePair>();

        /// <summary>
        /// An constructor-style function to handle the setup of variables once the class has been instantiated
        /// </summary>
        /// <param name="xArg">x position</param>
        /// <param name="yArg">y position</param>
        /// <param name="labelArg">cell label</param>
        /// <param name="zArg">z position (0 by default)</param>
        public void Init(string xArg, string yArg, string labelArg, List<BiomarkerValuePair> biomarkers, string zArg = "0.0")
        {
            //fill variables
            (x, y, label, z) = (xArg, yArg, labelArg, zArg);

            //make position
            position = new Vector3(float.Parse(x), float.Parse(y), float.Parse(z));
        }
    }

    [Serializable]
    public class BiomarkerValuePair
    {
        public string label;
        public float value;
    }
}
