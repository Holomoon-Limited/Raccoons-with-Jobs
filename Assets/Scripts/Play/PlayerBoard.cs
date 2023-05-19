using System.Collections;
using System.Collections.Generic;
using Holo.Cards;
using Holo.Racc.Game;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Holo.Racc.Play
{
    public class PlayerBoard : MonoBehaviour
    {
        [Header("Asset References")]
        [SerializeField] private PhaseHandler phaseHandler;
        [SerializeField] private CardsInPlayContainer cardsInPlayContainer;
        
        [Header("Prefab References")]
        [SerializeField] private PlayCardZone playCardZonePrefab;

        //Reference to game
        private List<CardZone> cardZones = new List<CardZone>();
        private int cardZoneCount = 3;
        
        public CardZone HighlightedZone { get; private set; }

        public bool CanEndPlayPhase
        {
            get
            {
                foreach (CardZone zone in cardZones)
                {
                    if (zone.HasCard == false)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private void OnEnable()
        {
            phaseHandler.OnPlayEnd += UpdatePlayerCards;
        }

        private void OnDisable()
        {
            phaseHandler.OnPlayEnd -= UpdatePlayerCards;
        }

        private void Start()
        {
            cardZoneCount = phaseHandler.PlayCardZoneCount;
            SpawnPlayCardZones(cardZoneCount);
        }

        /// <summary>
        /// Spawns zones to the board
        /// </summary>
        /// <param name="zones">The number of zones to spawn (from Game)</param>
        public void SpawnPlayCardZones(int zones)
        {
            Vector3 position = Vector3.zero;
            if (zones % 2 == 0)
            {
                position.x = (-0.75f * (zones - 1));
            }
            else
            {
                position.x = (float)(-2f * (int)(zones / 2));
            }

            for (int i = 0; i < zones; i++)
            {
                PlayCardZone zone = Instantiate(playCardZonePrefab, this.transform);
                zone.transform.localPosition = position;
                this.cardZones.Add(zone);
                zone.SetPlayerBoard(this);
                position.x += 2f;
            }
        }

        public void SetHighlightedZone(CardZone zone)
        {
            if (HighlightedZone != null && HighlightedZone != zone)
            {
                HighlightedZone = null;
            }
            if (zone == null) return;
            this.HighlightedZone = zone;
        }
        
        private void UpdatePlayerCards()
        {
            List<CardData> playerCards = new List<CardData>();
            
            foreach (CardZone zone in cardZones)
            {
                playerCards.Add(zone.HeldCard.CardData);
            }
            
            cardsInPlayContainer.UpdateCardsInPlay(true, playerCards);
        }
    }

}