using System.Collections.Generic;
using Holo.Input;
using Holo.Racc.Game;
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

        [Header("Asset References")]
        [SerializeField] InputManager input;
        [SerializeField] private PhaseHandler phaseHandler;
        [SerializeField] private DeckManager deckManager;
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

            phaseHandler.OnPlayEnd += ReturnCards;
        }

        private void OnDisable()
        {
            input.OnSubmitPressed -= SelectCard;
            input.OnCancelPressed -= CancelSelection;

            phaseHandler.OnPlayEnd -= ReturnCards;
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
                SelectedCard.MoveToPoint(hit.point + new Vector3(0f, 3.5f, 0f), Quaternion.identity);
            }
        }

        public void InstantiatePlayerCards(List<CardData> cardData)
        {
            for (int i = 0; i < cardData.Count; i++)
            {
                Card newCard = Instantiate(baseCard, new Vector3(0, 0, 0), Quaternion.identity, handParent.transform);
                newCard.name = cardData[i].CardName;
                newCard.SetActiveLocation(this);
                newCard.SetCardData(cardData[i]);
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
            card.IsPlayers = true;
            card.GetComponent<Collider>().enabled = true;
            card.transform.parent = handParent;
            card.name = card.CardData.CardName;
            card.SetActiveLocation(this);
            AudioPlayer.Instance.PlayInteractClip();
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

        // returns excess cards to the DeckManager at end of PlayPhase
        private void ReturnCards()
        {
            foreach (Card card in HeldCards)
            {
                deckManager.AddCardToPool(card.CardData);
                Destroy(card.gameObject);
            }

            HeldCards.Clear();

            // List<CardData> excessCardData = new List<CardData>();

            // for (int i = 0; i < handParent.transform.childCount; ++i)
            // {
            //     GameObject child = handParent.transform.GetChild(i).gameObject;
            //     Card childCard = child.GetComponent<Card>();
            //     excessCardData.Add(childCard.CardData);
            //     Destroy(child);
            // }

            // for (int i = 0; i < excessCardData.Count; i++)
            // {
            //     deckManager.AddCardToPool(excessCardData[i]);
            // }

            // HeldCards.Clear();
            // cardPositions.Clear();
        }
    }
}
