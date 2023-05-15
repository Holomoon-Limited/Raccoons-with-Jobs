using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Holo.Cards
{
    [CreateAssetMenu(fileName = "Deck Manager", menuName = "Cards/New Deck Manager", order = 1)]
    public class DeckManager : ScriptableObject
    {
        [field: SerializeField]
        public List<CardData> DeckOfAllCards { get; private set; } = new List<CardData>();

        [field: SerializeField] 
        public List<CardData> PoolOfCurrentCards { get; private set; } = new List<CardData>();

        private void OnEnable()
        {
            this.hideFlags = HideFlags.DontUnloadUnusedAsset;
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
            PoolOfCurrentCards = PoolOfCurrentCards.OrderBy( x => Random.value ).ToList( );
        }

        public CardData DrawCard()
        {
            CardData drawnCard = PoolOfCurrentCards[0];
            RemoveCardFromPool(PoolOfCurrentCards[0]);
            
            return drawnCard;
        }

        public void RemoveCardFromPool(CardData cardToRemove)
        {
            PoolOfCurrentCards.Remove(cardToRemove);
            Debug.Log($"Card to remove {cardToRemove}");
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

            // will add the card into the deck if current less than max 
            if (cardToAddCurrent < cardToAddMax)
            {
                PoolOfCurrentCards.Add(cardToAdd);
                Debug.Log($"Card to add {cardToAdd}");
            }

            else
            {
                Debug.LogWarning($"Could not add {cardToAdd} to pool of current cards because max of this card type already reached.");
            }
        }
    }
}
