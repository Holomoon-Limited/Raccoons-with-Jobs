using System.Collections;
using Holo.Cards;
using UnityEngine;

namespace Holo.Racc.Battle
{
    public class Attacker : MonoBehaviour
    {
        [SerializeField] BattleHandler battleHandler;

        [SerializeField][Min(0f)] private float timeBetweenAttacks = 0.5f;

        private BattleSpawner battleSpawner;

        private void Awake()
        {
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
            yield return new WaitForSeconds(timeBetweenAttacks);
            for (int i = 0; i < battleSpawner.Zones; i++)
            {
                Card playerCard = battleSpawner.PlayerCardZones[i].HeldCard;
                Card enemyCard = battleSpawner.EnemyCardZones[i].HeldCard;
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
        }
    }
}
