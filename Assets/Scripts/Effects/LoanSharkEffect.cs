using UnityEngine;
using System.Collections.Generic;

namespace Holo.Cards
{
    [CreateAssetMenu(fileName = "Loan Shark Effect", menuName = "Cards/Effects/New Loan Shark Effect", order = 0)]
    public class LoanSharkEffect : Effect
    {
        public override void Use(Card callingCard, Board board)
        {
            if (callingCard.Negated) return;
            if (board.PlayerCards.Contains(callingCard))
            {
                board.DestroyPlayerCard(board.PlayerZones[callingCard.Position]);
                CardZone zoneToDestroy = null;
                foreach (CardZone zone in board.EnemyZones)
                {
                    if (zone.HasCard == false) continue;
                    if (zoneToDestroy == null || (zoneToDestroy.HasCard && zone.HeldCard.Power < zoneToDestroy.HeldCard.Power))
                    {
                        zoneToDestroy = zone;
                    }
                }
                if (zoneToDestroy != null)
                {
                    board.DestroyEnemyCard(zoneToDestroy, false);
                }
            }
            else
            {
                board.DestroyEnemyCard(board.EnemyZones[callingCard.Position]);
                CardZone zoneToDestroy = null;
                foreach (CardZone zone in board.PlayerZones)
                {
                    if (zone.HasCard == false) continue;
                    if (zoneToDestroy == null || (zoneToDestroy.HasCard && zone.HeldCard.Power < zoneToDestroy.HeldCard.Power))
                    {
                        zoneToDestroy = zone;
                    }
                }
                if (zoneToDestroy != null)
                {
                    board.DestroyEnemyCard(zoneToDestroy, false);
                }
            }
        }
    }
}