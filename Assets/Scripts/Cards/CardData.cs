using UnityEngine;

namespace Holo.Cards
{
    [CreateAssetMenu(fileName = "Card", menuName = "Cards/New Card", order = 0)]
    public class CardData : ScriptableObject
    {
        public string CardName;
        public int Power;
        public string CardDescription;
        public Sprite Image;
        
        // public Effect Effect;
    }
}
