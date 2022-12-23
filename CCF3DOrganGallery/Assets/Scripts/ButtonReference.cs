using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonReference : MonoBehaviour
{
    // Reference to button that instructs tissue-blocks / organs to float back to original position
    public InputActionReference floatBackInputActionReference;
    
    // Reference to button that instructs tissue-blocks / organs to switch between interactable states 
    public InputActionReference stateChangeReference;
    
    public List<GameObject> TissueBlocks;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
