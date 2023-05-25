using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Holo.Cards
{
    public class InstantiateCards : MonoBehaviour
    {
        [SerializeField] private Card baseCard;
        [SerializeField] List<CardData> playersCardData;
        [SerializeField] private PlayerHand cardLocation;

        private void OnGUI()
        {
            if (GUILayout.Button("Instantiate Cards"))
            {
                // Debug.Log($"{playersCardData.Count}");
                // PlayerHand.Instance.InstantiatePlayerCards(playersCardData);
                //
                // InstantiateAllCards(playersCardData);
            }
        }

        private void InstantiateAllCards(List<CardData> cardScriptableObjects)
        {
            for (int i = 0; i < cardScriptableObjects.Count; i++)
            {
                Card newCard = Instantiate(baseCard, new Vector3(0, 0, 0), Quaternion.identity);
                newCard.name = cardScriptableObjects[i].CardName;
                newCard.SetActiveLocation(cardLocation);
                newCard.SetCardData(cardScriptableObjects[i]);
            }
        }
    }
}
