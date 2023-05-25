using UnityEngine;

namespace Holo.Cards
{
    [CreateAssetMenu(fileName = "HR Effect", menuName = "Cards/Effects/New HR Effect", order = 0)]
    public class HREffect : Effect
    {
        public override void Use(Card callingCard, Board board)
        {
            if (callingCard.Negated) return;
            int index = callingCard.Position - 1;
            if (board.PlayerCards.Contains(callingCard))
            {
                if (index >= 0 && board.PlayerZones[index].HasCard)
                {
                    board.PlayerZones[index].HeldCard.SetPower(board.PlayerZones[index].HeldCard.Power + 2);
                }
                index = callingCard.Position + 1;
                if (index > board.PlayerZones.Count - 1) return;
                if (board.PlayerZones[index].HasCard)
                {
                    board.PlayerZones[index].HeldCard.SetPower(board.PlayerZones[index].HeldCard.Power + 2);
                }
            }
            else
            {
                if (index >= 0 && board.EnemyZones[index].HasCard)
                {
                    board.EnemyZones[index].HeldCard.SetPower(board.EnemyZones[index].HeldCard.Power + 2);
                }
                index = callingCard.Position + 1;
                if (index > board.EnemyZones.Count - 1) return;
                if (board.EnemyZones[index].HasCard)
                {
                    board.EnemyZones[index].HeldCard.SetPower(board.EnemyZones[index].HeldCard.Power + 2);
                }
            }
        }
    }
}
