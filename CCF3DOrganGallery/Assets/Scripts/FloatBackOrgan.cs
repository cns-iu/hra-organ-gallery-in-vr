using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Scale the organ???

public class FloatBackOrgan : MonoBehaviour
{
    // To eventually store default position of organ this script is attached to
    private static Vector3 _defaultPosition = Vector3.zero;

    // To eventually store default rotation of organ this script is attached to
    private static Quaternion _defaultRotation = Quaternion.identity;

    // To eventually store default scale of organ this script is attached to
    private Vector3 _defaultScale = Vector3.one;

    // Duration to lerp over
    private float _duration = 1.5f;

    // To check if organ is still rotating
    private bool _rotating;

    // Reference to Input Action Binding
    public InputActionReference buttonPressed;
    
    // Reference for PullOutStateChanger
    private PullOutStateChanger _pullOut;
    
    // Flag to check if Organ Position values have already been initialized;
    private bool _flag; 
    
    // Start is called before the first frame update
    void Start()
    {
        _pullOut = GameObject.Find("RightHand Controller").GetComponent<PullOutStateChanger>();
    }

    // Initializing default Position, Rotation, and Scale
    public void UpdateOrganDefaultValues(List<Vector3> defaultDict)
    {
        _defaultPosition = defaultDict[0];
        _defaultRotation = Quaternion.Euler(defaultDict[1]);
        _defaultScale = defaultDict[2];
    }

    // Update is called once per frame
    private void Update()
    {
        // If primary button ('A' or 'X') on either controller is pressed, return organ that has been displaced smoothly back to original position 
        if (buttonPressed.action.triggered && _pullOut.organState)
        {
            FloatBack();
        }
    }

    // Calls linear interpolation for restoring organ to default position and rotation
    private void FloatBack()
    {
        StartCoroutine(SmoothLerp(gameObject, _defaultPosition, _duration));
        StartCoroutine(RotateObject(gameObject, _defaultRotation, _duration));
    }

    // Linearly interpolates the displaced organ smoothly back to default position
    private IEnumerator SmoothLerp(GameObject obj, Vector3 destination, float time)
    {
        Vector3 startingPos = obj.transform.position;
        Vector3 finalPos = destination;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            obj.transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    // Linearly interpolates the displaced organ smoothly back to default rotation
    private IEnumerator RotateObject(GameObject gameObjectToMove, Quaternion newRot, float duration)
    {
        // Checks if this IEnumerator has already been called
        if (_rotating)
        {
            yield break;
        }

        _rotating = true;

        Quaternion currentRot = gameObjectToMove.transform.rotation;

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            gameObjectToMove.transform.rotation = Quaternion.Lerp(currentRot, newRot, counter / duration);
            yield return null;
        }

        _rotating = false;
    }
}
