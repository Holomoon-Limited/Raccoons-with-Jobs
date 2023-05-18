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
        public Card HeldCard { get; protected set; }
        public bool HasCard => (HeldCard != null);

        public void AddCardToZone(Card card)
        {
            this.HeldCard = card;
            card.transform.parent = this.transform;
            card.MoveToPoint(this.transform.position, Quaternion.identity);
        }

        public void RemoveCardFromZone()
        {
            if (HeldCard == null) return;
            HeldCard = null;
        }

        public void DespawnCard()
        {
            if (HeldCard == null) return;
            Destroy(HeldCard.gameObject, 1f);
            HeldCard = null;
        }
    }
}
