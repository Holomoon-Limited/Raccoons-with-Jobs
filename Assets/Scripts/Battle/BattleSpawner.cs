using Holo.Cards;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Holo.Racc.Game;

namespace Holo.Racc.Battle
{
    /// <summary>
    /// Spawns cards for the battle phase 
    /// </summary>
    public class BattleSpawner : MonoBehaviour
    {
        [Header("Project References")]
        [SerializeField] BattleHandler battleHandler;
        [SerializeField] private CardZone cardZonePrefab;
        [SerializeField] private Card playerCardPrefab;
        [SerializeField] private Card enemyCardPrefab;

        [Header("Prefab references")]
        [SerializeField] private Transform playerZoneParent;
        [SerializeField] private Transform enemyZoneParent;

        [Header("Tuning variables")]
        [SerializeField][Min(0f)] private float timeBetweenShuffles = 0.5f;

        public List<CardZone> PlayerCardZones { get; private set; } = new List<CardZone>();
        public List<CardZone> EnemyCardZones { get; private set; } = new List<CardZone>();

        //The number of zones to spawn 
        public int Zones { get; private set; } = 0;

        private void OnEnable()
        {
            battleHandler.OnBattleStart += SetupZones;
            battleHandler.OnShuffleDown += ShuffleDown;
        }

        private void OnDisable()
        {
            battleHandler.OnBattleStart -= SetupZones;
            battleHandler.OnShuffleDown -= ShuffleDown;
        }

        private void Start()
        {
            battleHandler.StartBattle();
        }

        private void SetupZones()
        {
            DestroyZones();
            PlayerCardZones = SpawnCardList(battleHandler.PlayerCards, playerZoneParent, playerCardPrefab);
            EnemyCardZones = SpawnCardList(battleHandler.EnemyCards, enemyZoneParent, enemyCardPrefab);
            Zones = PlayerCardZones.Count;
            battleHandler.StartAttacks();
        }

        private List<CardZone> SpawnCardList(List<CardData> cardsToSpawn, Transform spawnParent, Card cardPrefab)
        {
            List<CardZone> cardZones = new List<CardZone>();
            Vector3 position = Vector3.zero;
            if (cardsToSpawn.Count % 2 == 0)
            {
                position.x = -0.75f * (battleHandler.PlayerCards.Count - 1);
            }
            else
            {
                position.x = (float)(-1.5f * (int)(cardsToSpawn.Count / 2));
            }
            foreach (CardData cardData in cardsToSpawn)
            {
                CardZone zone = Instantiate(cardZonePrefab, spawnParent);
                cardZones.Add(zone);
                zone.transform.localPosition = position;
                Card card = Instantiate(cardPrefab, spawnParent);
                card.DisplayCard(cardData);
                zone.AddCardToZone(card);
                position.x += 1.5f;
            }
            return cardZones;
        }

        private void ShuffleDown()
        {
            StartCoroutine(Co_ShuffleDownZones(PlayerCardZones));
            StartCoroutine(Co_ShuffleDownZones(EnemyCardZones));
        }

        private IEnumerator Co_ShuffleDownZones(List<CardZone> zones)
        {
            CardZone lastEmptyZone = null;
            for (int i = 0; i < Zones; i++)
            {
                if (zones[i].HeldCard == null)
                {
                    lastEmptyZone = zones[i];
                    continue;
                }
                else
                {
                    if (lastEmptyZone != null)
                    {
                        lastEmptyZone.AddCardToZone(zones[i].HeldCard);
                        zones[i].RemoveCardFromZone();
                        lastEmptyZone = zones[i];
                    }
                }
                yield return new WaitForSeconds(timeBetweenShuffles);
            }
            battleHandler.StartAttacks();
        }

        private void DestroyZones()
        {
            foreach (CardZone zone in PlayerCardZones)
            {
                Destroy(zone.gameObject);
                PlayerCardZones.Remove(zone);
            }
            foreach (CardZone zone in EnemyCardZones)
            {
                Destroy(zone.gameObject);
                EnemyCardZones.Remove(zone);
            }
        }
    }
}
