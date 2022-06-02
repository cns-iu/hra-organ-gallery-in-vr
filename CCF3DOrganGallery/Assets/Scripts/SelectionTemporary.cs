
using UnityEngine;
using UnityEngine.InputSystem;



public class SelectionTemporary: MonoBehaviour
{   
    public InputActionReference rightTriggerPressed; 
    
    public GameObject selectedTissueBlock;
    [SerializeField] private Material material;
   
    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Right trigger value " + rightHand);
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
        
        // Debug.Log("Enabled: "+rightTriggerPressed.action.enabled);
        // Debug.Log("ID: "+rightTriggerPressed.action.id);
        // Debug.Log("Triggered: "+rightTriggerPressed.action.triggered);
        // Debug.Log("In Progress: "+rightTriggerPressed.action.inProgress);
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.CompareTag("TissueBlock"))
            {
                selectedTissueBlock = hit.collider.gameObject;
                selectedTissueBlock.GetComponent<TissueBlocksColorChanger>().SetHoverColor();
                if (rightTriggerPressed.action.inProgress)
                {
                    Debug.Log("In Progress: "+rightTriggerPressed.action.inProgress);
                    selectedTissueBlock.GetComponent<TissueBlocksColorChanger>().SetSelectColor();
                }
            }
        }
        else
        {
            setToDefault();
        }
    }

    // Code to set to default color, rendered redundant by use of events
    void setToDefault()
    {
        var x = GameObject.FindGameObjectsWithTag("TissueBlock");
        foreach (var tissues in x)
        {
            tissues.GetComponent<TissueBlocksColorChanger>().SetDefaultColor();
        }
    }
}
