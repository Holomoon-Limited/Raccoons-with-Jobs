using System.Collections;
using System.Collections.Generic;
using Holo.Input;
using UnityEngine;

namespace Holo.Cards
{
    /// <summary>
    /// Monobehaviour used to instantiate cards to a zone
    /// </summary>
    public class CardZone : MonoBehaviour
    {
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color highlightColor;

        public Card HeldCard { get; protected set; }
        public bool HasCard => (HeldCard != null);

        public int Position { get; set; }

        public void AddCardToZone(Card card)
        {
            if (card == null) return;
            this.HeldCard = card;
            card.transform.parent = this.transform;
            card.MoveToPoint(this.transform.position, Quaternion.identity);
        }

        public void RemoveCardFromZone()
        {
            if (HeldCard == null) return;
            HeldCard = null;
        }

        public void HighlightZone()
        {
            this.GetComponentInChildren<SpriteRenderer>().color = highlightColor;
        }

        public void EndZoneHighlight()
        {
            this.GetComponentInChildren<SpriteRenderer>().color = defaultColor;
        }

        public void MoveCardToHand()
        {
            PlayerHand.Instance.AddCardToHand(HeldCard);
            RemoveCardFromZone();
        }

        public void DespawnCard()
        {
            if (HeldCard == null) return;
            Destroy(HeldCard.gameObject);
            HeldCard = null;
        }
    }
}
