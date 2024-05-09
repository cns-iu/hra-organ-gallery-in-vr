using System.Collections;
using System.Collections.Generic;
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
        private TMP_Text _TmpText;
        private Image _legendKeySpriteRenderer;


        [Header("Data")]
        [SerializeField] private CellLegend _legend;
        [SerializeField] private CellTypeToColorMapping _mapping;

        private void Start()
        {
            //get credits
            _credits = _verticalLayout.GetChild(0);

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
            entry.GetComponentInChildren<TMP_Text>().text = cellType;
            entry.GetComponentInChildren<SpriteRenderer>().color = color;
        }

        private void MoveCreditsToBottom()
        {
            _credits.transform.SetSiblingIndex(_verticalLayout.childCount - 1);
        }
    }
}
