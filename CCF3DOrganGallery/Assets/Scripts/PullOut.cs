using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// Scale the organ???
// Tissue block position changer for utility 
// Vector3 abc = GameObject.FindGameObjectWithTag("LeftController").transform.position;
// transform.position += (abc - transform.position);
// SmoothLerp and RotateObject are yet to be adapted to tissue-blocks as well

public class PullOut : MonoBehaviour
{
    // To eventually store default position of organ / tissue-block this script is attached to
    private Vector3 _defaultPosition = Vector3.zero;
    // To eventually store default rotation of organ / tissue-block this script is attached to
    private Quaternion _defaultRotation = Quaternion.identity;
    // To eventually store default scale of organ / tissue-block this script is attached to
    private Vector3 _defaultScale = Vector3.one;
    // Duration to lerp over
    private float _duration = 2f;
    // To check if organ is still rotating
    private bool _rotating;
    public InputActionReference buttonPressed;
    
    // Start is called before the first frame update
    void Start()
    {
        SceneBuilder.OnSceneBuilt += InitializeOrganDefaultValues;
        InitializeOrganDefaultValues();
    }

    // Initializing default Position, Rotation, and Scale
    private void InitializeOrganDefaultValues()
    {
        var o = gameObject;
        _defaultPosition = o.transform.position;
        _defaultRotation = o.transform.rotation;
        _defaultScale = o.transform.localScale;
    }
    
    // Update is called once per frame
   private void Update()
    {
        // If primary button ('A' or 'X') on either controller is pressed, return organ that has been displaced smoothly back to original position 
        if (buttonPressed.action.triggered)
        {
            FloatBack();
        }
    }

    // Calls linear interpolation for restoring organ to default position and rotation 
    private void FloatBack()
    {
        StartCoroutine(SmoothLerp(gameObject , _defaultPosition, _duration));
        StartCoroutine(RotateObject(gameObject, _defaultRotation, _duration));
    }
    
    // Smoothly linearly interpolates the displaced organ back to default position
    private IEnumerator SmoothLerp (GameObject obj, Vector3 destination, float time)
    {
        Vector3 startingPos  = obj.transform.position;
        Vector3 finalPos = destination; 
        float elapsedTime = 0;
        
        while (elapsedTime < time)
        {
            obj.transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    
    // Smoothly linearly interpolates the displaced organ back to default rotation
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