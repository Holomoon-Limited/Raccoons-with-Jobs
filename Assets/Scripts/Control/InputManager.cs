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
        public event Action OnCancelPressed;
        public event Action OnMouseScrolledUp;
        public event Action OnMouseScrolledDown;
        
        private void OnEnable()
        {
            this.hideFlags = HideFlags.DontUnloadUnusedAsset;

            Controls = new PlayerControls();
            Controls.Enable();

            Controls.Player.Submit.performed += ctx => SubmitPressed();
            Controls.Player.Cancel.performed += ctx => CancelPressed();
            
            Controls.Player.MouseScroll.performed += ctx => MouseScrolled();

        }

        public void EnableInput()
        {
            Controls.Player.Enable();
        }

        public void DisableInput()
        {
            Controls.Player.Disable();
        }

        public void SubmitPressed()
        {
            OnSubmitPressed?.Invoke();
        }

        public void CancelPressed()
        {
            OnCancelPressed?.Invoke();
        }

        public void MouseScrolled()
        {
            object objAxisAmount = Controls.Player.MouseScroll.ReadValueAsObject();
            int axisAmount = Convert.ToInt32(objAxisAmount);

            if (axisAmount > 0)
            {
                OnMouseScrolledUp?.Invoke();
            }
            
            else if (axisAmount < 0)
            {
                OnMouseScrolledDown?.Invoke();
            }
        }
    }
}