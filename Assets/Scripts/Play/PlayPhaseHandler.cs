using System.Collections.Generic;
using Holo.Cards;
using Unity.VisualScripting;
using UnityEngine;

namespace Holo.Racc.Play
{
    [CreateAssetMenu(fileName = "Play Phase Handler", menuName = "Play/New Play Phase Handler", order = 0)]
    public class PlayPhaseHandler : ScriptableObject
    {
        [Header("Lists of Data")] [SerializeField]
        private List<CardData> cardsInPlay = new List<CardData>();

        private void OnEnable()
        {
            this.hideFlags = HideFlags.DontUnloadUnusedAsset;

            ClearCardsInPlay();
        }
        
        private void OnDisable()
        {
            
        }

        public void EndPlayPhase()
        {
            Debug.Log("Play Phase over");
            // add stuff to return unused cards to deck
        }
        
        public void ClearCardsInPlay()
        {
            cardsInPlay.Clear();
        }
    }
}
