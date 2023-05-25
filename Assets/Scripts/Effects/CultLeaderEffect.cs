using UnityEngine;

namespace Holo.Cards
{
    [CreateAssetMenu(fileName = "Cult Leader Effect", menuName = "Cards/Effects/New Cult Leader Effect", order = 0)]
    public class CultLeaderEffect : Effect
    {
        public override void Use(Card callingCard, Board board)
        {
            if (Board.Instance.PlayerCards.Contains(callingCard))
            {
                foreach (Card card in board.PlayerCards)
                {
                    if (card == callingCard) continue;
                    card.SetPower(card.Power + 1);
                }
            }
            else
            {
                foreach (Card card in board.EnemyCards)
                {
                    if (card == callingCard) continue;
                    card.SetPower(card.Power + 1);
                }
            }
        }
    }
}
