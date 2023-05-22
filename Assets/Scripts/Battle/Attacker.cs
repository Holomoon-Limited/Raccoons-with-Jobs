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
        [SerializeField] BattleHandler battleHandler;
        [SerializeField] PhaseHandler phaseHandler;
        [SerializeField] ScoreManager scoreManager;

        [SerializeField][Min(0f)] private float timeBetweenAttacks = 0.5f;

        private BattleSpawner battleSpawner;

        private void Awake()
        {
            //Needs the spawned to get the frickin zone lists 
            battleSpawner = this.GetComponent<BattleSpawner>();
        }

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
            StopAllCoroutines();
            StartCoroutine(Co_RunAttackPhase());
        }

        private IEnumerator Co_RunAttackPhase()
        {
            //Check if the frickin cards are still on the board
            if (PlayerHasCards() && EnemyHasCards())
            {
                yield return new WaitForSeconds(timeBetweenAttacks);

                //Make the raccoons fight 
                for (int i = 0; i < battleSpawner.Zones; i++)
                {
                    Card playerCard = battleSpawner.PlayerCardZones[i].HeldCard;
                    Card enemyCard = battleSpawner.EnemyCardZones[i].HeldCard;
                    if (playerCard == null || enemyCard == null) continue;
                    playerCard.Attack();
                    enemyCard.Attack();
                    if (playerCard.Power > enemyCard.Power)
                    {
                        battleSpawner.EnemyCardZones[i].DespawnCard();
                    }
                    else if (playerCard.Power < enemyCard.Power)
                    {
                        PlayerHand.Instance.AddCardToHand(battleSpawner.PlayerCardZones[i].HeldCard);
                        battleSpawner.PlayerCardZones[i].DespawnCard();
                    }
                    else
                    {
                        PlayerHand.Instance.AddCardToHand(battleSpawner.PlayerCardZones[i].HeldCard);
                        battleSpawner.PlayerCardZones[i].DespawnCard();
                        battleSpawner.EnemyCardZones[i].DespawnCard();
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
                foreach (CardZone zone in battleSpawner.PlayerCardZones)
                {
                    if (zone.HasCard)
                    { PlayerHand.Instance.AddCardToHand(zone.HeldCard); }
                }
                phaseHandler.EndBattlePhase();
            }
        }

        private bool PlayerHasCards()
        {
            foreach (CardZone zone in battleSpawner.PlayerCardZones)
            {
                if (zone.HasCard) return true;
            }
            return false;
        }

        private bool EnemyHasCards()
        {
            foreach (CardZone zone in battleSpawner.EnemyCardZones)
            {
                if (zone.HasCard) return true;
            }
            return false;
        }
    }
}
