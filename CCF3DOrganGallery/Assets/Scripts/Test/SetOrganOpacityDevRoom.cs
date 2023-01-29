using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOrganOpacityDevRoom : MonoBehaviour
{
    [SerializeField] private float _alpha;
    private void Awake()
    {
        Utils.SetOrganOpacity(gameObject, _alpha);
    }
}
