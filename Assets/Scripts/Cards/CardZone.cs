using System.Collections;
using System.Collections.Generic;
using Holo.Input;
using UnityEngine;

namespace Holo.Cards
{
    /// <summary>
    /// Monobehaviour used to instantiate cards to a zone
    /// </summary>
    public class CardZone : MonoBehaviour, IRaycastable
    {
        public Card HeldCard { get; private set; }
        public bool HasCard => (HeldCard != null);

        public void AddCardToZone(Card card)
        {
            this.HeldCard = card;
            card.transform.parent = this.transform;
            card.MoveToPoint(this.transform.position, Quaternion.identity);
        }

        public void DespawnCard()
        {
            if (HeldCard == null) return;
            Destroy(HeldCard.gameObject, 1f);
            HeldCard = null;
        }

        public void OnEndHover()
        {
            throw new System.NotImplementedException();
        }

        public void OnHover()
        {
            throw new System.NotImplementedException();
        }
    }
}
