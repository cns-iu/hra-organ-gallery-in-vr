using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    private static Object instance = null;
    private AudioSource m_AudioSource;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            m_AudioSource = GetComponent<AudioSource>();
        }
        
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
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
