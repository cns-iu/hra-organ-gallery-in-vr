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
            if (hoveredCellType.Contains(GetComponent<CellData>().CellType))
                Increase();
        }

        private void HandleIPointerExitHandler(string unHoveredCellType)
        {
            if (unHoveredCellType.Contains(GetComponent<CellData>().CellType))
                Decrease();
        }

        private void Start()
        {
            _defaultScale = transform.localScale;
            if (GetComponent<CellData>().CellType.Contains("Endothelial")) _defaultScale = transform.localScale * 2f; transform.localScale = _defaultScale;
        }

        public void Init(string type) => (_cellType) = (type);

        private void Increase()
        {
            if (!GetComponent<CellData>().CellType.Contains("Endothelial")) transform.localScale = _defaultScale * 3f;
            else transform.localScale = _defaultScale * 1.5f;
        }

        private void Decrease() { transform.localScale = _defaultScale; }
    }


}
