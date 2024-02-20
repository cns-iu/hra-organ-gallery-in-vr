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
        public static event Action<UserInputState> OnStateChanged;

        [Header("Materials")]
        [SerializeField] private Material _readyMaterial;
        [SerializeField] private Material _pressedMaterial;
        [SerializeField] private Renderer _renderer;

        [Header("State")]
        [SerializeField] private XRSimpleInteractable _switch;
        [SerializeField] private UserInputState _state = UserInputState.Movement;

        [Header("Animation")]
        [SerializeField] private float _switchTime;
        [SerializeField] private Transform _movementPostion;
        [SerializeField] private Transform _explodePosition;

        private IKeyboardHover _keyboardHoverResponse;
        private void Awake()
        {
            //get materials
            _renderer = GetComponent<Renderer>();
            _readyMaterial = _renderer.material;

            //get interactable
            _switch = GetComponent<XRSimpleInteractable>();

            //get keyboard hover response
            _keyboardHoverResponse = GetComponentInChildren<IKeyboardHover>();

            _switch.selectEntered.AddListener(
                (SelectEnterEventArgs args) =>
                {
                    _state = _state == UserInputState.Movement ? UserInputState.TissueBlockExplode : UserInputState.Movement;
                    StartCoroutine(MoveSwitch());
                    OnStateChanged.Invoke(_state);
                }
                );

            //subscribe to hover enter event
            _switch.hoverEntered.AddListener(
                (HoverEnterEventArgs args) => { _keyboardHoverResponse.OnHoverEnter(); }
                );

            //subscribe to hover exit event
            _switch.hoverExited.AddListener(
                (HoverExitEventArgs args) => { _keyboardHoverResponse.OnHoverExit(); }
                );
        }

        private IEnumerator MoveSwitch()
        {
            float elapsedTime = 0f;

            //create List of Vector3 to hold the positions in the right direction
            List<Vector3> movements = new List<Vector3>();

            //determine direction of switch
            switch (_state)
            {
                case UserInputState.Movement:
                    movements.Add(_explodePosition.position);
                    movements.Add(_movementPostion.position);
                    break;
                case UserInputState.TissueBlockExplode:
                    movements.Add(_movementPostion.position);
                    movements.Add(_explodePosition.position);
                    break;
                default:
                    break;
            }

            _renderer.material = _pressedMaterial;

            while (elapsedTime < _switchTime)
            {
                elapsedTime += Time.deltaTime;

                float t = elapsedTime / _switchTime;

                _switch.gameObject.transform.position = Vector3.Lerp(movements[0], movements[1], t);
                yield return null;
            }

            _renderer.material = _readyMaterial;
        }
    }
}
