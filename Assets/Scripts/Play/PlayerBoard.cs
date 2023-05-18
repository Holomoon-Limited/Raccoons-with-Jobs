using System.Collections;
using System.Collections.Generic;
using Holo.Cards;
using UnityEngine;

namespace Holo.Racc.Play
{
    public class PlayerBoard : MonoBehaviour
    {
        //Reference to game
        private List<CardZone> cardZones = new List<CardZone>();
        public CardZone HighlightedZone { get; private set; }

        [SerializeField] private PlayCardZone playCardZonePrefab;

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
            //Register to play phase start
        }

        private void OnDisable()
        {
            //Unregister to play phase 
        }

        private void Start()
        {
            SpawnPlayCardZones(3);
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

        public IEnumerator<CardData> CardsInPlay()
        {
            foreach (CardZone zone in cardZones)
            {
                yield return zone.HeldCard.CardData;
            }
        }

        //Update game with cards in play 
    }

}