using HRAOrganGallery;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class AdjustSphereSize : MonoBehaviour
{
    [Header("Adjust sphere size")]
    [SerializeField] private InputActionReference _increaseSphereSize;
    [SerializeField] private float _rate;
    [SerializeField] private float _min;
    [SerializeField] private float _max;

    //set ability to use given mode switch
    [SerializeField] private bool _canUse = false;
    public float SphereSizeMax { get { return _max / 2f; } }
    private Transform _sphere;

    private void OnEnable()
    {
        _sphere = GetComponent<Transform>();
        _increaseSphereSize.action.performed += SetSphereSize;
        UserInputStateManager.OnStateChanged += (UserInputState newState) =>
        {
            _canUse = newState == UserInputState.TissueBlockExplode;
        };
    }

    private void OnDestroy()
    {
        _increaseSphereSize.action.performed -= SetSphereSize;
    }



    private void SetSphereSize(InputAction.CallbackContext ctx)
    {
        if (!_canUse) return;
        float inputValue = _rate * ctx.action.ReadValue<Vector2>().y;

        _sphere.localScale += new Vector3(inputValue, inputValue, inputValue);
        if (_sphere.localScale.y > _max)
        {
            _sphere.localScale = new Vector3(_max, _max, _max);
        }
        else if (_sphere.localScale.y < _min)
        {
            _sphere.localScale = new Vector3(_min, _min, _min);
        }
    }

}
