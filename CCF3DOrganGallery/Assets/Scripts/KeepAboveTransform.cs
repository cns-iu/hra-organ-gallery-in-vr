using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepAboveTransform : MonoBehaviour
{
    [SerializeField] private Transform lowest;
    [SerializeField] private Transform highest;


    private void Update()
    {
        if (transform.position.y <= lowest.position.y)
        {
            transform.position = GetLimit(lowest);
        }
        else if (transform.position.y >= highest.position.y)
        {
            transform.position = GetLimit(highest);
        }
    }

    Vector3 GetLimit(Transform limit)
    {
        return new Vector3(transform.position.x, limit.position.y, transform.position.z);
    }
}
