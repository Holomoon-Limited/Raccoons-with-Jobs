using System.Collections.Generic;
using Holo.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Holo.Cards
{
    public class PlayerHand : CardLocation
    {
        public static PlayerHand Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                DontDestroyOnLoad(this.gameObject);
                Instance = this;
            }
        }

        [Header("Temp Player Hand Card Data")]
        [SerializeField] private List<CardData> tempPlayerCardData;

        [SerializeField] InputManager input;
        [SerializeField] private Card baseCard;

        [SerializeField] private Transform minPosition;
        [SerializeField] private Transform maxPosition;
        [SerializeField] private Transform handParent;

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

            InstantiatePlayerCards(tempPlayerCardData);
        }

        private void Update()
        {
            if (SelectedCard == null) return;
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, boardMask))
            {
                SelectedCard.MoveToPoint(hit.point + new Vector3(0f, 2.75f, 0f), Quaternion.identity);
            }
        }

        public void InstantiatePlayerCards(List<CardData> cardData)
        {
            for (int i = 0; i < cardData.Count; i++)
            {
                Card newCard = Instantiate(baseCard, new Vector3(0, 0, 0), Quaternion.identity, this.gameObject.transform);
                newCard.name = cardData[i].CardName;
                newCard.SetActiveLocation(this);
                newCard.DisplayCard(cardData[i]);
            }
        }

        public void SetCardPositionsInHand()
        {
            cardPositions.Clear();
            Vector3 distanceBetweenPoints = Vector3.zero;
            if (HeldCards.Count > 1)
            {
                distanceBetweenPoints = (maxPosition.position - minPosition.position) / (HeldCards.Count - 1);
            }

            for (int i = 0; i < HeldCards.Count; i++)
            {
                cardPositions.Add(minPosition.position + (distanceBetweenPoints * i));
                HeldCards[i].MoveToPoint(cardPositions[i], minPosition.transform.rotation);
                HeldCards[i].Position = i;
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
            HighlightedCard.MoveToPoint(cardPositions[HighlightedCard.Position] + new Vector3(0f, 1f, 0.3f), Quaternion.identity);
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

        public void AddCardToHand(Card card)
        {
            AddCardToLocation(card);
            card.GetComponent<Collider>().enabled = true;
            card.transform.parent = handParent;
            card.name = card.CardData.CardName;
        }

        public override void AddCardToLocation(Card card)
        {
            base.AddCardToLocation(card);
            SetCardPositionsInHand();
        }

        public override void RemoveCardFromLocation(Card card)
        {
            base.RemoveCardFromLocation(card);
            SetCardPositionsInHand();
        }
    }
}
