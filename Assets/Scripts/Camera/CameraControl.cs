using UnityEngine;
using Cinemachine;

namespace Holo.Cam
{
    public class CameraControl : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera playerHandCamera;
        [SerializeField] private CinemachineVirtualCamera boardCamera;

        void Start()
        {
            playerHandCamera.Priority = 11;
            boardCamera.Priority = 10;
        }

        private void OnGUI()
        {
            GUILayout.Space(25);

            if (GUILayout.Button("Board View"))
            {
                playerHandCamera.Priority = 10;
                boardCamera.Priority = 11;
            }

            if (GUILayout.Button("Player Hand View"))
            {
                playerHandCamera.Priority = 11;
                boardCamera.Priority = 10;
            }
        }
    }
}
