using UnityEngine;
using UnityEngine.Serialization;

public class TissueBlockColorChanger : MonoBehaviour
{
    
    // References the Renderer of current tissue block in question
    [FormerlySerializedAs("_tissueBlocksRenderer")] public Renderer tissueBlocksRenderer;
    // Rider optimization code to replace "_color" in *.SetColor() method
    private static readonly int Color1 = Shader.PropertyToID("_Color");

    // When the required Input action is in progress
    private void OnEnable()
    {
        // Subscribes events to 
        TissueBlockSelectActions.OnSelected += SetSelectColor;
        TissueBlockSelectActions.OnHover += SetHoverColor;
        TissueBlockSelectActions.SetToDefault += SetDefaultColor;
    }

    // When required Input action is no longer taking place
    private void OnDestroy()
    {
        // Unsubscribes
        TissueBlockSelectActions.OnSelected -= SetSelectColor;
        TissueBlockSelectActions.OnHover -= SetHoverColor;
        TissueBlockSelectActions.SetToDefault -= SetDefaultColor;
    }

    // Start is called before the first frame update
    void Start()
    {
        tissueBlocksRenderer = GetComponent<Renderer>();
    }

    public void SetDefaultColor(RaycastHit hit) //
    {
        tissueBlocksRenderer.material.SetColor(Color1, Color.white);
    }

    public void SetHoverColor(RaycastHit hit)
    {
        if (hit.collider.name.Equals(this.gameObject.name))
        {
            tissueBlocksRenderer.material.SetColor(Color1, Color.yellow);   
        }
    }

    public void SetSelectColor(RaycastHit hit)
    {
        if (hit.collider.name.Equals(this.gameObject.name))
        {
            tissueBlocksRenderer.material.SetColor(Color1, Color.blue);
        }
    }
}
