using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Holo.Cards
{
    public class PlayerHand : CardLocation
    {
        [SerializeField] InputManager input;

        [SerializeField] private Transform minPosition;
        [SerializeField] private Transform maxPosition;

        [SerializeField] LayerMask boardMask = -1;

        private List<Vector3> cardPositions = new List<Vector3>();

        private void OnEnable()
        {
            input.OnSubmitPressed += SelectCard;
            input.OnCancelPressed += CancelSelection;
        }

        private void OnDisable()
        {
            input.OnSubmitPressed -= SelectCard;
            input.OnCancelPressed -= CancelSelection;
        }

        private void Start()
        {
            SetCardPositionsInHand();
        }

        private void Update()
        {
            if (SelectedCard == null) return;
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, boardMask))
            {
                SelectedCard.MoveToPoint(hit.point + new Vector3(0f, 2f, 0f), Quaternion.identity);
            }
        }

        private void SetCardPositionsInHand()
        {
            cardPositions.Clear();

            Vector3 distanceBetweenPoints = Vector3.zero;
            if (heldCards.Count > 1)
            {
                distanceBetweenPoints = (maxPosition.position - minPosition.position) / (heldCards.Count - 1);
            }

            for (int i = 0; i < heldCards.Count; i++)
            {
                cardPositions.Add(minPosition.position + (distanceBetweenPoints * i));
                heldCards[i].MoveToPoint(cardPositions[i], minPosition.transform.rotation);
                heldCards[i].Position = i;
            }
        }

        public override void SetHighlightedCard(Card card)
        {
            if (HighlightedCard != null && HighlightedCard != card)
            {
                HighlightedCard.MoveToPoint(cardPositions[HighlightedCard.Position], minPosition.rotation);
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

        public override void SetSelectedCard(Card card)
        {
            if (HighlightedCard == SelectedCard) return;
            if (SelectedCard != null && SelectedCard != card)
            {
                SelectedCard.MoveToPoint(cardPositions[SelectedCard.Position], minPosition.rotation);
                SelectedCard.GetComponent<Collider>().enabled = true;
                SelectedCard = null;
            }
            if (card != null)
            {
                SelectedCard = card;
                SelectedCard.GetComponent<Collider>().enabled = false;
            }
        }

        private void SelectCard()
        {
            if (HighlightedCard == null) return;
            SetSelectedCard(HighlightedCard);
        }

        private void CancelSelection()
        {
            SetSelectedCard(null);
        }

        public override void AddCardToLocation(Card card)
        {
            if (heldCards.Contains(card)) return;
            heldCards.Add(card);
            SetCardPositionsInHand();
        }

        public override void RemoveCardFromLocation(Card card)
        {
            throw new System.NotImplementedException();
        }
    }
}
