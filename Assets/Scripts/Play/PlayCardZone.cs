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
        private PlayCardZone thisZone;
        
        [SerializeField] private Card assignedCard;
        private bool hasCard => (HeldCard != null);

        [Header("Prefab References")] 
        [SerializeField] private InputManager input;

        private bool isHoveredOver;
        
        // PlayPhase subbed to manage assigned cards
        public static event Action<CardData> OnCardAssigned  = (cardData) => { };
        
        void Start()
        {
            thisZone = this;
        }
        
        private void OnEnable()
        {
            input.OnSubmitPressed += AssignCard;
        }

        private void OnDisable()
        {
            input.OnSubmitPressed -= AssignCard;
        }

        public void OnHover()
        {
            isHoveredOver = true;
        }

        public void OnEndHover()
        {
            isHoveredOver = false;
        }

        private void AssignCard()
        {
            if (isHoveredOver)
            {
                Debug.Log($"{thisZone} has been clicked on!");
                assignedCard = PlayerHand.Instance.SelectedCard;
                
                OnCardAssigned?.Invoke(assignedCard.CardData);
            }
        }
    }
}
