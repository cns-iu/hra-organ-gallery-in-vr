using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TeleportationPointLabeler : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;
    private List<string> _organs = new List<string>() {
        "Large intestine",
        "Blood vasculature",
        "Eye",
        "Heart",
        "Left kidney",
        "Right kidney",
        "Knee",
        "Liver",
        "Lung",
        "Ovary",
        "Pancreas",
        "Pelvis",
        "Small intestine",
        "Skin",
        "Spleen",
        "Uterus",
     };

    void Start()
    {
        _label.text = this.gameObject.name;
    }

}
