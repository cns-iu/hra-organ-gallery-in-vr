using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace HRAOrganGallery
{
    public class LegendDisplay : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject _pre_LegendEntry;
        [SerializeField] private Transform _verticalLayout;
        private Transform _credits;


        [Header("Data")]
        [SerializeField] private CellLegend _legend;
        [SerializeField] private SODatasetCellTypeFrequency _frequency;
        [SerializeField] private CellTypeToColorMapping _mapping = new CellTypeToColorMapping();

        private void Start()
        {
            //get credits
            _credits = _verticalLayout.GetChild(0);

            //get frequency
            _frequency = GetComponent<Visualizer>().cellTypeFrequency;

            //get kegend
            _legend = GetComponent<CellLegend>();

            //get mapping
            _mapping = _legend.mapping;

            _mapping.pairs.ForEach(p =>
            {
                CreateKeyAndText(p.cellType, p.color);
            });

            MoveCreditsToBottom();
        }

        private void CreateKeyAndText(string cellType, Color color)
        {
            GameObject entry = Instantiate(_pre_LegendEntry, _verticalLayout);
            entry.GetComponentInChildren<TMP_Text>().text = cellType 
                + $" ({_frequency.pairs.Where(p => p.type == cellType).First().frequency.ToString("#,#")})"
                ;
            entry.GetComponentInChildren<SpriteRenderer>().color = color;
        }

        private void MoveCreditsToBottom()
        {
            _credits.transform.SetSiblingIndex(_verticalLayout.childCount - 1);
        }
    }
}
