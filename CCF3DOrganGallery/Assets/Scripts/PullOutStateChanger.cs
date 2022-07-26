using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PullOutStateChanger : MonoBehaviour
{
    // New additions
    private List<GameObject> _organs;
    private List<GameObject> _tissueBlocks;

    // Dict to store above values
    private readonly Dictionary<GameObject, List<Vector3>> _organDefaults = new Dictionary<GameObject, List<Vector3>>();
    
    [FormerlySerializedAs("_organState")] public bool organState = true;
    
    public InputActionReference buttonPressed;

    public InputActionReference floatBackInputActionReference;

    // Start is called before the first frame update
    void Start()
    {
        _organs = GameObject.FindGameObjectsWithTag("Organ").ToList();
        _tissueBlocks = GameObject.FindGameObjectsWithTag("TissueBlock").ToList();
        
        // SceneBuilder.OnSceneBuilt += InitializeOrganDefaultValues;
        InitializeOrganDefaultValues();
    }

    
    // Update is called once per frame
    private void Update()
    {
        if (buttonPressed.action.triggered)
        {
            StateChange();
        }
    }

    void StateChange()
    {
        if (organState)
        {
            OrganTriggerOff();
            TissueBlockTriggerOn();
            InitializeTissueBlocksDefaultLocation();
        }
        else
        {
            // StartCoroutine(WaitDestroyTask());
            TissueBlockTriggerOff();
            OrganTriggerOn();
        } 
        organState = !organState;
    }

    //  Initializing default Position, Rotation, and Scale of all organs
    private void InitializeOrganDefaultValues()
    {
        foreach (var o in _organs)
        {
            var values = new List<Vector3>(){o.transform.position, o.transform.rotation.eulerAngles, o.transform.localScale};
            _organDefaults.Add(o, values); // For later
            Debug.Log(o.name);
            o.GetComponent<FloatBackOrgan>().UpdateOrganDefaultValues(values);
        }
    }
    
    public List<Vector3> GetDefaultValuesForOrgan(GameObject organ)
    {
        return _organDefaults[organ];
    }


    // void SwitchTriggerOff(List<GameObject> grabObjects)
    // {
    //     foreach (var item in grabObjects) // item being a singular tissue-block or organ
    //     {
    //         var component = item.GetComponent<Collider>();
    //         var interactable = item.GetComponent<OffsetAttach>();
    //         var rb = item.GetComponent<Rigidbody>();
    //         
    //         Destroy(interactable);
    //         Destroy(rb);
    //         component.isTrigger = !component.isTrigger;
    //     }
    // }
    //
    // void SwitchTriggerOn(List<GameObject> grabObjects)
    // {
    //     foreach (var item in grabObjects) // item being a singular organ or tissue-block
    //     {
    //         item.AddComponent<Rigidbody>();
    //         item.AddComponent<OffsetAttach>();
    //         
    //         var component = item.GetComponent<Collider>();
    //         var rb = item.GetComponent<Rigidbody>();
    //         var interactable = item.GetComponent<OffsetAttach>();
    //         var offset = GameObject.Find("Offset");
    //         
    //         component.isTrigger = false;
    //         interactable.throwOnDetach = false;
    //         interactable.attachTransform = offset.transform;
    //         rb.useGravity = false;
    //     }
    // }

    void InitializeTissueBlocksDefaultLocation()
    {
        foreach (var tissueBlock in _tissueBlocks)
        {
            tissueBlock.GetComponent<FloatBackTissueBlocks>().InitializeTissueBlockDefaultValues();
        }
    }
    
    void OrganTriggerOff()
    {
        foreach (var item in _organs) // item being a singular tissue-block or organ
        {
            var component = item.GetComponent<Collider>();
            var interactable = item.GetComponent<OffsetAttach>();
            var rb = item.GetComponent<Rigidbody>();
            var floatBack = item.GetComponent<FloatBackOrgan>();
            
            Destroy(interactable);
            Destroy(rb);
            Destroy(floatBack);
            component.isTrigger = true;
        }
    }
    
    private void TissueBlockTriggerOff()
    {
        foreach (var item in _tissueBlocks) // item being a singular tissue-block or organ
        {
            var component = item.GetComponent<Collider>();
            var interactable = item.GetComponent<OffsetAttach>();
            var rb = item.GetComponent<Rigidbody>();
            var floatBack = item.GetComponent<FloatBackTissueBlocks>();

            // floatBack.FloatBack();
            // handle with IEnumerators later

            Destroy(interactable);
            Destroy(rb);    
            Destroy(floatBack);
            component.isTrigger = true;
            // StartCoroutine(WaitDestroyTask(interactable, rb, floatBack, component));
        }
    }
    
    void OrganTriggerOn()
    {
        foreach (var item in _organs) // item being a singular organ
        {
            var rb = item.AddComponent<Rigidbody>();
            var interactable = item.AddComponent<OffsetAttach>();
            var floatBack = item.AddComponent<FloatBackOrgan>();
            
            var component = item.GetComponent<Collider>();
            var offset = GameObject.Find("Offset");

            component.isTrigger = false;
            interactable.throwOnDetach = false;
            interactable.attachTransform = offset.transform;
            rb.useGravity = false;
            floatBack.buttonPressed = floatBackInputActionReference;
        }
    }
    
    void TissueBlockTriggerOn()
    {
        foreach (var item in _tissueBlocks) // item being a singular tissue-block
        {
            var rb = item.AddComponent<Rigidbody>();
            var interactable = item.AddComponent<OffsetAttach>();
            var floatBack = item.AddComponent<FloatBackTissueBlocks>();
            
            var component = item.GetComponent<Collider>();
            // var offset = GameObject.Find("Offset"); // To add 
            
            component.isTrigger = false;
            interactable.throwOnDetach = false;
            // interactable.attachTransform = offset.transform;
            rb.useGravity = false;
            floatBack.buttonPressed = floatBackInputActionReference;
        }
    }

    // IEnumerator WaitDestroyTask(OffsetAttach interactable, Rigidbody rb, FloatBackTissueBlocks floatBack, Collider component)
    // {
    //     yield return new WaitForSecondsRealtime(2);
    //     DestroyForTissueBlocks(interactable, rb, floatBack, component);
    // }
    //
    // void DestroyForTissueBlocks(OffsetAttach interactable, Rigidbody rb, FloatBackTissueBlocks floatBack, Collider component)
    // {
    //     Destroy(interactable);
    //     Destroy(rb);    
    //     Destroy(floatBack);
    //     component.isTrigger = true;
    // }
}