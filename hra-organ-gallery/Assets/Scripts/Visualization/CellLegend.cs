using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public class CellLegend : MonoBehaviour
    {
        [SerializeField] public CellTypeToColorMapping mapping;
        [SerializeField] private Visualizer _visualizer; //Visualizer on same GameObject
        [SerializeField]
        private SODatasetCellTypeFrequency _cellTypeFrequency;
        [SerializeField] private SOColorValues _colorValues;

        private void Awake()
        {
            _visualizer = GetComponent<Visualizer>();

            CaptureColorMappings();
        }

        private void CaptureColorMappings()
        {
            mapping = new CellTypeToColorMapping();
            int colorIterator = 0;

            _cellTypeFrequency.pairs.ForEach(
                p =>
            {
                mapping.pairs.Add(new CellTypeColorPair(p.type, _colorValues.values[colorIterator].color));

                colorIterator++;
            });

        }

    }
}
