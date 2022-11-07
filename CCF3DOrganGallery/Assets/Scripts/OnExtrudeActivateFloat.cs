using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnEnable()
    {
        if(gameObject.CompareTag("Organ")){
            HorizontalExtruder.ExtrusionUpdate += FloatBackOn;
        }
    }

    private void OnDestroy()
    {
        if(gameObject.CompareTag("Organ")){
            HorizontalExtruder.ExtrusionUpdate -= FloatBackOn;
        }
    }

    public void FloatBackOn(float[] stepValues)
    {
        if(stepValues[1] == 1f)
        {
            UnityEngine.Debug.Log("Completely Extruded");
            if(!canPullOutOrgan)
            {
                canPullOutOrgan = true;
                UnityEngine.Debug.Log("a");
                organTriggerOn();
                sceneBuilder.InitializeOrganDefaultValues(gameObject);
                UnityEngine.Debug.Log("A");
            }
            else{
                UnityEngine.Debug.Log("c");
            }
        }
        else
        {
            canPullOutOrgan = false;
            organTriggerOff();
            UnityEngine.Debug.Log("b");
        }
    }

    void organTriggerOn()
    {
        UnityEngine.Debug.Log("organTriggerOn has been called");
        // make organs interactable, but remain disabled.
        var rb = gameObject.AddComponent<Rigidbody>();
        var interaction = gameObject.AddComponent<OffsetAttach>();
        var floatBack = gameObject.AddComponent<FloatBackOrgan>();
        UnityEngine.Debug.Log("floatbacc");
        floatBack.buttonPressed = GameObject.Find("SceneBuilder").GetComponent<SceneBuilder>().floatBackInputActionReference;
        var component = gameObject.GetComponent<Collider>();
        // var offset = GameObject.Find("Offset");
        component.isTrigger = false;
        interaction.throwOnDetach = false;
        // interaction.attachTransform = offset.transform; // For when we attach an offset transform to the gameObject to grab without coinciding with controllers. 
        rb.useGravity = false;
    }

    void organTriggerOff()
    {
        var component = gameObject.GetComponent<Collider>();
        var interactable = gameObject.GetComponent<OffsetAttach>();
        var rb = gameObject.GetComponent<Rigidbody>();
        var floatBack = gameObject.GetComponent<FloatBackOrgan>();
        
        Destroy(interactable);
        Destroy(rb);
        Destroy(floatBack);
        component.isTrigger = true;
    }
}
