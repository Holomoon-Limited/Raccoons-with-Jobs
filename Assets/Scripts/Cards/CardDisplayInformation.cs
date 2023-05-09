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
        // data from ScriptableObject
        [SerializeField] private CardScriptableObject cardData;
        private string cardName;
        private int power;
        private string cardDescription;
        private Sprite image;
        private int id;
        // private Effect effect;
        
        // components to update with data
        [SerializeField] private TextMeshProUGUI cardNameTMP;
        [SerializeField] private TextMeshProUGUI powerTMP;
        [SerializeField] private TextMeshProUGUI cardDescriptionTMP;
        [SerializeField] private Image imageImage;
        
        void Start()
        {
            // get data from SO
            cardName = cardData.CardName;
            power = cardData.Power;
            cardDescription = cardData.CardDescription;
            image = cardData.Image;
            id = cardData.ID;
            // effect = cardData.Effect;

            // assign data to components 
            cardNameTMP.text = cardName;
            powerTMP.text = power.ToString();
            cardDescriptionTMP.text = cardDescription;
            imageImage.sprite = image;
        }
    }
}
