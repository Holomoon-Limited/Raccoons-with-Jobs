using UnityEngine;

namespace Holo.Cards
{
    [CreateAssetMenu(fileName = "Comeback Effect", menuName = "Cards/Effects/New Comeback Effect", order = 0)]
    public class ComebackEffect : Effect
    {
        public override void Use(Card callingCard, Board board)
        {
            if (callingCard.Negated) return;
            if (board.PlayerCards.Contains(callingCard))
            {
                foreach (CardZone zone in board.PlayerZones)
                {
                    if (zone.HasCard && zone.HeldCard != callingCard) return;
                }
                callingCard.SetPower(callingCard.Power + 5);
            }
            else
            {
                foreach (CardZone zone in board.EnemyZones)
                {
                    if (zone.HasCard && zone.HeldCard != callingCard) return;
                }
                callingCard.SetPower(callingCard.Power + 5);
            }
        }
    }
}