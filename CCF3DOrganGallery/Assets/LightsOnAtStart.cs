using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsOnAtStart : MonoBehaviour
{
    public float m_MaxIntensity;
    public float m_Duration;
    public float m_Delay;
    private Light m_Light;
    // Start is called before the first frame update
    void Start()
    {
        m_Light = GetComponent<Light>();
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(m_Delay);
        StartCoroutine(TurnOnLight());
    }

    IEnumerator TurnOnLight()
    {
        float elapsedTime = 0f;

        while (elapsedTime < m_Duration)
        {
            m_Light.intensity = Mathf.Lerp(0f, m_MaxIntensity, elapsedTime / m_Duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
