using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

namespace HRAOrganGallery
{
    public class OnHoverSendMessage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public static event Action<string> OnLegendButtonHoverEnter;
        public static event Action<string> OnLegendButtonHoverExit;

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnLegendButtonHoverEnter?.Invoke(GetComponentInChildren<TMP_Text>().text);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnLegendButtonHoverExit?.Invoke(GetComponentInChildren<TMP_Text>().text);
        }
    }
}
