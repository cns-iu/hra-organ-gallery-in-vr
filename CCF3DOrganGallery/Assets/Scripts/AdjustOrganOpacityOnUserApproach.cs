using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AdjustOrganOpacityOnUserApproach : MonoBehaviour
{
    [Header("Fade")]
    [SerializeField] private float maxAlpha = .8f;
    [SerializeField] private float minAlpha = .5f;
    [SerializeField] private float fadeDuration = 10f;

    [Header("Collider")]
    [SerializeField] private float radiusMultiplier = 2f;
    [SerializeField] private SphereCollider col;
    public int OpacityOnApproach { get; }

    private void OnTriggerEnter(Collider other)
    {
        SceneBuilder.SetOrganOpacity(gameObject, maxAlpha);
    }

    private void OnTriggerExit(Collider other)
    {
        SceneBuilder.SetOrganOpacity(gameObject, minAlpha);
    }

    public void SetCollider()
    {
        SphereCollider col = GetComponent<SphereCollider>();
        col.radius = col.radius * radiusMultiplier;
    }
}
