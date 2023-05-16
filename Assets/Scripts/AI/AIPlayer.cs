using System.Collections;
using System.Collections.Generic;
using Holo.Cards;
using Holo.Racc.Draft;
using UnityEngine;

namespace Holo.Racc.AI
{
    public class AIPlayer : MonoBehaviour
    {
        [SerializeField] private DraftHandler draftHandler;

        [SerializeField] private float timeBetweenSelection = 0.3f;
        [SerializeField] private Transform aiHandPoint;

        private List<CardScriptableObject> heldCards = new List<CardScriptableObject>();

        private void OnEnable()
        {
            draftHandler.OnAIPick += SelectDraftCards;
        }

        private void OnDisable()
        {
            draftHandler.OnAIPick -= SelectDraftCards;
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
                Dealer.Instance.RemoveCardFromLocation(Dealer.Instance.HeldCards[index]);
                //Add card data to this card data 
                Destroy(card.gameObject, 1f);
                yield return new WaitForSeconds(timeBetweenSelection);
            }
            draftHandler.ProgressPhase();
        }
    }
}
