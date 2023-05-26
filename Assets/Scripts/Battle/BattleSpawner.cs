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
        [SerializeField] CardsInPlayContainer cardsInPlay;
        [SerializeField] private TransitionHandler transitionHandler;
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
            transitionHandler.OnTransitionOver += InitiateBattle;
        }

        private void OnDisable()
        {
            battleHandler.OnBattleStart -= SetupZones;
            transitionHandler.OnTransitionOver -= InitiateBattle;
        }

        private void Start()
        {
            battleHandler.StartBattle();
        }

        private void SetupZones()
        {
            SetupBoard();
        }

        private void SetupBoard()
        {
            DestroyZones();

            SpawnCardZones(cardsInPlay.playerCardsInPlay.Count);

            SpawnPlayerCards();
            SpawnEnemyCards();
        }

        private void InitiateBattle()
        {
            StartCoroutine(Co_InitiateBattle());
        }

        private IEnumerator Co_InitiateBattle()
        {
            EffectHandler.Instance.ApplyContinuousEffects();
            yield return EffectHandler.Instance.Co_RunBattleStartEffects();

            battleHandler.StartAttacks();
        }

        private void SpawnPlayerCards()
        {
            for (int i = 0; i < Board.Instance.PlayerZones.Count; i++)
            {
                Card card = Instantiate(playerCardPrefab);
                card.SetCardData(cardsInPlay.playerCardsInPlay[i]);
                card.Position = i;
                Board.Instance.PlayerZones[i].AddCardToZone(card);
                Board.Instance.PlayerCards.Add(card);
            }
        }

        private void SpawnEnemyCards()
        {
            for (int i = 0; i < Board.Instance.EnemyZones.Count; i++)
            {
                Card card = Instantiate(enemyCardPrefab);
                card.SetCardData(cardsInPlay.enemyCardsInPlay[i]);
                card.Position = i;
                Board.Instance.EnemyZones[i].AddCardToZone(card);
                Board.Instance.EnemyCards.Add(card);
            }
        }

        public void SpawnCardZones(int numberOfZones)
        {
            Vector3 position = Vector3.zero;
            if (numberOfZones % 2 == 0)
            {
                position.x = -1f * (numberOfZones - 1);
            }
            else
            {
                position.x = (float)(-2f * (int)(numberOfZones / 2));
            }

            for (int i = 0; i < numberOfZones; i++)
            {
                CardZone playerZone = Instantiate(cardZonePrefab, playerZoneParent);
                playerZone.transform.localPosition = position;
                Board.Instance.PlayerZones.Add(playerZone);

                CardZone enemyZone = Instantiate(cardZonePrefab, enemyZoneParent);
                enemyZone.transform.localPosition = position;
                Board.Instance.EnemyZones.Add(enemyZone);
                position.x += 2f;
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
