using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class FloatBackTissueBlocks : MonoBehaviour
{
    // To eventually store default position of tissue-block this script is attached to
    private Vector3 _defaultPosition = Vector3.zero;
    
    // To store default local position of tissue-block with respect to parent organ
    private Vector3 _defaultDefaultPosition = Vector3.zero;
    
    // To eventually store default rotation of tissue-block this script is attached to
    private Quaternion _defaultRotation = Quaternion.identity;

    // To store default local rotation of tissue-block with respect to parent organ
    private Quaternion _defaultDefaultRotation = Quaternion.identity;
    
    // To eventually store default scale of tissue-block this script is attached to
    private Vector3 _defaultScale = Vector3.one;

    // To store default local scale of tissue-block with respect to parent organ
    private Vector3 _defaultDefaultScale = Vector3.one;
    
    // Reference to parent organ
    private GameObject _parentOrgan;
    
    // Duration to lerp over
    private float _duration = 1.5f;

    // To check if tissue-block is still rotating
    private bool _rotating;

    // Reference to Input Action Binding
    public InputActionReference buttonPressed;
    
    // Reference for PullOutStateChanger
    private PullOutStateChanger _pullOut;

    // Start is called before the first frame update
    void Start()
    {
        SceneBuilder.OnSceneBuilt += InitializeTissueBlockDefaultValues;
        InitializeTissueBlockDefaultValues();
        _parentOrgan = GameObject.Find("VH_F_Kidney_Left_v1.1");
        _pullOut = GameObject.Find("RightHand Controller").GetComponent<PullOutStateChanger>();
    }
    
    // Initializing default Position, Rotation, and Scale
    public void InitializeTissueBlockDefaultValues()
    {
        var tb = gameObject;
        _defaultPosition = tb.transform.position;
        // _defaultLocalPosition = tb.transform.localPosition;
        _defaultRotation = tb.transform.rotation;
        // _defaultLocalRotation = tb.transform.localRotation;
        _defaultScale = tb.transform.localScale;
    }

    // Update is called once per frame
    private void Update()
    {
        // If primary button ('A' or 'X') on either controller is pressed, return tissue-block that has been displaced smoothly back to original position 
        if (buttonPressed.action.triggered && !_pullOut.organState)
        {
            FloatBack();
            Debug.Log("Float back tissue-blocks triggered");
        }
    }

    // Calls linear interpolation for restoring tissue-block to default position and rotation
    public void FloatBack()
    {
        // var position = _parentOrgan.transform.position;
        
        StartCoroutine(SmoothLerp(gameObject, _defaultPosition, _duration));
        StartCoroutine(RotateObject(gameObject, _defaultRotation, _duration));
    }

    // Smoothly linearly interpolates the displaced tissue-block back to default position
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

    // Smoothly linearly interpolates the displaced tissue-block back to default rotation
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