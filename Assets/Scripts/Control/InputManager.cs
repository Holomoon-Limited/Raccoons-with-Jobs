using System;
using UnityEngine;

namespace Holo.Input
{
    [CreateAssetMenu(fileName = "Input Manager", menuName = "Input/New Input Manager", order = 0)]
    public class InputManager : ScriptableObject
    {
        public PlayerControls Controls { get; private set; }

        public event Action OnSubmitPressed;
        public event Action OnCancelPressed;

        private void OnEnable()
        {
            this.hideFlags = HideFlags.DontUnloadUnusedAsset;

            Controls = new PlayerControls();
            Controls.Enable();

            Controls.Player.Submit.performed += ctx => SubmitPressed();
            Controls.Player.Cancel.performed += ctx => CancelPressed();
        }

        public void SubmitPressed()
        {
            OnSubmitPressed?.Invoke();
        }

        public void CancelPressed()
        {
            OnCancelPressed?.Invoke();
        }
    }
}