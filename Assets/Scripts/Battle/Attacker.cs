using System.Collections;
using Holo.Cards;
using UnityEngine;

namespace Holo.Racc.Battle
{
    /// <summary>
    /// Runs the battle attack phase
    /// </summary>
    public class Attacker : MonoBehaviour
    {
        [SerializeField] BattleHandler battleHandler;

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
                        battleSpawner.PlayerCardZones[i].DespawnCard();
                    }
                    else
                    {
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
                if (!PlayerHasCards() && !EnemyHasCards())
                {
                    Debug.Log("Draw");
                }
                else if (PlayerHasCards() && !EnemyHasCards())
                {
                    Debug.Log("Player wins");
                }
                else if (!PlayerHasCards() && EnemyHasCards())
                {
                    Debug.Log("Enemy wins");
                }
                //Start next draft phase here
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
