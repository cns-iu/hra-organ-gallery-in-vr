using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace HRAOrganGallery
{
    public class OnConditionIncreaseSeIf : MonoBehaviour
    {
        [SerializeField] private string _cellType;
        private Vector3 _defaultScale;

        private void OnEnable()
        {
            OnHoverSendMessage.OnLegendButtonHoverEnter += HandleIPointerEnterHandler;
            OnHoverSendMessage.OnLegendButtonHoverExit += HandleIPointerExitHandler;
        }

        private void OnDestroy()
        {
            OnHoverSendMessage.OnLegendButtonHoverEnter -= HandleIPointerEnterHandler;
            OnHoverSendMessage.OnLegendButtonHoverExit -= HandleIPointerExitHandler;
        }

        private void HandleIPointerEnterHandler(string hoveredCellType)
        {
            if (GetComponent<CellData>().CellType == hoveredCellType && GetComponent<CellData>().CellType != "Endothelial")
                Increase();
        }

        private void HandleIPointerExitHandler(string unHoveredCellType)
        {
            if (GetComponent<CellData>().CellType == unHoveredCellType && GetComponent<CellData>().CellType != "Endothelial")
                Decrease();
        }

        private void Start()
        {
            _defaultScale = transform.localScale;
            if (GetComponent<CellData>().CellType == "Endothelial")
                Increase();
        }

        public void Init(string type) => (_cellType) = (type);

        private void Increase() { transform.localScale = _defaultScale * 3f; }

        private void Decrease() { transform.localScale = _defaultScale; }
    }


}
