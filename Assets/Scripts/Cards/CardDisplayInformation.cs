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
        [SerializeField] private TextMeshPro cardNameTMP;
        [SerializeField] private TextMeshPro powerTMP;
        [SerializeField] private TextMeshPro cardDescriptionTMP;
        [SerializeField] private Image imageImage;
        
        void Start()
        {
            cardName = cardData.CardName;
            power = cardData.Power;
            cardDescription = cardData.CardDescription;
            image = cardData.Image;
            id = cardData.ID;
            // effect = cardData.Effect;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
