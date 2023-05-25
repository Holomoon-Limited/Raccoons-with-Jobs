using UnityEngine;

namespace Holo.Cards
{
    [CreateAssetMenu(fileName = "Dirty Fighter Effect", menuName = "Cards/Effects/New Dirty Fighter Effect", order = 0)]
    public class DirtyFighterEffect : Effect
    {
        public override void Use(Card callingCard, Board board)
        {
            if (callingCard.Negated) return;
            if (board.PlayerCards.Contains(callingCard))
            {
                board.DestroyEnemyCard(board.EnemyZones[callingCard.Position]);
            }
            else
            {
                board.DestroyPlayerCard(board.PlayerZones[callingCard.Position]);
            }
        }
    }
}
