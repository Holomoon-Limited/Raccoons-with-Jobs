using System;
using System.Collections.Generic;
using System.Linq;
using Holo.Cards;
using UnityEngine;
using Random = UnityEngine.Random;

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

            // clear playerCardsInPlay when game ends and when Play Phase begins
            phaseHandler.OnGameStart += ClearPlayerCardsInPlay;
            phaseHandler.OnDraftEnd += ClearPlayerCardsInPlay;

            // clear enemyCardsInPlay when game starts and ends
            phaseHandler.OnGameStart += ClearEnemyCardsInPlay;
            phaseHandler.OnGameEnd += ClearEnemyCardsInPlay;

            ClearPlayerCardsInPlay();
        }

        private void OnDisable()
        {
            phaseHandler.OnGameStart -= ClearPlayerCardsInPlay;
            phaseHandler.OnDraftEnd -= ClearPlayerCardsInPlay;
            
            phaseHandler.OnGameStart -= ClearEnemyCardsInPlay;
            phaseHandler.OnGameEnd -= ClearEnemyCardsInPlay;
        }

        private void ClearPlayerCardsInPlay()
        {
            playerCardsInPlay.Clear();
        }

        // cards are returned to deck in AIPlayer.cs
        private void ClearEnemyCardsInPlay()
        {
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
                    enemyCardsInPlay.Add(cardData[i]);
                }
            }
        }
    }
}

