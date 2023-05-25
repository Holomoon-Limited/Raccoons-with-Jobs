using UnityEngine;

namespace Holo.Cards
{
    [CreateAssetMenu(fileName = "Lolipop Effect", menuName = "Cards/Effects/New Lolipop Effect", order = 0)]
    public class LolipopEffect : Effect
    {
        public override void Use(Card callingCard, Board board)
        {
            if (callingCard.Negated) return;
            if (board.PlayerCards.Contains(callingCard))
            {
                if (board.EnemyZones[callingCard.Position].HasCard)
                {
                    board.EnemyZones[callingCard.Position].HeldCard.Negated = true;
                }
            }
            else
            {
                if (board.PlayerZones[callingCard.Position].HasCard)
                {
                    board.PlayerZones[callingCard.Position].HeldCard.Negated = true;
                }
            }
        }
    }
}