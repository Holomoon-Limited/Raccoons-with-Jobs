using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Holo.Cards
{
    public class InstantiateCards : MonoBehaviour
    {
        [SerializeField] private Card baseCard;
        [SerializeField] List<CardScriptableObject> cardScriptableObjects;
        [SerializeField] private CardLocation cardLocation;

        private void OnGUI()
        {
            if (GUILayout.Button("Instantiate Cards"))
            {
                InstantiateAllCards(cardScriptableObjects);
            }
        }

        private void InstantiateAllCards(List<CardScriptableObject> cardScriptableObjects)
        {
            for (int i = 0; i < cardScriptableObjects.Count; i++)
            {
                Card newCard = Instantiate(baseCard, new Vector3(0, 0, 0), Quaternion.identity);
                newCard.name = cardScriptableObjects[i].CardName;
                newCard.SetActiveLocation(cardLocation);
                CardDisplayInformation cardDisplayInformation = newCard.GetComponent<CardDisplayInformation>();
                cardDisplayInformation.DisplayCard(cardScriptableObjects[i]);
            }
        }
    }
}
