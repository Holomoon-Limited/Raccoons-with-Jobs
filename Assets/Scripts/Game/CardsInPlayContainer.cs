using System;
using System.Collections.Generic;
using Holo.Cards;
using UnityEngine;

namespace Holo.Racc.Game
{
    [CreateAssetMenu(fileName = "CardsInPlay Container", menuName = "Game/New CardsInPlay Container", order = 0)]
    public class CardsInPlayContainer : ScriptableObject
    {
        [Header("Asset References")]
        [SerializeField] private PhaseHandler phaseHandler;
        
        [field: Header("Lists of Data")] 
        [field: SerializeField] public List<CardData> playerCardsInPlay { get; private set; }
        [field: SerializeField] public List<CardData> enemyCardsInPlay { get; private set; }

        private void OnEnable()
        {
            this.hideFlags = HideFlags.DontUnloadUnusedAsset;
            
            // clear CardsInPlay when game ends and when Play Phase begins
            phaseHandler.OnGameStart += ClearCardsInPlay;
            phaseHandler.OnDraftEnd += ClearCardsInPlay;

            ClearCardsInPlay();
        }

        private void OnDisable()
        {
            phaseHandler.OnGameStart -= ClearCardsInPlay;
            phaseHandler.OnDraftEnd -= ClearCardsInPlay;
        }

        private void ClearCardsInPlay()
        {
            playerCardsInPlay.Clear();
            enemyCardsInPlay.Clear();
        }

        public void UpdateCardsInPlay(bool playersCards, List<CardData> cardData)
        {
            if (playersCards)
            {
                playerCardsInPlay.Clear();
                for (int i = 0; i < cardData.Count; i++)
                {
                    playerCardsInPlay.Add(cardData[i]);
                }
            }

            if (!playersCards)
            {
                enemyCardsInPlay.Clear();
                for (int i = 0; i < cardData.Count; i++)
                {
                    playerCardsInPlay.Add(cardData[i]);
                }
            }
        }
    }
}

