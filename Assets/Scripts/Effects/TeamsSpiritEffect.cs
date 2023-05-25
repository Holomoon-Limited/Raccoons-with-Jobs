using UnityEngine;

namespace Holo.Cards
{
    [CreateAssetMenu(fileName = "Teams Spirit Effect", menuName = "Cards/Effects/New Teams Spirit Effect", order = 0)]
    public class TeamsSpiritEffect : Effect
    {
        public override void Use(Card callingCard, Board board)
        {
            if (callingCard.Negated) return;
            int targetPosition = callingCard.Position + 1;

            if (board.PlayerCards.Contains(callingCard))
            {
                if (targetPosition > board.PlayerZones.Count - 1 || targetPosition < 0) return;
                CardZone zone = board.PlayerZones[targetPosition];
                if (zone.HasCard)
                {
                    zone.HeldCard.SetBasePower(zone.HeldCard.CardData.Power + callingCard.Power);
                }
            }
            else
            {
                if (targetPosition > board.EnemyZones.Count - 1 || targetPosition < 0) return;
                CardZone zone = board.EnemyZones[targetPosition];
                if (zone.HasCard)
                {
                    zone.HeldCard.SetBasePower(zone.HeldCard.CardData.Power + callingCard.Power);
                }
            }
        }
    }
}
