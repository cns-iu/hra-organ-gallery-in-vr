using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrecklesControl : MonoBehaviour
{
    ParticleSystem m_ParticleSystem;
    private void Awake()
    {
        m_ParticleSystem = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = m_ParticleSystem.main;
        main.startLifetime = Mathf.Infinity;
    }
}
