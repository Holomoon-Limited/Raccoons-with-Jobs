using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Holo.Cards;
using Holo.Input;
using UnityEngine;

namespace Holo.Racc.Play
{
    public class PlayCardZone : CardZone, IRaycastable
    {
        [SerializeField] private Card assignedCard;

        [Header("Prefab References")] 
        [SerializeField] private InputManager input;

        private int thisCardZoneSI => this.gameObject.transform.GetSiblingIndex();
        private bool isHoveredOver;
        public bool wasClicked;
        
        // PlayPhase subbed to manage assigned cards
        public static event Action<CardData, int> OnCardAssigned  = (cardData, position) => { };
        
        private void OnEnable()
        {
            input.OnSubmitPressed += AssignCardCheck;
        }

        private void OnDisable()
        {
            input.OnSubmitPressed -= AssignCardCheck;
        }

        public void OnHover()
        {
            isHoveredOver = true;
        }

        public void OnEndHover()
        {
            isHoveredOver = false;
            wasClicked = false;
        }

        private void AssignCardCheck()
        {
            if (isHoveredOver)
            {
                // return if no selected card 
                if (PlayerHand.Instance.SelectedCard == null) return;

                if (assignedCard == null && !wasClicked)
                {
                    AssignCardToEmptyZone();
                }

                if (assignedCard != null && !wasClicked)
                {
                    SwapCards();
                }
            }
        }

        private void AssignCardToEmptyZone()
        {
            assignedCard = PlayerHand.Instance.SelectedCard;

            PlayerHand.Instance.SelectedCard = null;
            PlayerHand.Instance.HighlightedCard = null;
            PlayerHand.Instance.RemoveCardFromLocation(assignedCard);
            AddCardToZone(assignedCard);

            OnCardAssigned?.Invoke(assignedCard.CardData, thisCardZoneSI);
            wasClicked = true;
        }

        private void SwapCards()
        {
            // mark new and old cards
            Card oldCard = assignedCard;
            Card newCard = PlayerHand.Instance.SelectedCard;
                    
            // remove newCard from PlayerHand and assign it to the zone
            PlayerHand.Instance.SelectedCard = null;
            PlayerHand.Instance.HighlightedCard = null;
            PlayerHand.Instance.RemoveCardFromLocation(newCard);
            AddCardToZone(newCard);
                    
            // remove oldCard from the CardZone
            RemoveCardFromZone(oldCard);

            // add oldCard back to PlayerHand
            PlayerHand.Instance.SetSelectedCard(oldCard);
            PlayerHand.Instance.SetHighlightedCard(oldCard);
            PlayerHand.Instance.SetCardPositionsInHand();

            // mark the newCard as my assigned one and Invoke list update 
            assignedCard = newCard;
            OnCardAssigned?.Invoke(assignedCard.CardData, thisCardZoneSI);
            wasClicked = true;
        }
    }
}
