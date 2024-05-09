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
            if (hoveredCellType.Contains(GetComponent<CellData>().CellType) && !GetComponent<CellData>().CellType.Contains("Endothelial"))
                Increase();
        }

        private void HandleIPointerExitHandler(string unHoveredCellType)
        {
            if (unHoveredCellType.Contains(GetComponent<CellData>().CellType) && !GetComponent<CellData>().CellType.Contains("Endothelial"))
                Decrease();
        }

        private void Start()
        {
            _defaultScale = transform.localScale;
            if (GetComponent<CellData>().CellType.Contains( "Endothelial"))
                Increase();
        }

        public void Init(string type) => (_cellType) = (type);

        private void Increase() { transform.localScale = _defaultScale * 3f; }

        private void Decrease() { transform.localScale = _defaultScale; }
    }


}
