using System.Collections;
using System.Collections.Generic;
using Holo.Racc.Game;
using UnityEngine;

namespace Holo.Racc.UI
{
    public class PickCardInstructions : MonoBehaviour
    {
        [Header("Asset References")] 
        [SerializeField] private DraftHandler draftHandler;
        
        [Header("Component References")] 
        [SerializeField] private GameObject[] children;

        private void OnEnable()
        {
            draftHandler.OnPlayerPick += PlayerPickInstructions;
            draftHandler.OnAIPick += OpponentPickInstructions;
        }

        private void OnDisable()
        {
            draftHandler.OnPlayerPick -= PlayerPickInstructions;
            draftHandler.OnAIPick -= OpponentPickInstructions;
        }

        private void Start()
        {
            PlayerPickInstructions(1);
        }

        private void PlayerPickInstructions(int playerPickCount)
        {
            for (int i = 0; i < children.Length; i++)
            {
                if (i == playerPickCount - 1)
                {
                    children[i].SetActive(true);
                }
                else
                { 
                    children[i].SetActive(false);
                }
            }
        }
        
        private void OpponentPickInstructions(int aiPickCount)
        {
            for (int i = 0; i < children.Length; i++)
            {
                if (i == 2)
                {
                    children[i].SetActive(true);
                }
                else
                { 
                    children[i].SetActive(false);
                }
            }
        }
    }
}
