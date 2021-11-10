using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEventHandler : MonoBehaviour
{
    public delegate void CollisionStart(GameObject gameObject);
    public static event CollisionStart CollisionStartEvent;
    public delegate void CollisionEnd();
    public static event CollisionEnd CollisionEndEvent;
    private void OnTriggerStay(Collider other)
    {
        CollisionStartEvent?.Invoke(this.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        CollisionEndEvent?.Invoke();
    }
}
