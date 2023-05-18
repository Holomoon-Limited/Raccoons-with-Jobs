using System;
using System.Collections.Generic;
using Holo.Cards;
using UnityEngine;

namespace Holo.Racc.Game
{
    [CreateAssetMenu(fileName = "CardsInPlay Container", menuName = "Game/New CardsInPlay Container", order = 0)]
    public class CardsInPlayContainer : ScriptableObject
    {
        [Header("Lists of Data")] 
        [SerializeField] private List<CardData> playerCardsInPlay = new List<CardData>();
        [SerializeField] private List<CardData> enemyCardsInPlay = new List<CardData>();

        private void OnEnable()
        {
            this.hideFlags = HideFlags.DontUnloadUnusedAsset;
            
            ClearCardsInPlay();
        }

        private void ClearCardsInPlay()
        {
            playerCardsInPlay.Clear();
            enemyCardsInPlay.Clear();
        }
    }
}

