using UnityEngine;

namespace Holo.Cards
{
    [CreateAssetMenu(fileName = "Undertaker Effect", menuName = "Cards/Effects/New Undertaker Effect", order = 0)]
    public class UndertakerEffect : Effect
    {
        public override void Use(Card callingCard, Board board)
        {
            if (board.PlayerCards.Contains(callingCard))
            {
                callingCard.SetPower(callingCard.Power + board.PlayerDestroyedCards.Count);
            }
            else
            {
                callingCard.SetPower(callingCard.Power + board.EnemyDestroyedCards.Count);
            }
        }
    }
}
