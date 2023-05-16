using System.Collections.Generic;
using UnityEngine;

namespace Holo.Cards
{
    /// <summary>
    /// MonoBehaviour base class for handling card containers (e.g. Hand, Board)
    /// </summary>
    public abstract class CardLocation : MonoBehaviour
    {
        public Card HighlightedCard { get; set; }
        public Card SelectedCard { get; set; }

        public List<Card> HeldCards { get; private set; } = new List<Card>();

        public abstract void SetHighlightedCard(Card card);
        public abstract void SetSelectedCard(Card card);

        public virtual void AddCardToLocation(Card card)
        {
            if (HeldCards.Contains(card)) return;
            HeldCards.Add(card);
        }
        public virtual void RemoveCardFromLocation(Card card)
        {
            if (this.HeldCards.Contains(card))
            {
                HeldCards.Remove(card);
            }

            if (HighlightedCard == card)
            {
                HighlightedCard = null;
            }
        }
    }
}
