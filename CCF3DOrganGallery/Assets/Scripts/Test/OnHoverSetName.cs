using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class OnHoverSetName : MonoBehaviour
{
    public void DisplayName(BaseInteractionEventArgs args)
    {
        GetComponentInChildren<TMP_Text>().text = args.interactableObject.transform.gameObject.name;
    }
}
