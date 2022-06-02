using UnityEngine;

public class TissueBlocksColorChanger : MonoBehaviour
{
    private Renderer _tissueBlocksRenderer;
    private static readonly int Color1 = Shader.PropertyToID("_Color");

    // Start is called before the first frame update
    void Start()
    {
        _tissueBlocksRenderer = GetComponent<Renderer>();
    }

    // void OnHover()
    // {
    //     Demo_Selector.OnSelected += SetHoverColor;
    // }
    //
    // void OnDeselection() //Hover inclusive
    // {
    //     Demo_Selector.OnSelected -= SetDefaultColor;
    // }

    public void SetHoverColor()
    {
        _tissueBlocksRenderer.material.SetColor(Color1, Color.yellow);
    }

    public void SetDefaultColor()
    {
        _tissueBlocksRenderer.material.SetColor(Color1, Color.white);
    }

    public void SetSelectColor()
    {
        _tissueBlocksRenderer.material.SetColor(Color1, Color.blue);
    }
}
