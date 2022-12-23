using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PullOutStateChanger : MonoBehaviour
{
    // List of all Organs in the scene
    [SerializeField] private List<GameObject> _organs;
    
    // List of all tissue-blocks in the scene
    [SerializeField] private List<GameObject> _tissueBlocks;

    // Reference to SceneBuilder.cs script
    private SceneBuilder sceneBuilder;
    
    // Dict to store default position, rotation and scale values of organs
    // private readonly Dictionary<GameObject, List<Vector3>> _organDefaults = new Dictionary<GameObject, List<Vector3>>();
    
    // Checks whether Organ is in a state to interact with physics or not to perform appropriate actions
    public bool organState = true;
    
    // Reference to button that changes the state ('Y' or 'B')
    public InputActionReference stateChangeButtonPressed;

    // Start is called before the first frame update
    void Start()
    { 
        sceneBuilder = GameObject.Find("SceneBuilder").GetComponent<SceneBuilder>();
        
        
        // Adding all organs and tissue-blocks in the scene to their respective list
        // _organs = GameObject.FindGameObjectsWithTag("Organ").ToList();
        // _tissueBlocks = GameObject.FindGameObjectsWithTag("TissueBlock").ToList();
        stateChangeButtonPressed = GameObject.Find("Reference").GetComponent<ButtonReference>().stateChangeReference;
        // SceneBuilder.OnSceneBuilt += InitializeOrganDefaultValues;
        // InitializeOrganDefaultValues(); // stores organ defaults on scene load
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if reference button is pressed and call StateChange() function to perform certain actions
        if (stateChangeButtonPressed.action.triggered)
        {
            Debug.Log("Changing state");
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
            organTriggerOff();
            
            // Then turn on/re-attach tissue-block scripts, and turn off 'IsTrigger' for their colliders
            TissueBlockTriggerOn();
            
            // Need intermediary function responsible for floating back tissue blocks to default location before switch state ~ YK,12/22;8:41a
            
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

    void organTriggerOff()
    {
        // Debug.Log("organ trigger off from pullout state changer");
        foreach(var organ in sceneBuilder.Organs) // WORK ON THE FIRST CHILD OF THIS LIST - YASH KUMAR
        {
            try
            {
                var organModel = organ.transform.GetChild(0).GetComponent<OnExtrudeActivateFloat>();
                organModel.organTriggerOff();
                organModel.canPullOutOrgan = false;
            }
            catch (Exception e){ }
        }    
    }
    
    void organTriggerOn()   
    {
        foreach(var organ in sceneBuilder.Organs)
        {
            try
            {
                var organModel = organ.transform.GetChild(0).GetComponent<OnExtrudeActivateFloat>(); // Because sceneBuilder.Organs is a list of organ wrappers according to Andi
                organModel.organTriggerOn();
                organModel.canPullOutOrgan = true;
            }
            catch(Exception e) {}
        }    
    }
    
    //  Initializing default Position, Rotation, and Scale of all organs
    // private void InitializeOrganDefaultValues()
    // {
    //     foreach (var o in _organs)
    //     {
    //         var values = new List<Vector3>(){o.transform.position, o.transform.rotation.eulerAngles, o.transform.localScale};
    //         _organDefaults.Add(o, values); // For later
    //         // Calls UpdateOrganDefaultValues() function from FloatBackOrgan script attached to each organ
    //         o.GetComponent<FloatBackOrgan>().UpdateOrganDefaultValues(values);
    //     }
    // }
    //
    // // Function for later features
    // public List<Vector3> GetDefaultValuesForOrgan(GameObject organ)
    // {
    //     return _organDefaults[organ];
    // }

    // Initializing default Position, Rotation, and Scale of all tissue-blocks
    
    void InitializeTissueBlocksDefaultValues()
    {
        var referenceList = GameObject.Find("Reference").GetComponent<ButtonReference>();

        foreach (var tissueBlock in referenceList.TissueBlocks)
        {
            // Calls InitializeTissueBlockDefaultValues() function from FloatBackTissueBlocks script attached to each organ
            tissueBlock.GetComponent<FloatBackTissueBlocks>().InitializeTissueBlockDefaultValues(tissueBlock);
        }
    }

    // Turns off physics for organs, removes scripts, rigidbodies to each and makes them un-interactable
    // void OrganTriggerOff()
    // {
    //     foreach (var item in _organs) // item being a singular organ
    //     {
    //         var component = item.GetComponent<Collider>();
    //         var interactable = item.GetComponent<OffsetAttach>();
    //         var rb = item.GetComponent<Rigidbody>();
    //         var floatBack = item.GetComponent<FloatBackOrgan>();
    //         
    //         Destroy(interactable);
    //         Destroy(rb);
    //         Destroy(floatBack);
    //         component.isTrigger = true;
    //     }
    // }
    
    // Turns off physics for tissue-blocks, removes scripts, rigidbodies to each and makes them un-interactable

    // Turns on physics for organs, adds scripts, rigidbodies to each and makes the organ interactable
    // void OrganTriggerOn()
    // {
    //     foreach (var item in _organs) // item being a singular organ
    //     {
    //         var rb = item.AddComponent<Rigidbody>();
    //         var interactable = item.AddComponent<OffsetAttach>();
    //         var floatBack = item.AddComponent<FloatBackOrgan>();
    //         
    //         var component = item.GetComponent<Collider>();
    //         var offset = GameObject.Find("Offset");
    //
    //         component.isTrigger = false;
    //         interactable.throwOnDetach = false;
    //         interactable.attachTransform = offset.transform;
    //         rb.useGravity = false;
    //         floatBack.buttonPressed = GameObject.Find("SceneBuilder").GetComponent<SceneBuilder>().floatBackInputActionReference;
    //     }
    // }
    
    // Turns on physics for tissue-blocks, adds scripts, rigidbodies to each and makes the organ interactable
    
    private void TissueBlockTriggerOff()
    {
        var referenceList = GameObject.Find("Reference").GetComponent<ButtonReference>();
        
        foreach (var item in referenceList.TissueBlocks) // item being a singular tissue-block or organ
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
    
    void TissueBlockTriggerOn()
    {
        var referenceList = GameObject.Find("Reference").GetComponent<ButtonReference>();
        
        foreach (var item in referenceList.TissueBlocks) // item being a singular tissue-block
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
            floatBack.buttonPressed = referenceList.floatBackInputActionReference;
        }
    }

    // Stalls till the last tissue-block floats back and then conducts state-change actions: TissueBlockTriggerOff() OrganTriggerOn();
    IEnumerator WaitFloatBackTask()
    {
        for (int i = 0; i < _tissueBlocks.Count; i++)
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
        TissueBlockTriggerOff();
        organTriggerOn();
    }
}