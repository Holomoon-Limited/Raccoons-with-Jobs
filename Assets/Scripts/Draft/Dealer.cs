using System.Collections;
using System.Collections.Generic;
using Holo.Cards;
using UnityEngine;
using Holo.Input;

namespace Holo.Racc.Draft
{
    public class Dealer : CardLocation
    {
        [SerializeField] InputManager input;
        [SerializeField] DraftHandler draftHandler;
        [SerializeField] private List<CardZone> cardZones = new List<CardZone>();
        [SerializeField] private Card cardPrefab;
        [SerializeField] private Transform deckLocation;
        [SerializeField][Min(0f)] private float timeBetweenCards = 0.2f;

        private List<Vector3> cardPositions = new List<Vector3>();

        private void OnEnable()
        {
            input.OnSubmitPressed += MoveCardToHand;
        }

        private void OnDisable()
        {
            input.OnSubmitPressed += MoveCardToHand;
        }

        private void Start()
        {
            DealCards();
        }

        public void DealCards()
        {
            StartCoroutine(Co_DealCards());
        }

        private IEnumerator Co_DealCards()
        {
            for (int i = 0; i < cardZones.Count; i++)
            {
                Card card = Instantiate(cardPrefab, deckLocation.position, Quaternion.Euler(0, 0, 180f));
                cardZones[i].AddCardToZone(card);
                card.Position = i;
                cardPositions.Add(cardZones[i].transform.position);
                AddCardToLocation(card);
                yield return new WaitForSeconds(timeBetweenCards);
            }
        }

        public override void AddCardToLocation(Card card)
        {
            if (heldCards.Contains(card)) return;
            heldCards.Add(card);

            card.SetActiveLocation(this);
        }

        public override void RemoveCardFromLocation(Card card)
        {
            if (heldCards.Contains(card))
            {
                heldCards.Remove(card);
            }
        }

        public override void SetHighlightedCard(Card card)
        {
            if (HighlightedCard != null && HighlightedCard != card)
            {
                HighlightedCard.MoveToPoint(cardPositions[HighlightedCard.Position], Quaternion.identity);
                HighlightedCard = null;
            }
            if (card != null)
            {
                HighlightedCard = card;
            }
            if (HighlightedCard == null) return;
            if (SelectedCard != null) return;
            HighlightedCard.MoveToPoint(cardPositions[HighlightedCard.Position] + new Vector3(0f, 1f, 0f), Quaternion.identity);
        }

        private void MoveCardToHand()
        {
            if (HighlightedCard == null) return;
            SetSelectedCard(HighlightedCard);
            RemoveCardFromLocation(HighlightedCard);
            HighlightedCard = null;
        }

        public override void SetSelectedCard(Card card)
        {
            PlayerHand hand = GameObject.FindObjectOfType<PlayerHand>();
            if (hand == null) return;
            hand.AddCardToHand(card);
            card.SetActiveLocation(hand);
        }
    }
}
