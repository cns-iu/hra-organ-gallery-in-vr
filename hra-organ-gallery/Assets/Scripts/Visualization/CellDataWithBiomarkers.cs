using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public class CellDataWithBiomarkers : CellData
    {
        // [field: SerializeField]
        // public string CellType { get; set; }

        // [field: SerializeField]
        // public Color Color { get; set; }

        // private string _type;
        // private Color _color;

        public List<BiomarkerValuePair> biomarkers = new List<BiomarkerValuePair>();

        /// <summary>
        /// A constructor-like function to initialize an object of this class
        /// </summary>
        /// <param name="cellType"></param>
        /// <param name="color"></param>
        /// <param name="biomarkersArg"></param>
        public void Init(string cellType, Color color, List<BiomarkerValuePair> biomarkersArg) {
            (CellType, Color, biomarkers) = (cellType, color, biomarkersArg);
        }
    }
}
