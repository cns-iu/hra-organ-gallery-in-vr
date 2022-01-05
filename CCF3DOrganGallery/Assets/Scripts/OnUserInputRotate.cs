using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnUserInputRotate : MonoBehaviour
{
    [SerializeField] private float _degrees;
    private void OnEnable()
    {
        UserInputModule.OnUpdateJoystickValueEvent += Rotate;
    }

    private void OnDestroy()
    {
        UserInputModule.OnUpdateJoystickValueEvent -= Rotate;
    }

    void Rotate(float value)
    {
        Quaternion rotation = Quaternion.Euler(0, _degrees * value, 0);
        transform.Rotate(0, _degrees * value, 0);
    }
}
