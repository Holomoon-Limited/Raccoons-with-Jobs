using System.Collections.Generic;
using Holo.Cards;
using Unity.VisualScripting;
using UnityEngine;

namespace Holo.Racc.Play
{
    [CreateAssetMenu(fileName = "Play Phase Handler", menuName = "Play/New Play Phase Handler", order = 0)]
    public class PlayPhaseHandler : ScriptableObject
    {
        private int cardZones = 6;

        [Header("Lists of Data")] [SerializeField]
        private List<CardData> cardsInPlay = new List<CardData>();

        public bool CanEndPlayPhase => CanEndPlayPhaseCheck();

        private void OnEnable()
        {
            this.hideFlags = HideFlags.DontUnloadUnusedAsset;
            PlayCardZone.OnCardAssigned += UpdateCardsInPlay;
            
            ClearCardsInPlay();
        }
        
        private void OnDisable()
        {
            PlayCardZone.OnCardAssigned -= UpdateCardsInPlay;
        }

        public void UpdateCardsInPlay(CardData cardData, int position)
        {
            cardsInPlay[position] = cardData;
        }

        // end Play Phase if cardsInPlay is fully populated 
        private bool CanEndPlayPhaseCheck()
        {
            for (int i = 0; i < cardsInPlay.Count; i++)
            {
                if (cardsInPlay[i] == null) return false;
            }

            return true;
        }
        
        public void EndPlayPhase()
        {
            Debug.Log("Play Phase over");
            // add stuff to return unused cards to deck
        }
        
        public void ClearCardsInPlay()
        {
            cardsInPlay.Clear();
            // create empty list equal to number of cardZones
            for (int i = 0; i < cardZones; i++)
            {
                cardsInPlay.Add(null);
            }
        }
    }
}
