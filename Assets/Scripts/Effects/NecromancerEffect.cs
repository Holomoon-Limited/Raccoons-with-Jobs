using System.Collections.Generic;
using UnityEngine;

namespace Holo.Cards
{
    [CreateAssetMenu(fileName = "Necromancer Effect", menuName = "Cards/Effects/New Necromancer Effect", order = 1)]
    public class NecromancerEffect : Effect
    {
        public override void Use(Card callingCard, Board board)
        {
            //board.SpawnLastDestroyedCard(callingCard);
        }
    }
}
