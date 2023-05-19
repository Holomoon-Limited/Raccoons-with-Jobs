using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Holo.Racc.Game;

namespace Holo.Racc.Play
{
    public class EndPlayPhaseButton : MonoBehaviour
    {
        [Header("Scene References")] 
        [SerializeField] private PlayerBoard playerBoard;

        private Button button;
        void Awake()
        {
            button = GetComponent<Button>();
        }
        
        void Update()
        {
            if (!playerBoard.CanEndPlayPhase)
            {
                button.interactable = false;
            }
            if (playerBoard.CanEndPlayPhase)
            {
                button.interactable = true;
            }
        }
    }
}
