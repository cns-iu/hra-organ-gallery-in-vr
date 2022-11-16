using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColliderTest : MonoBehaviour
{

    [SerializeField] private SceneBuilder sceneBuilder;

    private void OnEnable()
    {
        SceneBuilder.OnOrgansLoaded += () =>
        {
            foreach (var o in sceneBuilder.Organs)
            {
                AddColliderAroundChildren(o);
            }
        };
    }

    private void AddColliderAroundChildren(GameObject wrapper)
    {
        var pos = wrapper.transform.localPosition;
        var rot = wrapper.transform.localRotation;
        var scale = wrapper.transform.localScale;

        // need to clear out transforms while encapsulating bounds
        wrapper.transform.localPosition = Vector3.zero;
        wrapper.transform.localRotation = Quaternion.identity;
        wrapper.transform.localScale = Vector3.one;

        // start with root object's bounds
        var bounds = new Bounds(Vector3.zero, Vector3.zero);
        if (wrapper.transform.TryGetComponent<Renderer>(out var mainRenderer))
        {
            // as mentioned here https://forum.unity.com/threads/what-are-bounds.480975/
            // new Bounds() will include 0,0,0 which you may not want to Encapsulate
            // because the vertices of the mesh may be way off the model's origin
            // so instead start with the first renderer bounds and Encapsulate from there
            bounds = mainRenderer.bounds;
        }

        var descendants = wrapper.GetComponentsInChildren<Transform>();
        foreach (Transform desc in descendants)
        {
            if (desc.gameObject.tag == "Untagged" && desc.gameObject.name != "Model")
            {
                if (desc.TryGetComponent<Renderer>(out var childRenderer))
                {
                    // use this trick to see if initialized to renderer bounds yet
                    // https://answers.unity.com/questions/724635/how-does-boundsencapsulate-work.html
                    if (bounds.size == Vector3.zero)
                        bounds = childRenderer.bounds;
                    bounds.Encapsulate(childRenderer.bounds);
                }
            }

        }

        var boxCol = wrapper.transform.GetChild(0).AddComponent<BoxCollider>();
        boxCol.center = bounds.center - wrapper.transform.position;
        boxCol.size = bounds.size;

        // restore transforms
        wrapper.transform.localPosition = pos;
        wrapper.transform.localRotation = rot;
        wrapper.transform.localScale = scale;
    }
}
