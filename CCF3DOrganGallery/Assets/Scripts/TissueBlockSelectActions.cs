using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class TissueBlockSelectActions : MonoBehaviour
{
    // Standard delegate declaration to further create events
    public delegate void RayAct(RaycastHit hit); // "Raycast Actions"

    // Event for when the user points at specific tissue-block with Right hand Oculus Quest Controller and pulls the Index Trigger of Right hand Oculus Quest Controller
    public static event RayAct OnSelected;
    // Event for when the user hovers over specific tissue-block in tissue with Right hand Oculus Quest Controller
    public static event RayAct OnHover;
    // Event for when user is not pointing at any of the tissue-blocks with Right hand Oculus Quest Controller
    public static event RayAct SetToDefault;

    // References Input Action created "XRI RightHand/SelectTissue"

    public InputActionReference triggerPressed;

    private bool _isSceneBuilt;

    private void OnEnable()
    {
        SceneBuilder.OnSceneBuilt += () => _isSceneBuilt = true;
    }

    private void OnDestroy()
    {
        SceneBuilder.OnSceneBuilt -= () => _isSceneBuilt = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Records structure used to get information back from a raycast.
        RaycastHit hit;
        // Casting new ray into the scene
        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));

        // If our ray hits a collider somewhere in the scene, do:
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // Debug.Log((hit.collider.name)); // Logs the name of the gameObject our Raycast hits
            // Compare if the object's tag was "TissueBlock"
            if (hit.collider.CompareTag("TissueBlock"))
            {
                if (!_isSceneBuilt) return;
                // Invoke OnHover event
                OnHover?.Invoke(hit); // '?' is an elegant way to check whether null without using an additional if statement

                // Check if Index Trigger of Right hand Oculus Quest Controller has been pressed

                if (triggerPressed.action.inProgress)

                {
                    // Invoke event responsible for setting Selection colour
                    OnSelected?.Invoke(hit);
                }
            }
        }
        else
        {
            // If raycast hits objects other than those with "TissueBlock" tag, then invoke event responsible for setting Default colour
            SetToDefault?.Invoke(hit);
        }
    }

    public void OnABCD()
    {
        Debug.Log("abc");
    }
}
