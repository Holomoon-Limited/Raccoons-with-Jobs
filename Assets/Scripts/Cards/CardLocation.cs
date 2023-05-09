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

        protected List<Card> heldCards = new List<Card>();

        private void OnEnable()
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
        }

        public abstract void SetHighlightedCard(Card card);
        public abstract void SetSelectedCard(Card card);
        public abstract void AddCardToLocation(Card card);
        public abstract void RemoveCardFromLocation(Card card);

        public void ResetData()
        {
            SelectedCard = null;
            HighlightedCard = null;
        }
    }
}
