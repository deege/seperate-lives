using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Deege.Game.Events;

namespace Deege.Game.Player
{
    public class PlayerInput : MonoBehaviour
    {
        // Events
        [Header("Player Event Channels")]
        [SerializeField] public Vector2EventChannelSO OnPlayerMovement;
        [SerializeField] public BoolEventChannelSO OnPlayerJump;

        private bool controlsAreEnabled = true;
        private PlayerControls inputActions;
        private InputAction moveAction;
        private InputAction fireAction;
        private InputAction jumpAction;
        private InputAction pauseAction;
        private Vector2 rawInput;

        public bool ControlsAreEnabled
        {
            get { return controlsAreEnabled; }
        }

        public void OnEnable()
        {
            inputActions = new PlayerControls();
            moveAction = inputActions.Player.Move;
            moveAction.Enable();
            inputActions.Player.Move.performed += OnPlayerMove;
            inputActions.Player.Move.canceled += OnPlayerMove;

            fireAction = inputActions.Player.Fire;
            fireAction.Enable();
            inputActions.Player.Fire.performed += OnPlayerFirePerformed;
            inputActions.Player.Fire.canceled += OnPlayerFireCanceled;

            pauseAction = inputActions.Player.Pause;
            pauseAction.Enable();
            inputActions.Player.Pause.performed += OnPlayerPause;

            jumpAction = inputActions.Player.Jump;
            jumpAction.Enable();
            inputActions.Player.Jump.performed += OnPlayerJumpPerformed;
            inputActions.Player.Jump.canceled += OnPlayerJumpCanceled;
        }

        public void OnDisable()
        {
            inputActions.Player.Move.performed -= OnPlayerMove;
            inputActions.Player.Move.canceled -= OnPlayerMove;

            inputActions.Player.Fire.performed -= OnPlayerFirePerformed;
            inputActions.Player.Fire.canceled -= OnPlayerFireCanceled;

            inputActions.Player.Jump.performed -= OnPlayerJumpPerformed;
            inputActions.Player.Jump.canceled -= OnPlayerJumpCanceled;
            inputActions.Player.Pause.performed -= OnPlayerPause;
        }

        // Update is called once per frame
        void Update()
        {
            if (controlsAreEnabled)
            {
                OnPlayerMovement?.RaiseEvent(rawInput);
            }
        }

        public void EnableControls()
        {
            // GameSwitchControlsEvent.Trigger("Game Controls", GameControl.Player);
            controlsAreEnabled = true;
        }
        public void DisableControls()
        {
            // GameSwitchControlsEvent.Trigger("Game Controls", GameControl.UI);
            // PlayerShootingStopEvent.Trigger("Stop Player Fire Event");
            controlsAreEnabled = false;
        }

        public void OnPlayerMove(InputAction.CallbackContext obj)
        {
            rawInput = obj.ReadValue<Vector2>();
        }

        public void OnPlayerFirePerformed(InputAction.CallbackContext obj)
        {
            if (controlsAreEnabled)
            {
                // PlayerShootingStartEvent.Trigger("Start Player Fire Event");
            }
        }

        public void OnPlayerFireCanceled(InputAction.CallbackContext obj)
        {
            // PlayerShootingStopEvent.Trigger("Stop Player Fire Event");
        }

        public void OnPlayerJumpPerformed(InputAction.CallbackContext obj)
        {
            if (controlsAreEnabled)
            {
                OnPlayerJump?.RaiseEvent(true);
            }
        }

        public void OnPlayerJumpCanceled(InputAction.CallbackContext obj)
        {
            if (controlsAreEnabled)
            {
                OnPlayerJump?.RaiseEvent(false);
            }
        }

        public void OnPlayerPause(InputAction.CallbackContext obj)
        {
            Debug.Log("Pause pressed");
            if (controlsAreEnabled)
            {
                DisableControls();
                // GamePauseEvent.Trigger("Pause Game");
            }
        }
    }
}