using Holo.Cards;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Holo.Racc.Game;

namespace Holo.Racc.Battle
{
    public class BattleSpawner : MonoBehaviour
    {
        [SerializeField] BattleHandler battleHandler;
        [SerializeField] private CardZone cardZonePrefab;
        [SerializeField] private Card playerCardPrefab;
        [SerializeField] private Card enemyCardPrefab;

        [SerializeField] private Transform playerZoneParent;
        [SerializeField] private Transform enemyZoneParent;

        [SerializeField][Min(0f)] private float timeBetweenShuffles = 0.5f;

        public List<CardZone> PlayerCardZones { get; private set; } = new List<CardZone>();
        public List<CardZone> EnemyCardZones { get; private set; } = new List<CardZone>();

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
            PlayerCardZones = SpawnPlayerCards();
            EnemyCardZones = SpawnEnemyCards();
            Zones = PlayerCardZones.Count;
            battleHandler.StartAttacks();
        }

        private List<CardZone> SpawnPlayerCards()
        {
            List<CardZone> cardZones = new List<CardZone>();
            Vector3 position = Vector3.zero;
            if (battleHandler.PlayerCards.Count % 2 == 0)
            {
                position.x = -0.75f * (battleHandler.PlayerCards.Count - 1);
            }
            else
            {
                position.x = (float)(-1.5f * (int)(battleHandler.PlayerCards.Count / 2));
            }
            foreach (CardData cardData in battleHandler.PlayerCards)
            {
                CardZone zone = Instantiate(cardZonePrefab, playerZoneParent);
                cardZones.Add(zone);
                zone.transform.localPosition = position;
                Card card = Instantiate(playerCardPrefab, playerZoneParent.transform);
                card.DisplayCard(cardData);
                zone.AddCardToZone(card);
                position.x += 1.5f;
            }
            return cardZones;
        }

        private List<CardZone> SpawnEnemyCards()
        {
            List<CardZone> cardZones = new List<CardZone>();
            Vector3 position = Vector3.zero;
            if (battleHandler.EnemyCards.Count % 2 == 0)
            {
                position.x = -0.75f * (battleHandler.EnemyCards.Count - 1);
            }
            else
            {
                position.x = (float)(-1.5f * (int)(battleHandler.EnemyCards.Count / 2));
            }
            foreach (CardData cardData in battleHandler.EnemyCards)
            {
                CardZone zone = Instantiate(cardZonePrefab, enemyZoneParent);
                cardZones.Add(zone);
                zone.transform.localPosition = position;
                Card card = Instantiate(enemyCardPrefab, enemyZoneParent.transform);
                card.DisplayCard(cardData);
                zone.AddCardToZone(card);
                position.x += 1.5f;
            }
            return cardZones;
        }

        private void ShuffleDown()
        {
            StartCoroutine(Co_EnemyShuffleDown());
            StartCoroutine(Co_PlayerShuffleDown());
        }

        private IEnumerator Co_PlayerShuffleDown()
        {
            CardZone lastEmptyZone = null;
            for (int i = 0; i < Zones; i++)
            {
                if (PlayerCardZones[i].HeldCard == null)
                {
                    lastEmptyZone = PlayerCardZones[i];
                    continue;
                }
                else
                {
                    if (lastEmptyZone != null)
                    {
                        lastEmptyZone.AddCardToZone(PlayerCardZones[i].HeldCard);
                        PlayerCardZones[i].RemoveCardFromZone();
                        lastEmptyZone = PlayerCardZones[i];
                    }
                }
                yield return new WaitForSeconds(timeBetweenShuffles);
            }
            battleHandler.StartAttacks();
        }

        private IEnumerator Co_EnemyShuffleDown()
        {
            CardZone lastEmptyZone = null;
            for (int i = 0; i < Zones; i++)
            {
                if (EnemyCardZones[i].HeldCard == null)
                {
                    lastEmptyZone = EnemyCardZones[i];
                    continue;
                }
                else
                {
                    if (lastEmptyZone != null)
                    {
                        lastEmptyZone.AddCardToZone(EnemyCardZones[i].HeldCard);
                        EnemyCardZones[i].RemoveCardFromZone();
                        lastEmptyZone = EnemyCardZones[i];
                    }
                }
                yield return new WaitForSeconds(timeBetweenShuffles);
            }
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
