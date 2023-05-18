using Holo.Cards;
using UnityEngine;
using System.Collections.Generic;

namespace Holo.Racc.Battle
{
    public class BattleSpawner : MonoBehaviour
    {
        [SerializeField] BattleHandler battleHandler;
        [SerializeField] private CardZone cardZonePrefab;
        [SerializeField] private Card cardPrefab;

        [SerializeField] private Transform playerZoneParent;
        [SerializeField] private Transform enemyZoneParent;

        public List<CardZone> PlayerCardZones { get; private set; } = new List<CardZone>();
        public List<CardZone> EnemyCardZones { get; private set; } = new List<CardZone>();

        public int Zones { get; private set; } = 0;

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
            DestroyZones();
            PlayerCardZones = SpawnCards(battleHandler.PlayerCards, playerZoneParent);
            EnemyCardZones = SpawnCards(battleHandler.EnemyCards, enemyZoneParent);
            Zones = PlayerCardZones.Count;
            battleHandler.StartAttacks();
        }

        private List<CardZone> SpawnCards(List<CardData> cardsToSpawn, Transform parent)
        {
            List<CardZone> cardZones = new List<CardZone>();
            Vector3 position = Vector3.zero;
            if (cardsToSpawn.Count % 2 == 0)
            {
                position.x = -0.75f * (cardsToSpawn.Count - 1);
            }
            else
            {
                position.x = (float)(-1.5f * (int)(cardsToSpawn.Count / 2));
            }
            foreach (CardData cardData in cardsToSpawn)
            {
                CardZone zone = Instantiate(cardZonePrefab, parent);
                cardZones.Add(zone);
                zone.transform.localPosition = position;
                Card card = Instantiate(cardPrefab, parent.transform);
                card.DisplayCard(cardData);
                zone.AddCardToZone(card);
                position.x += 1.5f;
            }
            return cardZones;
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
