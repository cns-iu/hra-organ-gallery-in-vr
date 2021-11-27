using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    private AudioSource m_AudioSource;
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        if (m_AudioSource.isPlaying) return;
        m_AudioSource.Play();
    }

    public void StopMusic()
    {
        m_AudioSource.Stop();
    }
}
