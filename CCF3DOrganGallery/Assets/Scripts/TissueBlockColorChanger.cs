using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Need to implement so that only one cube changes colour at one time
public class TissueBlockColorChanger : MonoBehaviour
{
    // References the Renderer of current tissue block in question
    private Renderer _tissueBlocksRenderer;
    // Rider optimization code to replace "_color" in *.SetColor() method
    private static readonly int Color1 = Shader.PropertyToID("_Color");

    // When the required Input action is in progress
    private void OnEnable()
    {
        // Subscribes events to respective colour changing method
        TissueBlockSelectActions.OnSelected += SetSelectColor;
        TissueBlockSelectActions.OnHover += SetHoverColor;
        TissueBlockSelectActions.SetToDefault += SetDefaultColor;
    }

    // When required Input action is no longer taking place
    private void OnDestroy()
    {
        // Unsubscribes events to respective colour changing method
        TissueBlockSelectActions.OnSelected -= SetSelectColor;
        TissueBlockSelectActions.OnHover -= SetHoverColor;
        TissueBlockSelectActions.SetToDefault -= SetDefaultColor;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Acquires renderer from tissueBlock(s)
        _tissueBlocksRenderer = GetComponent<Renderer>();
    }
    

    public void SetDefaultColor(RaycastHit hit) 
    {
        // Sets colour of tissue-block to white for default state when tissue-block is not given focus
        _tissueBlocksRenderer.material.SetColor(Color1, Color.white);
        // Turning off the Outline Script  
        gameObject.GetComponent<Outline>().enabled = false;
    }

    public void SetHoverColor(RaycastHit hit)
    {
        if (hit.collider.gameObject.Equals(this.gameObject))
        {
            // Sets colour of tissue-block to yellow when it is hovered upon. Only works when RaycastHit object collider matches the same tissue-block.
            _tissueBlocksRenderer.material.SetColor(Color1, Color.yellow);   
            // Turning on the Outline Script 
            gameObject.GetComponent<Outline>().enabled = true;
        }
    }

    public void SetSelectColor(RaycastHit hit)
    {
        if (hit.collider.gameObject.Equals(this.gameObject))
        {
            // Sets colour of tissue-block to blue when it is hovered upon and selected. Only works when RaycastHit object collider matches the same tissue-block.
            _tissueBlocksRenderer.material.SetColor(Color1, Color.blue);
            // Turning on the Outline Script
            gameObject.GetComponent<Outline>().enabled = true;
        }
    }
}
