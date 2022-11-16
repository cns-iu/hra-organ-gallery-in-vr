using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class OnExtrudeActivateFloat : MonoBehaviour
{
    // Reference to boolean for checking if completely extruded or not
    private bool canPullOutOrgan = false;
    // Reference to SceneBuilder.cs script
    private SceneBuilder sceneBuilder;

    // Start is called before the first frame update
    void Start()
    {
        sceneBuilder = GameObject.Find("SceneBuilder").GetComponent<SceneBuilder>();
    }

    private void OnEnable()
    {
        HorizontalExtruder.ExtrusionUpdate += FloatBackOn;
        HorizontalExtruder.ExtrusionUpdate += (v) => { Debug.Log($"{v[0]},{v[1]}"); };
    }

    private void OnDestroy()
    {
        HorizontalExtruder.ExtrusionUpdate -= FloatBackOn;

    }

    public void FloatBackOn(float[] stepValues)
    {
        if (stepValues[1] >= .95f)
        {
            if (!canPullOutOrgan)
            {
                canPullOutOrgan = true;
                organTriggerOn();
                sceneBuilder.InitializeOrganDefaultValues(gameObject);
            }

        }
        else
        {
            canPullOutOrgan = false;
            organTriggerOff();
        }
    }

    void organTriggerOn()
    {
        // make organs interactable, but remain disabled.
        var rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        var interaction = gameObject.AddComponent<XRGrabInteractable>();
        var floatBack = gameObject.AddComponent<FloatBackOrgan>();
        floatBack.buttonPressed = GameObject.Find("SceneBuilder").GetComponent<SceneBuilder>().floatBackInputActionReference;
        var component = gameObject.GetComponent<BoxCollider>();
        // var offset = GameObject.Find("Offset");
        component.isTrigger = false;
        interaction.throwOnDetach = false;
        // interaction.attachTransform = offset.transform; // For when we attach an offset transform to the gameObject to grab without coinciding with controllers. 
        rb.useGravity = false;
    }

    void organTriggerOff()
    {
        var component = gameObject.GetComponent<BoxCollider>();
        var interactable = gameObject.GetComponent<XRGrabInteractable>();
        var rb = gameObject.GetComponent<Rigidbody>();
        var floatBack = gameObject.GetComponent<FloatBackOrgan>();

        Destroy(interactable);
        Destroy(rb);
        Destroy(floatBack);
        component.isTrigger = true;
    }
}
