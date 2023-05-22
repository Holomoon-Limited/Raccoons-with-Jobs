using System.Collections.Generic;
using System.Linq;
using Holo.Racc.Game;
using UnityEngine;

namespace Holo.Cards
{
    [CreateAssetMenu(fileName = "Deck Manager", menuName = "Cards/New Deck Manager", order = 1)]
    public class DeckManager : ScriptableObject
    {
        [Header("Asset References")]
        [SerializeField] private PhaseHandler phaseHandler;
        
        [field: SerializeField]
        public List<CardData> DeckOfAllCards { get; private set; } = new List<CardData>();

        [field: SerializeField]
        public List<CardData> PoolOfCurrentCards { get; private set; } = new List<CardData>();

        private void OnEnable()
        {
            this.hideFlags = HideFlags.DontUnloadUnusedAsset;
            phaseHandler.OnGameStart += ResetPoolOfCurrentCards;
        }

        private void OnDisable()
        {
            phaseHandler.OnGameStart -= ResetPoolOfCurrentCards;
        }

        public void ResetPoolOfCurrentCards()
        {
            PoolOfCurrentCards.Clear();
            for (int i = 0; i < DeckOfAllCards.Count; i++)
            {
                PoolOfCurrentCards.Add(DeckOfAllCards[i]);
            }

            ShufflePoolOfCurrentCards();
        }

        public void ShufflePoolOfCurrentCards()
        {
            PoolOfCurrentCards = PoolOfCurrentCards.OrderBy(x => Random.value).ToList();
        }

        public CardData DrawCard()
        {
            if (PoolOfCurrentCards.Count <= 0)
            {
                ResetPoolOfCurrentCards();
            }
            
            CardData drawnCard = PoolOfCurrentCards[0];
            RemoveCardFromPool(PoolOfCurrentCards[0]);

            return drawnCard;
        }

        public void RemoveCardFromPool(CardData cardToRemove)
        {
            PoolOfCurrentCards.Remove(cardToRemove);
        }

        public void AddCardToPool(CardData cardToAdd)
        {
            // maximum number of this card type allowed to be in the deck 
            int cardToAddMax = 0;
            for (int i = 0; i < DeckOfAllCards.Count; i++)
            {
                if (DeckOfAllCards[i] == cardToAdd)
                {
                    cardToAddMax++;
                }
            }

            // current number of this card in pool 
            int cardToAddCurrent = 0;
            for (int i = 0; i < PoolOfCurrentCards.Count; i++)
            {
                if (PoolOfCurrentCards[i] == cardToAdd)
                {
                    cardToAddCurrent++;
                }
            }

            // add card into pool if current less than max 
            if (cardToAddCurrent < cardToAddMax)
            {
                PoolOfCurrentCards.Add(cardToAdd);
            }

            else
            {
                Debug.LogWarning($"Could not add {cardToAdd} to pool of current cards because max of this card type already reached.");
            }
        }
    }
}
