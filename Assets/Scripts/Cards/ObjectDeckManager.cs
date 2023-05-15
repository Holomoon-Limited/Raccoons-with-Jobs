using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Holo.Cards
{
    public class ObjectDeckManager : MonoBehaviour
    {
        [SerializeField] private DeckManager deckManager;
        void Start()
        {
            deckManager.ResetPoolOfCurrentCards();
            CardData myCard = deckManager.DrawCard();
            deckManager.AddCardToPool(myCard);
            deckManager.AddCardToPool(myCard);
        }
    }
}
