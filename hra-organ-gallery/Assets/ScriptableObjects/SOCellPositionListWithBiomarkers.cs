using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace HRAOrganGallery
{
    public class SOCellPositionListWithBiomarkers : ScriptableObject
    {
        public List<CellWithBiomarkers> cells = new List<CellWithBiomarkers>();
    }

    [Serializable]
    public class CellWithBiomarkers : Cell
    {
        //public Vector3 position;
        //public string label;
        //private string x, y, z;
        public List<BiomarkerValuePair> biomarkers = new List<BiomarkerValuePair>();

        public static Dictionary<int, string> biomarkerColumnLookup = new Dictionary<int, string>()
        {
            { 3,"San.Diego.TMC"},
            { 4,"Resistance.to.Apoptosis"},
            { 5,"Fridman"},
            { 6,"Activated.p53.Targets"},
            { 7,"DDR"},
            { 8,"SASP"},
            { 9,"Cell.Cycle.Arrest"},
            { 10,"Senmayo"},
        };

        /// <summary>
        /// An constructor-style function to handle the setup of variables once the class has been instantiated (with biomarkers)
        /// </summary>
        /// <param name="xArg"></param>
        /// <param name="yArg"></param>
        /// <param name="labelArg"></param>
        /// <param name="biomarkersArg"></param>
        /// <param name="zArg"></param>
        public void Init(string xArg, string yArg, string labelArg, List<BiomarkerValuePair> biomarkersArg, string zArg = "0.0")
        {
            //fill variables
            (x, y, label, z) = (xArg, yArg, labelArg, zArg);

            //make position
            position = new Vector3(float.Parse(x), float.Parse(y), float.Parse(z));

            // get biomarkers and values
            biomarkers = biomarkersArg;

        }
    }

    [Serializable]
    public class BiomarkerValuePair
    {
        public string label;
        public float value;

        public BiomarkerValuePair(string labelArg, float valueArg)
        {
            //fill variables
            (label, value) = (labelArg, valueArg);
        }
    }
}
