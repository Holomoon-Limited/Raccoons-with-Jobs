using System;
using Holo.Cards;
using Holo.Input;
using UnityEngine;

namespace Holo.Racc.Play
{
    public class PlayCardZone : CardZone, IRaycastable
    {
        [Header("Prefab References")]
        [SerializeField] private InputManager input;

        private int thisCardZoneSI => this.gameObject.transform.GetSiblingIndex();
        private bool isHoveredOver;
        public bool wasClicked;

        PlayerBoard board;

        // Audio
        //AudioPlayer audioPlayer;

        // PlayPhase subbed to manage assigned cards
        public static event Action<CardData, int> OnCardAssigned = (cardData, position) => { };

        private void Awake() 
        {
            
        }

        private void OnEnable()
        {
            input.OnSubmitPressed += AddCardToZone;
        }

        private void OnDisable()
        {
            input.OnSubmitPressed -= AddCardToZone;
        }

        public void SetPlayerBoard(PlayerBoard board)
        {
            this.board = board;
        }

        public void OnHover()
        {
            if (board == null) return;
            board.SetHighlightedZone(this);
        }

        public void OnEndHover()
        {
            if (board) board.SetHighlightedZone(null);
            // isHoveredOver = false;
            // wasClicked = false;
        }

        private void AddCardToZone()
        {
            if (board.HighlightedZone != this) return;
            if (this.HeldCard != null)
            {
                PlayerHand.Instance.AddCardToHand(this.HeldCard);
                this.RemoveCardFromZone();
            }
            if (PlayerHand.Instance.SelectedCard != null)
            {
                AddCardToZone(PlayerHand.Instance.SelectedCard);
                PlayerHand.Instance.RemoveCardFromLocation(HeldCard);
                AudioPlayer.Instance.PlayInteractClip();
            }

            // if (isHoveredOver)
            // {
            //     // return if no selected card 
            //     if (PlayerHand.Instance.SelectedCard == null) return;

            //     if (HeldCard == null && !wasClicked)
            //     {
            //         AssignCardToEmptyZone();
            //     }

            //     if (HeldCard != null && !wasClicked)
            //     {
            //         SwapCards();
            //     }
            // }
        }

        private void AssignCardToEmptyZone()
        {
            HeldCard = PlayerHand.Instance.SelectedCard;

            PlayerHand.Instance.SelectedCard = null;
            PlayerHand.Instance.HighlightedCard = null;
            PlayerHand.Instance.RemoveCardFromLocation(HeldCard);
            AddCardToZone(HeldCard);

            OnCardAssigned?.Invoke(HeldCard.CardData, thisCardZoneSI);
            wasClicked = true;
        }

        private void SwapCards()
        {
            // mark new and old cards
            Card oldCard = HeldCard;
            Card newCard = PlayerHand.Instance.SelectedCard;

            // remove newCard from PlayerHand and assign it to the zone
            PlayerHand.Instance.SelectedCard = null;
            PlayerHand.Instance.HighlightedCard = null;
            PlayerHand.Instance.RemoveCardFromLocation(newCard);
            AddCardToZone(newCard);

            // remove oldCard from the CardZone
            oldCard.transform.parent = null;

            // add oldCard back to PlayerHand
            PlayerHand.Instance.SetSelectedCard(oldCard);
            PlayerHand.Instance.SetHighlightedCard(oldCard);
            PlayerHand.Instance.SetCardPositionsInHand();

            // mark the newCard as my assigned one and Invoke list update 
            HeldCard = newCard;
            OnCardAssigned?.Invoke(HeldCard.CardData, thisCardZoneSI);
            wasClicked = true;
        }
    }
}
