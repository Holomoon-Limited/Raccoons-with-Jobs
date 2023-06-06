using System.Collections;
using System.Collections.Generic;
using Holo.Cards;
using UnityEngine;
using Holo.Input;
using Holo.Racc.Game;
using System;

namespace Holo.Racc.Draft
{
    /// <summary>
    /// Singleton responsible for displaying dealt cards during Draft selection
    /// </summary>
    public class Dealer : CardLocation, IGamepadLocation
    {
        public static Dealer Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        [Header("Asset References")]
        [SerializeField] private DeckManager deck;
        [SerializeField] private InputManager input;
        [SerializeField] private DraftHandler draftHandler;
        [SerializeField] private TransitionHandler transitionHandler;
        [SerializeField] private Card cardPrefab;

        [Header("Prefab References")]
        [SerializeField][Tooltip("The locations the cards will be spawned to")] private List<CardZone> cardZones = new List<CardZone>();
        [SerializeField] private Transform cardSpawnLocation;

        [Header("Tuning Variables")]
        [SerializeField][Min(0f)] private float timeBetweenCards = 0.2f;

        private List<Vector3> cardPositions = new List<Vector3>();

        //Tracks how many picks the player has made 
        private int playerPicks = 0;

        private void OnEnable()
        {
            input.OnSubmitPressed += MoveCardToHand;
            draftHandler.OnStartDraft += DealCards;
            transitionHandler.OnTransitionOver += StartDraft;
            draftHandler.OnPlayerPick += SelectFirstCard;
        }

        private void OnDisable()
        {
            input.OnSubmitPressed -= MoveCardToHand;
            draftHandler.OnStartDraft -= DealCards;
            transitionHandler.OnTransitionOver -= StartDraft;
            draftHandler.OnPlayerPick -= SelectFirstCard;
        }

        private void StartDraft()
        {
            draftHandler.StartDraft();
        }

        public void DealCards()
        {
            deck.ShufflePoolOfCurrentCards();
            StartCoroutine(Co_DealCards());
        }

        private IEnumerator Co_DealCards()
        {
            for (int i = 0; i < cardZones.Count; i++)
            {
                Card card = Instantiate(cardPrefab, cardSpawnLocation.position, Quaternion.Euler(0, 0, 180f));
                card.SetCardData(deck.DrawCard());
                cardZones[i].AddCardToZone(card);
                card.Position = i;
                cardPositions.Add(cardZones[i].transform.position);
                AddCardToLocation(card);
                AudioPlayer.Instance.PlayDealClip();
                yield return new WaitForSeconds(timeBetweenCards);
            }
            if (input.GamepadEnabled)
            {
                SetHighlightedCard(HeldCards[0]);
                GamepadControls.Instance.activeLocation = this;
            }
        }

        public override void AddCardToLocation(Card card)
        {
            if (HeldCards.Contains(card)) return;
            HeldCards.Add(card);

            card.SetActiveLocation(this);
        }

        public override void RemoveCardFromLocation(Card card)
        {
            if (HeldCards.Contains(card))
            {
                HeldCards.RemoveAt(card.Position);
                cardPositions.RemoveAt(card.Position);
            }
            UpdateCardPositions();
        }

        private void UpdateCardPositions()
        {
            for (int i = 0; i < HeldCards.Count; i++)
            {
                HeldCards[i].Position = i;
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

        public override void SetSelectedCard(Card card)
        {
            PlayerHand.Instance.AddCardToHand(card);
            card.SetActiveLocation(PlayerHand.Instance);
        }

        private void MoveCardToHand()
        {
            if (HighlightedCard == null) return;
            RemoveCardFromLocation(HighlightedCard);
            SetSelectedCard(HighlightedCard);
            HighlightedCard = null;
            playerPicks++;
            if (playerPicks >= draftHandler.Picks)
            {
                draftHandler.ProgressPhase();
                playerPicks = 0;
            }
            else
            {
                SelectFirstCard(playerPicks);
            }
        }

        private void SelectFirstCard(int picks)
        {
            SetHighlightedCard(HeldCards[0]);
        }

        public void OnControllerActivated()
        {
            SetHighlightedCard(HeldCards[0]);
        }

        public void OnNavigate(float value)
        {
            if (value > 0)
            {
                int index = HighlightedCard.Position + 1;
                if (index > HeldCards.Count - 1) index = 0;
                SetHighlightedCard(HeldCards[index]);
            }
            else if (value < 0)
            {
                int index = HighlightedCard.Position - 1;
                if (index < 0) index = HeldCards.Count - 1;
                SetHighlightedCard(HeldCards[index]);
            }
        }

        public void OnSubmit()
        {
            MoveCardToHand();
        }

        public void OnCancel()
        {

        }
    }
}
