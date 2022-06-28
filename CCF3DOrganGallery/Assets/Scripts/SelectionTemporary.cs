using UnityEngine;
using UnityEngine.InputSystem;


/* This code is a Temporary replacement till Andi works with me on the events functionality ~Yash Kumar*/ 
public class SelectionTemporary: MonoBehaviour
{   
    // References Input Action created "XRI RightHand/SelectTissue"
    public InputActionReference leftTriggerPressed; 
    
    //References selectedTissueBlock to accept the gameObject whose Collider is met with Raycast Ray.
    public GameObject selectedTissueBlock;
   
    // Update is called once per frame 
    void Update()
    {
        // Records structure used to get information back from a raycast.
        RaycastHit hit;
        // Casting new ray into the scene
        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
        
        /* Testing out rightTriggerPressed.action.* methods */ 
        // Debug.Log("Enabled: "+rightTriggerPressed.action.enabled);
        // Debug.Log("ID: "+rightTriggerPressed.action.id);
        // Debug.Log("Triggered: "+rightTriggerPressed.action.triggered);
        // Debug.Log("In Progress: "+rightTriggerPressed.action.inProgress);
        
        // If our ray hits a collider somewhere in the scene, do:
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // Compare if the object's tag was "TissueBlock"
            // Debug.Log(hit.collider.gameObject.name); (code to read which object's collider was hit)
            if (hit.collider.CompareTag("TissueBlock"))
            {
                // Store the reference of gameObject into selectedTissue gameObject reference
                selectedTissueBlock = hit.collider.gameObject;
                // Fetch script TissueBlocksColorChanger and use method responsible for setting Hover color
                selectedTissueBlock.GetComponent<TissueBlockColorChanger>().SetHoverColor(hit);
                // In case the rightTriggerPressed reference returns true (is in progress)
                if (leftTriggerPressed.action.inProgress)
                {
                    // Fetch script TissueBlocksColorChanger and use method responsible for setting Selection color
                    selectedTissueBlock.GetComponent<TissueBlockColorChanger>().SetSelectColor(hit);
                }
            }
        }
        else
        {
            // Invoke method responsible for setting colour back to default, if raycast hits objects other than those with "TissueBlock" tag
            setToDefault(hit);
        }
    }

    // Method to set to default color
    void setToDefault(RaycastHit hit) //passing variable to prevent errors even though setToDefault does not require RaycastHit structure
    {
        // x is an array of GameObjects that stores all the objects in the scene with tag TissueBlock  
        var x = GameObject.FindGameObjectsWithTag("TissueBlock");
        // for loop that goes over each tissue-block in the array of tissue-blocks, x
        foreach (var tissueblock in x)
        {
            // sets every tissue-block to default colour
            tissueblock.GetComponent<TissueBlockColorChanger>().SetDefaultColor(hit);
        }
    }
}
