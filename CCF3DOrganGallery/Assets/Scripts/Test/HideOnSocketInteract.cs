using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class HideOnSocketInteract : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor interactor;

    private void Awake()
    {

        interactor = GetComponent<XRSocketInteractor>();

        interactor.hoverEntered.AddListener(
            (HoverEnterEventArgs args) =>
            {
                gameObject.GetComponent<MeshRenderer>().enabled = false;
            });

        interactor.hoverExited.AddListener((HoverExitEventArgs args) =>
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        });

    }

}
