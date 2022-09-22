using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SetTeleportationAnchors : MonoBehaviour
{
    public GameObject pre_teleportationAnchor;

    private void Awake()
    {
        GameObject anchor = Instantiate(pre_teleportationAnchor);
        anchor.transform.parent = transform;
    }

}
