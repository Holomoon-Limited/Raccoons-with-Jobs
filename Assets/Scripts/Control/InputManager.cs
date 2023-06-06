using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Holo.Input
{
    [CreateAssetMenu(fileName = "Input Manager", menuName = "Input/New Input Manager", order = 0)]
    public class InputManager : ScriptableObject
    {
        public PlayerControls Controls { get; private set; }

        public event Action OnSubmitPressed;
        public event Action OnGamepadSubmit;
        public event Action OnCancelPressed;
        public event Action OnGamepadCancel;
        public event Action OnMouseScrolledUp;
        public event Action OnMouseScrolledDown;
        public event Action OnStartBattle;

        public bool GamepadEnabled => Controls.Gamepad.enabled == true;

        private void OnEnable()
        {
            this.hideFlags = HideFlags.DontUnloadUnusedAsset;

            Controls = new PlayerControls();
            Controls.Enable();

            Controls.Keyboard.Submit.performed += ctx => SubmitPressed();
            Controls.Gamepad.Submit.performed += ctx => GamepadSubmitPressed();

            Controls.Keyboard.Cancel.performed += ctx => CancelPressed();
            Controls.Gamepad.Cancel.performed += ctx => GamepadCancelPressed();

            Controls.Gamepad.Battle.performed += ctx => StartBattle();

            Controls.Keyboard.MouseScroll.performed += ctx => MouseScrolled(ctx.ReadValue<float>());
            Controls.Gamepad.Scroll.performed += ctx => MouseScrolled(ctx.ReadValue<float>());
        }

        public void EnableKeyboardControls()
        {
            Controls.Gamepad.Disable();
            Controls.Keyboard.Enable();
            Debug.Log("Mouse controls enabled");
        }

        public void EnableGamepadControls()
        {
            Controls.Keyboard.Disable();
            Controls.Gamepad.Enable();
            Debug.Log("Gamepad controls enabled");
        }

        public void EnableInput()
        {
            Controls.Enable();
        }

        public void DisableInput()
        {
            Controls.Disable();
        }

        public void SubmitPressed()
        {
            OnSubmitPressed?.Invoke();
        }

        public void GamepadSubmitPressed()
        {
            OnGamepadSubmit?.Invoke();
        }

        public void CancelPressed()
        {
            OnCancelPressed?.Invoke();
        }

        public void GamepadCancelPressed()
        {
            OnGamepadCancel?.Invoke();
        }

        public void StartBattle()
        {
            OnStartBattle?.Invoke();
        }

        public void MouseScrolled(float value)
        {
            if (value > 0)
            {
                OnMouseScrolledUp?.Invoke();
            }

            else if (value < 0)
            {
                OnMouseScrolledDown?.Invoke();
            }
        }
    }
}