using System.Collections.Generic;
using Holo.Cards;
using Holo.Input;
using Holo.Racc.Game;
using UnityEngine;

namespace Holo.Racc.Play
{
    public class PlayerBoard : MonoBehaviour, IGamepadLocation
    {
        [Header("Asset References")]
        [SerializeField] private PhaseHandler phaseHandler;
        [SerializeField] private PlayHandler playHandler;
        [SerializeField] private CardsInPlayContainer cardsInPlayContainer;

        [Header("Prefab References")]
        [SerializeField] private PlayCardZone playCardZonePrefab;

        [SerializeField] InputManager input;

        //Reference to game
        private List<PlayCardZone> cardZones = new List<PlayCardZone>();
        private int cardZoneCount = 3;

        public PlayCardZone HighlightedZone { get; private set; }

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
            PlayerHand.Instance.OnCardSelected += UpdateGamepadControls;
            input.OnStartBattle += EndPlayPhase;
        }

        private void OnDisable()
        {
            phaseHandler.OnPlayEnd -= UpdatePlayerCards;
            PlayerHand.Instance.OnCardSelected -= UpdateGamepadControls;
            input.OnStartBattle -= EndPlayPhase;
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
                position.x = (-1f * (zones - 1));
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
                zone.Position = i;
                position.x += 2f;
            }
        }

        public void SetHighlightedZone(PlayCardZone zone)
        {
            if (HighlightedZone != null && HighlightedZone != zone)
            {
                HighlightedZone.EndZoneHighlight();
                HighlightedZone = null;
            }
            if (zone == null) return;
            this.HighlightedZone = zone;
            this.HighlightedZone.HighlightZone();
        }

        private void EndPlayPhase()
        {
            if (CanEndPlayPhase)
            {
                playHandler.EndPlayPhase();
            }
        }

        private void UpdatePlayerCards()
        {
            List<CardData> playerCards = new List<CardData>();

            foreach (CardZone zone in cardZones)
            {
                CardData card = zone.HeldCard.CardData;
                playerCards.Add(card);
            }

            cardsInPlayContainer.UpdateCardsInPlay(true, playerCards);
        }

        private void UpdateGamepadControls()
        {
            GamepadControls.Instance.activeLocation = this;
            SetHighlightedZone(cardZones[0]);
        }

        public void OnControllerActivated()
        {
            SetHighlightedZone(cardZones[0]);
        }

        public void OnNavigate(float value)
        {
            if (value > 0)
            {
                int index = HighlightedZone.Position + 1;
                if (index > cardZones.Count - 1) index = 0;
                SetHighlightedZone(cardZones[index]);
            }
            else if (value < 0)
            {
                int index = HighlightedZone.Position - 1;
                if (index < 0) index = cardZones.Count - 1;
                SetHighlightedZone(cardZones[index]);
            }
        }

        public void OnSubmit()
        {
            if (HighlightedZone != null)
            {
                HighlightedZone.AddCardToZone();
            }
            OnCancel();
        }

        public void OnCancel()
        {
            SetHighlightedZone(null);
            PlayerHand.Instance.OnCancel();
        }
    }

}