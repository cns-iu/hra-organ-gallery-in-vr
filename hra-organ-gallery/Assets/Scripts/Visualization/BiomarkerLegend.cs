using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HRAOrganGallery
{
    public class BiomarkerLegend : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject _pre_LegendEntry;
        [SerializeField] private Transform _verticalLayout;
        private Transform _credits;


        [Header("Data")]
        [SerializeField] private CellTypeToColorMapping _mapping = new CellTypeToColorMapping();

        private void Start()
        {
            //get credits
            _credits = _verticalLayout.GetChild(0);

            //get mapping
            _mapping = VisualizerBiomarkers.Instance.mapping;

            _mapping.pairs.ForEach(p =>
            {
                CreateKeyAndText(p.cellType, p.color);
            });

            MoveCreditsToBottom();
        }

        private void CreateKeyAndText(string cellType, Color color)
        {
            GameObject entry = Instantiate(_pre_LegendEntry, _verticalLayout);
            entry.GetComponentInChildren<TMP_Text>().text = cellType;
            entry.GetComponentInChildren<SpriteRenderer>().color = color;
        }

        private void MoveCreditsToBottom()
        {
            _credits.transform.SetSiblingIndex(_verticalLayout.childCount - 1);
        }
    }
}
