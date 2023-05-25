using System.Collections;
using Holo.Cards;
using Holo.Racc.Game;
using UnityEngine;

namespace Holo.Racc.Battle
{
    /// <summary>
    /// Runs the battle attack phase
    /// </summary>
    public class Attacker : MonoBehaviour
    {
        [SerializeField] private Transform playerGraveyard;
        [SerializeField] private Transform enemyGraveyard;

        [SerializeField] BattleHandler battleHandler;
        [SerializeField] PhaseHandler phaseHandler;
        [SerializeField] ScoreManager scoreManager;

        [SerializeField][Min(0f)] private float timeBetweenAttacks = 0.5f;

        private void OnEnable()
        {
            battleHandler.OnAttackPhase += RunAttackPhase;
        }

        private void OnDisable()
        {
            battleHandler.OnAttackPhase -= RunAttackPhase;
        }

        private void RunAttackPhase()
        {
            StartCoroutine(Co_RunAttackPhase());
        }

        private IEnumerator Co_RunAttackPhase()
        {
            //Check if the frickin cards are still on the board
            if (PlayerHasCards() && EnemyHasCards())
            {
                yield return new WaitForSeconds(timeBetweenAttacks);

                //Make the raccoons fight 
                for (int i = 0; i < Board.Instance.PlayerZones.Count; i++)
                {
                    Card playerCard = Board.Instance.PlayerZones[i].HeldCard;
                    Card enemyCard = Board.Instance.EnemyZones[i].HeldCard;
                    if (playerCard == null || enemyCard == null) continue;

                    Debug.Log($"Run Attack effects for {playerCard.CardData.CardName} and {enemyCard.CardData.CardName}");
                    playerCard.Attack();
                    enemyCard.Attack();

                    //Let the attack play 
                    yield return new WaitForSeconds(1f);
                    if (playerCard.Power > enemyCard.Power)
                    {
                        DestroyEnemyCard(Board.Instance.EnemyZones[i]);
                    }
                    else if (playerCard.Power < enemyCard.Power)
                    {
                        DestroyPlayerCard(Board.Instance.PlayerZones[i]);
                    }
                    else
                    {
                        DestroyEnemyCard(Board.Instance.EnemyZones[i]);
                        DestroyPlayerCard(Board.Instance.PlayerZones[i]);
                    }
                    yield return new WaitForSeconds(timeBetweenAttacks);
                }
                battleHandler.ShuffleDown();
            }
            else
            {
                //Then some gross looking win/loss logic 
                //Draw 
                if (!PlayerHasCards() && !EnemyHasCards())
                {
                    Debug.Log("Draw");
                }
                //Player wins
                else if (PlayerHasCards() && !EnemyHasCards())
                {
                    scoreManager.IncreasePlayerScore();
                }
                //Enemy Wins
                else if (!PlayerHasCards() && EnemyHasCards())
                {
                    scoreManager.IncreaseEnemyScore();
                }
                foreach (CardZone zone in Board.Instance.PlayerZones)
                {
                    if (zone.HasCard)
                    { PlayerHand.Instance.AddCardToHand(zone.HeldCard); }
                }
                Board.Instance.ResetBoard();
                StartCoroutine(phaseHandler.Co_EndBattlePhase());
            }
        }

        private void DestroyEnemyCard(CardZone zone)
        {
            Card card = zone.HeldCard;
            zone.RemoveCardFromZone();
            card.MoveToPoint(enemyGraveyard.position, enemyGraveyard.rotation);
            Board.Instance.EnemyDestroyedCards.Add(card);
            Board.Instance.DestroyedEnemyCardsNumber++;
            if (card.HasEffect && card.Effect.Timing == EffectTiming.OnCardDestroyed)
            {
                card.Effect.Use(card, Board.Instance);
            }
            EffectHandler.Instance.UnRegisterEffect(card);
            EffectHandler.Instance.ApplyContinuousEffects();
        }

        private void DestroyPlayerCard(CardZone zone)
        {
            Card card = zone.HeldCard;
            zone.RemoveCardFromZone();
            card.MoveToPoint(playerGraveyard.position, enemyGraveyard.rotation);
            Board.Instance.PlayerDestroyedCards.Add(card);
            Board.Instance.DestroyedPlayerCardsNumber++;
            if (card.HasEffect && card.Effect.Timing == EffectTiming.OnCardDestroyed)
            {
                card.Effect.Use(card, Board.Instance);
            }
            EffectHandler.Instance.UnRegisterEffect(card);
            EffectHandler.Instance.ApplyContinuousEffects();
        }

        private bool PlayerHasCards()
        {
            foreach (CardZone zone in Board.Instance.PlayerZones)
            {
                if (zone.HasCard) return true;
            }
            return false;
        }

        private bool EnemyHasCards()
        {
            foreach (CardZone zone in Board.Instance.EnemyZones)
            {
                if (zone.HasCard) return true;
            }
            return false;
        }
    }
}
