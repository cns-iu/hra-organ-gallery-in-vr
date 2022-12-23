using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class OnExtrudeActivateFloat : MonoBehaviour
{
    // Reference to boolean for checking if completely extruded or not
    public bool canPullOutOrgan = false;
    // Reference to SceneBuilder.cs script
    private SceneBuilder sceneBuilder;
    // List of all Organs in the scene
    private List<GameObject> _organs;
    // Dict to store default position, rotation adn scale values of organs
    private readonly Dictionary<GameObject, List<Vector3>> _organDefaults = new Dictionary<GameObject, List<Vector3>>();
    
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
                InitializeOrganDefaultValues(gameObject);
            }
        }
        else
        {
            canPullOutOrgan = false;
            organTriggerOff();
        }
    }

    public void organTriggerOn()
    {
        // make organs interactable, but remain disabled.
        var rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        var interaction = gameObject.AddComponent<XRGrabInteractable>();
        var floatBack = gameObject.AddComponent<FloatBackOrgan>();
        floatBack.buttonPressed = GameObject.Find("Reference").GetComponent<ButtonReference>().floatBackInputActionReference;
        var component = gameObject.GetComponent<BoxCollider>();
        // var offset = GameObject.Find("Offset");
        component.isTrigger = false;
        interaction.throwOnDetach = false;
        // interaction.useDynamicAttach = true;
        // interaction.attachTransform = offset.transform; // For when we attach an offset transform to the gameObject to grab without coinciding with controllers. 
        rb.useGravity = false;
    }

    public void organTriggerOff()
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

    private void InitializeOrganDefaultValues(GameObject o) // o = organ
    {
        var values = new List<Vector3>(){o.transform.position, o.transform.rotation.eulerAngles, o.transform.localScale};
        if(!_organDefaults.ContainsKey(o))
        {
            _organDefaults.Add(o, values); 
        }
        // Calls UpdateOrganDefaultValues() function from FloatBackOrgan script attached to each organ
        o.GetComponent<FloatBackOrgan>().UpdateOrganDefaultValues(values);
    }
    
    public List<Vector3> GetDefaultValuesForOrgan(GameObject organ) 
    {
        return _organDefaults[organ];
    }
}
