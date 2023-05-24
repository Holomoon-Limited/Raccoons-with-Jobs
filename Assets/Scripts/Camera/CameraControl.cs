using System;
using UnityEngine;
using Cinemachine;
using Holo.Input;
using UnityEngine.SceneManagement;

namespace Holo.Cam
{
    public class CameraControl : MonoBehaviour
    {
        [Header("Asset References")]
        [SerializeField] private InputManager inputManager;
        
        [Header("Component References")]
        [SerializeField] private CinemachineVirtualCamera playerHandCamera;
        [SerializeField] private CinemachineVirtualCamera boardCamera;

        public event Action OnBoardView;
        public event Action OnHandView;

        private Scene thisScene;

        private void OnEnable()
        {
            inputManager.OnMouseScrolledUp += DisplayBoardView;
            inputManager.OnMouseScrolledDown += DisplayHandView;
        }

        private void OnDisable()
        {
            inputManager.OnMouseScrolledUp -= DisplayBoardView;
            inputManager.OnMouseScrolledDown -= DisplayHandView;
        }

        void Start()
        {
            // default to Board View in battle 
            thisScene = SceneManager.GetActiveScene();
            if (thisScene.name.Equals("Battle"))
            {
                DisplayBoardView();
                return;
            }

            playerHandCamera.Priority = 11;
            boardCamera.Priority = 10;
        }

        private void DisplayBoardView()
        {
            playerHandCamera.Priority = 10;
            boardCamera.Priority = 11;
            
            OnBoardView?.Invoke();
        }
        
        private void DisplayHandView()
        {
            // disable hand view in Battle
            if (thisScene.name.Equals("Battle")) return;

            playerHandCamera.Priority = 11;
            boardCamera.Priority = 10;
            
            OnHandView?.Invoke();
        }
    }
}
