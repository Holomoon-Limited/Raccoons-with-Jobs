using Holo.Cards;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Holo.Racc.Game;
using System;

namespace Holo.Racc.Battle
{
    /// <summary>
    /// Spawns cards for the battle phase 
    /// </summary>
    public class BattleSpawner : MonoBehaviour
    {
        [Header("Project References")]
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] CardsInPlayContainer cardsInPlay;
        [SerializeField] BattleHandler battleHandler;
        [SerializeField] private CardZone cardZonePrefab;
        [SerializeField] private Card playerCardPrefab;
        [SerializeField] private Card enemyCardPrefab;

        [Header("Prefab references")]
        [SerializeField] private Transform playerZoneParent;
        [SerializeField] private Transform enemyZoneParent;

        private void OnEnable()
        {
            battleHandler.OnBattleStart += SetupZones;
        }

        private void OnDisable()
        {
            battleHandler.OnBattleStart -= SetupZones;
        }

        private void Start()
        {
            battleHandler.StartBattle();
        }

        private void SetupZones()
        {
            StartCoroutine(Co_SetupBoard());
        }

        private IEnumerator Co_SetupBoard()
        {
            DestroyZones();

            SpawnCardZones(cardsInPlay.playerCardsInPlay.Count);

            SpawnPlayerCards();
            SpawnEnemyCards();

            EffectHandler.Instance.ApplyContinuousEffects();
            yield return EffectHandler.Instance.Co_RunBattleStartEffects();

            battleHandler.StartAttacks();
        }

        private void SpawnPlayerCards()
        {
            for (int i = 0; i < Board.Instance.PlayerZones.Count; i++)
            {
                Card card = Instantiate(playerCardPrefab);
                card.DisplayCard(cardsInPlay.playerCardsInPlay[i]);
                card.Position = i;
                Board.Instance.PlayerZones[i].AddCardToZone(card);
                Board.Instance.PlayerCards.Add(card);
                EffectHandler.Instance.RegisterEffect(card);
            }
        }

        private void SpawnEnemyCards()
        {
            for (int i = 0; i < Board.Instance.EnemyZones.Count; i++)
            {
                Card card = Instantiate(enemyCardPrefab);
                card.DisplayCard(cardsInPlay.enemyCardsInPlay[i]);
                card.Position = i;
                Board.Instance.EnemyZones[i].AddCardToZone(card);
                Board.Instance.EnemyCards.Add(card);
                EffectHandler.Instance.RegisterEffect(card);
            }
        }

        public void SpawnCardZones(int numberOfZones)
        {
            Vector3 position = Vector3.zero;
            if (numberOfZones % 2 == 0)
            {
                position.x = -0.75f * (battleHandler.PlayerCards.Count / 2);
            }
            else
            {
                position.x = (float)(-1.5f * (int)(numberOfZones / 2));
            }

            for (int i = 0; i < numberOfZones; i++)
            {
                CardZone playerZone = Instantiate(cardZonePrefab, playerZoneParent);
                playerZone.transform.localPosition = position;
                Board.Instance.PlayerZones.Add(playerZone);

                CardZone enemyZone = Instantiate(cardZonePrefab, enemyZoneParent);
                enemyZone.transform.localPosition = position;
                Board.Instance.EnemyZones.Add(enemyZone);
                position.x += 1.5f;
            }
        }

        private void DestroyZones()
        {
            foreach (CardZone zone in Board.Instance.PlayerZones)
            {
                Destroy(zone.gameObject);
            }
            foreach (CardZone zone in Board.Instance.PlayerZones)
            {
                Destroy(zone.gameObject);
            }

            Board.Instance.ResetBoard();
        }
    }
}
