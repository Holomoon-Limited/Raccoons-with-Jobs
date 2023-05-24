using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Holo.Cards;
using Holo.Racc.Draft;
using Holo.Racc.Game;
using UnityEngine;

namespace Holo.Racc.AI
{
    public class AIPlayer : MonoBehaviour
    {
        public static AIPlayer Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                DontDestroyOnLoad(this.gameObject);
                Instance = this;
            }
        }

        [Header("Asset References")]
        [SerializeField] private DraftHandler draftHandler;
        [SerializeField] private PhaseHandler phaseHandler;
        [SerializeField] private CardsInPlayContainer cardsInPlay;
        [SerializeField] private DeckManager deckManager;

        [Header("Design Properties")]
        [SerializeField] private float timeBetweenSelection = 0.3f;
        [SerializeField] private Transform aiHandPoint;

        private List<CardData> heldCards = new List<CardData>();

        private void OnEnable()
        {
            draftHandler.OnAIPick += SelectDraftCards;

            phaseHandler.OnGameEnd += ResetCards;
        }

        private void OnDisable()
        {
            draftHandler.OnAIPick -= SelectDraftCards;
            
            phaseHandler.OnGameEnd -= ResetCards;
        }

        public void SelectDraftCards(int picks)
        {
            StartCoroutine(Co_SelectDraftCards(picks));
        }

        private IEnumerator Co_SelectDraftCards(int picks)
        {
            yield return new WaitForSeconds(timeBetweenSelection);
            for (int i = 0; i < picks; i++)
            {
                Dealer.Instance.SetHighlightedCard(null);
                int index = Random.Range(0, Dealer.Instance.HeldCards.Count);
                Card card = Dealer.Instance.HeldCards[index];
                card.GetComponent<Collider>().enabled = false;
                card.MoveToPoint(aiHandPoint.position, aiHandPoint.rotation);
                heldCards.Add(card.CardData);
                UpdateEnemyCards();
                Dealer.Instance.RemoveCardFromLocation(Dealer.Instance.HeldCards[index]);
                Destroy(card.gameObject, 1f);
                yield return new WaitForSeconds(timeBetweenSelection);
            }
            draftHandler.ProgressPhase();
        }

        private void UpdateEnemyCards()
        {
            while (heldCards.Count > phaseHandler.PlayCardZoneCount)
            {
                deckManager.AddCardToPool(heldCards[heldCards.Count - 1]);

                heldCards.RemoveAt(heldCards.Count - 1);
            }
            // shuffles the cards
            heldCards = heldCards.OrderBy(x => Random.value).ToList();
            cardsInPlay.UpdateCardsInPlay(false, heldCards);
        }

        // clears AI hand and returns cards to deck 
        private void ResetCards()
        {
            foreach (CardData card in heldCards)
            {
                deckManager.AddCardToPool(card);
            }
            
            heldCards.Clear();
        }
    }
}
