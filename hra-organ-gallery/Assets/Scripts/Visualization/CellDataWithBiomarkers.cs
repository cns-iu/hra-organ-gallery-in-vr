using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public class CellDataWithBiomarkers : CellData
    {
        public bool meetsThresholds = false;
        public List<BiomarkerValuePair> biomarkers = new List<BiomarkerValuePair>();


        /// <summary>
        /// A constructor-like function to initialize an object of this class
        /// </summary>
        /// <param name="cellType"></param>
        /// <param name="color"></param>
        /// <param name="biomarkersArg"></param>
        public void Init(string cellType, Color color, List<BiomarkerValuePair> biomarkersArg, bool meetsThresholdsArg)
        {
            (CellType, Color, biomarkers, meetsThresholds) = (cellType, color, biomarkersArg, meetsThresholdsArg);
        }
    }
}
