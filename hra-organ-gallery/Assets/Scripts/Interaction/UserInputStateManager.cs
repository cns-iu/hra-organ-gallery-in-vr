using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System;

namespace HRAOrganGallery
{
    public enum UserInputState { Movement, TissueBlockExplode }

    /// <summary>
    /// A class to handle user input states, i.e., movement vs. tissue block explode interaction
    /// </summary>
    public class UserInputStateManager : MonoBehaviour
    {
        public static UserInputStateManager Instance { get; private set; }
        public static event Action<UserInputState> OnStateChanged;

        [field: SerializeField] public UserInputState State { get; set; }

        [Header("Input State Buttons")]
        [SerializeField] private List<InputStateButton> inputStateButtons = new List<InputStateButton>();

        private void Awake()
        {
            inputStateButtons.ForEach(i =>
            {
                i.GetComponent<XRBaseInteractable>().selectEntered.AddListener(
                (SelectEnterEventArgs args) =>
                {
                    i.OnSelect();
                    OnStateChanged.Invoke(State);
                }
                );
            });


            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

    }
}
