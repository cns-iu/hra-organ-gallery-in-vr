using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRAOrganGallery
{
    public class ParticleSystemKeyboard : MonoBehaviour, IKeyboardHover
    {
        private ParticleSystem _particleSystem;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        public void OnHoverEnter()
        {
            _particleSystem.Play();
        }

        public void OnHoverExit()
        {
            _particleSystem.Stop();
        }
    }
}
