using System.Collections.Generic;
using Holo.Cards;
using UnityEngine;

namespace Holo.Racc.Play
{
    [CreateAssetMenu(fileName = "Play Phase Handler", menuName = "Play/New Play Phase Handler", order = 0)]
    public class PlayPhaseHandler : ScriptableObject
    {
        [Header("Lists of Data")]
        [SerializeField] private List<CardData> cardsInPlay;

        private int cardZones = 6;
        private bool canEndPlayPhase => (cardZones == cardsInPlay.Count);

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

        public void UpdateCardsInPlay(CardData cardData)
        {
            // adds new card. will need more functionality for swapping 
            cardsInPlay.Add(cardData);
        }
        
        // hook up to a button 
        public void EndPlayPhase()
        {
            Debug.Log("Play Phase over");
            // add stuff to return unused cards to deck
        }

        // will need to be called at end of battle phase or something
        public void ClearCardsInPlay()
        {
            cardsInPlay.Clear();
        }
    }
}
