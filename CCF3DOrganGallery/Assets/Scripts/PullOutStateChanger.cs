using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PullOutStateChanger : MonoBehaviour
{
    // List of all Organs in the scene
    private List<GameObject> _organs;
    
    // List of all tissue-blocks in the scene
    private List<GameObject> _tissueBlocks;

    // Dict to store default position, rotation adn scale values of organs
    private readonly Dictionary<GameObject, List<Vector3>> _organDefaults = new Dictionary<GameObject, List<Vector3>>();
    
    // Checks whether Organ is in a state to interact with physics or not to perform appropriate actions
    [FormerlySerializedAs("_organState")] public bool organState = true;
    
    // Reference to button that changes the state ('Y' or 'B')
    public InputActionReference stateChangeButtonPressed;

    // Reference to button that instructs tissue-blocks / organs to float back to original position
    public InputActionReference floatBackInputActionReference;

    // WristPocket manager
    public WristPocketManager wristPocketManager;
    
    // Start is called before the first frame update
    void Start()
    {
        // Adding all organs and tissue-blocks in the scene to their respective list
        _organs = GameObject.FindGameObjectsWithTag("Organ").ToList();
        _tissueBlocks = GameObject.FindGameObjectsWithTag("TissueBlock").ToList();
        wristPocketManager = GameObject.Find("WristPocket").GetComponent<WristPocketManager>();
        // SceneBuilder.OnSceneBuilt += InitializeOrganDefaultValues;
        InitializeOrganDefaultValues(); // stores organ defaults on scene load
    }
    
    // Update is called once per frame
    private void Update()
    {
        // Check if reference button is pressed and call StateChange() function to perform certain actions
        if (stateChangeButtonPressed.action.triggered)
        {
            StateChange();
        }
    }

    // Switches between Organ and Tissue-Block PullOut/FloatBack state on button press
    void StateChange()
    {
        // If organ state is true / changed to true
        if (organState)
        {
            // Then turn off/remove organ scripts, and turn on 'IsTrigger' for their colliders
            OrganTriggerOff();
            // Then turn on/re-attach tissue-block scripts, and turn off 'IsTrigger' for their colliders
            TissueBlockTriggerOn();
            // Save all tissue-block's default values 
            InitializeTissueBlocksDefaultValues();
        }
        // If organ state is false / changed to false
        else
        {
            // Start WaitFloatBackTask Coroutine
            StartCoroutine(WaitFloatBackTask());
        } 
        // Switch organState bool
        organState = !organState;
    }

    //  Initializing default Position, Rotation, and Scale of all organs
    private void InitializeOrganDefaultValues()
    {
        foreach (var o in _organs)
        {
            var values = new List<Vector3>(){o.transform.position, o.transform.rotation.eulerAngles, o.transform.localScale};
            _organDefaults.Add(o, values); // For later
            // Calls UpdateOrganDefaultValues() function from FloatBackOrgan script attached to each organ
            o.GetComponent<FloatBackOrgan>().UpdateOrganDefaultValues(values);
        }
    }
    
    // Function for later features
    public List<Vector3> GetDefaultValuesForOrgan(GameObject organ)
    {
        return _organDefaults[organ];
    }

    // Initializing default Position, Rotation, and Scale of all tissue-blocks
    void InitializeTissueBlocksDefaultValues()
    {
        foreach (var tissueBlock in _tissueBlocks)
        {
            // Calls InitializeTissueBlockDefaultValues() function from FloatBackTissueBlocks script attached to each organ
            tissueBlock.GetComponent<FloatBackTissueBlocks>().InitializeTissueBlockDefaultValues();
        }
    }
    
    // Turns off physics for organs, removes scripts, rigidbodies to each and makes them un-interactable
    void OrganTriggerOff()
    {
        foreach (var item in _organs) // item being a singular organ
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
    
    // Turns off physics for tissue-blocks, removes scripts, rigidbodies to each and makes them un-interactable
    private void TissueBlockTriggerOff()
    {
        foreach (var item in _tissueBlocks) // item being a singular tissue-block or organ
        {
            var component = item.GetComponent<Collider>();
            var interactable = item.GetComponent<OffsetAttach>();
            var rb = item.GetComponent<Rigidbody>();
            var floatBack = item.GetComponent<FloatBackTissueBlocks>();

            Destroy(interactable);
            Destroy(rb);    
            Destroy(floatBack);
            component.isTrigger = true;
        }
    }
    
    // Turns on physics for organs, adds scripts, rigidbodies to each and makes the organ interactable
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
    
    // Turns on physics for tissue-blocks, adds scripts, rigidbodies to each and makes the organ interactable
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

    // Stalls till the last tissue-block floats back and then conducts state-change actions: TissueBlockTriggerOff() OrganTriggerOn();
    IEnumerator WaitFloatBackTask()
    {
        for (int i = 0; i < _tissueBlocks.Count; i++)
        {
            if (!wristPocketManager.wristPocket.Contains(_tissueBlocks[i]))
            {
                var floatBack = _tissueBlocks[i].GetComponent<FloatBackTissueBlocks>();

                if (i == _tissueBlocks.Count - 1)       
                {
                    yield return StartCoroutine(floatBack.FloatBack());
                }
                else
                {
                    StartCoroutine(floatBack.FloatBack());
                }
            }
        }
        
        TissueBlockTriggerOff();
        OrganTriggerOn();
    }
}