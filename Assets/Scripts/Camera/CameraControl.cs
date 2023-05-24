using System;
using UnityEngine;
using Cinemachine;
using Holo.Input;

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
            playerHandCamera.Priority = 11;
            boardCamera.Priority = 10;
        }

        private void DisplayBoardView()
        {
            playerHandCamera.Priority = 10;
            boardCamera.Priority = 11;
        }
        
        private void DisplayHandView()
        {
            playerHandCamera.Priority = 11;
            boardCamera.Priority = 10;
        }
    }
}
