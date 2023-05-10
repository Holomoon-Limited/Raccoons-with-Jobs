using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Holo.Cards
{
    /// <summary>
    /// Monobehaviour responsible for displaying a card's data
    /// </summary>
    public class CardDisplayInformation : MonoBehaviour
    {
        // required ScriptableObject
        [SerializeField] private CardScriptableObject cardData;

        // components to update with data
        [SerializeField] private TextMeshProUGUI cardNameTMP;
        [SerializeField] private TextMeshProUGUI powerTMP;
        [SerializeField] private TextMeshProUGUI cardDescriptionTMP;
        [SerializeField] private Image imageImage;

        void Start()
        {
            // defaults to test card as set in prefab 
            DisplayCard(cardData);
        }

        public void DisplayCard(CardScriptableObject newCardData)
        {
            // overwrites default with new value
            cardData = newCardData;
            
            // assign data to components 
            cardNameTMP.text = cardData.CardName;
            powerTMP.text = cardData.Power.ToString();
            cardDescriptionTMP.text = cardData.CardDescription;
            imageImage.sprite = cardData.Image;
        }
    }
}
