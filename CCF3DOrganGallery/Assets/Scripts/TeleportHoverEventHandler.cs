using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class TeleportHoverEventHandler : MonoBehaviour
{
    public void OnHoverEntered(HoverEnterEventArgs args)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    public void OnHoverExited(HoverExitEventArgs args)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
