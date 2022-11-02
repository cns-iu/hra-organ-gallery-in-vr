using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LookAtUser : MonoBehaviour
{
    private BaseTeleportationInteractable[] teleportInteractables;
    private float cameraYOffset;

    private void Start()
    {
        teleportInteractables = FindObjectsOfType<BaseTeleportationInteractable>();

        for (int i = 0; i < teleportInteractables.Length; i++)
        {
            teleportInteractables[i].teleporting.AddListener(
              (args) => { cameraYOffset = SetOffset(); });
        }

        cameraYOffset = SetOffset();

    }

    private void Update()
    {
        transform.LookAt(Camera.main.GetComponentInParent<XROrigin>().transform.position + new Vector3(0f, cameraYOffset, 0f));
    }

    private float SetOffset()
    {
        return Camera.main.transform.position.y;
    }

}


