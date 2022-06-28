using System.Collections;
using UnityEngine;

// Scale the organ

public class PullOut : MonoBehaviour
{
    // [SerializeField] private GameObject rightController;
    // private Collider _organ; 
    private Vector3 _defaultPosition = Vector3.zero;
    private Quaternion _defaultRotation = Quaternion.identity;
    private Vector3 _defaultScale = Vector3.one;
    private bool _organContact;
    private float _duration = 2f;
    private bool _rotating;
    private bool stillHolding;
    
    // Start is called before the first frame update
    void Start()
    {
        // SceneBuilder.OnSceneBuilt += InitializeOrganDefaultValues; // If commented, that is because SceneBuilder is not inside the current scene
        InitializeOrganDefaultValues();
    }

    // Initializing default Position, Rotation, and Scale
    private void InitializeOrganDefaultValues()
    {
        var o = gameObject;
        _defaultPosition = o.transform.position;
        _defaultRotation = o.transform.rotation;
        _defaultScale = o.transform.localScale;
        //_organ = GetComponent<Collider>();
    }
    
    // Update is called once per frame
    // void Update()
    // {
    //     // Checking to make sure Rotation is correct
    //     Debug.Log("default Rotation: " + _defaultRotation.eulerAngles);
    //     Debug.Log("Rotation: " + transform.rotation.eulerAngles);
    //     
    //     // Check if the right controller's position is within the bounds of the organ's collider
    //     if(_organ.bounds.Contains(rightController.transform.position))
    //     {
    //         _organContact = true;
    //     }
    //     else
    //     {
    //         // Upon releasing the organ at different location/rotation, starting coroutines to lerp the organ back to original position and rotation
    //         if (_organContact)
    //         {
    //             StartCoroutine(SmoothLerp(_duration));
    //             StartCoroutine(RotateObject(gameObject, _defaultRotation, _duration));
    //         }
    //         _organContact = false;
    //     }
    //
    //     // Maps the organ's position and rotation to that of the right controller 
    //     if (_organContact)
    //     {
    //         var transform1 = transform;
    //         transform1.position = rightController.transform.position;
    //         transform1.rotation = rightController.transform.rotation;
    //     }
    // }
    
    // Smoothly returning organ to default position
    
    private IEnumerator SmoothLerp (float time)
    {
        Vector3 startingPos  = transform.position;
        Vector3 finalPos = _defaultPosition; //transform.position + (transform.forward * 5);
        float elapsedTime = 0;
        
        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    // public void OnStartHolding()
    // {
    //     stillHolding = true;
    //     OnHoldOrgan();
    // }
    //
    // private void OnHoldOrgan()p
    // {
    //     transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 5);
    //     Debug.Log("hi");
    //     if (stillHolding)
    //     {
    //         OnHoldOrgan();
    //     }
    // }

    public void FloatBack()
    {
        StartCoroutine(SmoothLerp(_duration));
        StartCoroutine(RotateObject(gameObject, _defaultRotation, _duration));
        //stillHolding = false;
        Debug.Log("Float back method called.");
        Debug.Log("default Rotation: " + _defaultRotation.eulerAngles);
        Debug.Log("Rotation: " + transform.rotation.eulerAngles);
        Debug.Log("default Rotation: " + _defaultPosition);
        Debug.Log("Rotation: " + transform.position);
    }
    
    // Smoothly returning organ to default rotation
    IEnumerator RotateObject(GameObject gameObjectToMove, Quaternion newRot, float duration)
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