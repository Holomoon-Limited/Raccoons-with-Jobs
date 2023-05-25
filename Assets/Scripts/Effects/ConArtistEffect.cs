using UnityEngine;

namespace Holo.Cards
{
    [CreateAssetMenu(fileName = "Con Artist Effect", menuName = "Cards/Effects/New Con Artist Effect", order = 0)]
    public class ConArtistEffect : Effect
    {
        public override void Use(Card callingCard, Board board)
        {
            if (callingCard.Negated) return;
            if (board.PlayerCards.Contains(callingCard))
            {
                Card card = board.EnemyZones[callingCard.Position].HeldCard;
                if (card == null) return;
                card.SetPower(card.Power / 2);
            }
            else
            {
                Card card = board.PlayerZones[callingCard.Position].HeldCard;
                if (card == null) return;
                card.SetPower(card.Power / 2);
            }
        }
    }
}
