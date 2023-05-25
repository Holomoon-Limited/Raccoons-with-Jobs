using System.Collections;
using System.Collections.Generic;
using Holo.Racc.Game;
using UnityEngine;

namespace Holo.Cards
{
    public class Board : MonoBehaviour
    {
        public static Board Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                DontDestroyOnLoad(this.gameObject);
                Instance = this;
            }
        }

        [SerializeField] private BattleHandler battleHandler;

        [SerializeField] private Card playerCardPrefab;
        [SerializeField] private Card enemyCardPrefab;

        [SerializeField] private float timeBetweenSlides = 0.5f;

        public List<CardZone> PlayerZones = new List<CardZone>();
        public List<CardZone> EnemyZones = new List<CardZone>();

        public IEnumerator<Card> PlayerCards
        {
            get
            {
                foreach (CardZone zone in PlayerZones)
                {
                    if (zone.HasCard)
                    {
                        yield return zone.HeldCard;
                    }
                    else continue;
                }
            }
        }

        public IEnumerator<Card> EnemyCards
        {
            get
            {
                foreach (CardZone zone in EnemyZones)
                {
                    if (zone.HasCard)
                    {
                        yield return zone.HeldCard;
                    }
                    else continue;
                }
            }
        }

        public List<CardData> PlayerDestroyedCards = new List<CardData>();
        public List<CardData> EnemyDestroyedCards = new List<CardData>();

        private void OnEnable()
        {
            battleHandler.OnShuffleDown += SlideZonesDown;
        }

        private void OnDisable()
        {
            battleHandler.OnShuffleDown -= SlideZonesDown;
        }

        private void SlideZonesDown()
        {
            StartCoroutine(Co_SlideZonesDown());
        }

        private IEnumerator Co_SlideZonesDown()
        {
            List<CardZone> emptyPlayerZones = new List<CardZone>();
            List<CardZone> emptyEnemyZones = new List<CardZone>();

            for (int i = 0; i < PlayerZones.Count; i++)
            {
                if (PlayerZones[i].HeldCard == null)
                {
                    emptyPlayerZones.Add(PlayerZones[i]);
                    continue;
                }
                else
                {
                    if (emptyPlayerZones.Count > 0)
                    {
                        emptyPlayerZones[0].AddCardToZone(Board.Instance.PlayerZones[i].HeldCard);
                        PlayerZones[i].RemoveCardFromZone();
                        emptyPlayerZones.RemoveAt(0);
                        emptyPlayerZones.Add(Board.Instance.PlayerZones[i]);
                    }
                }
                yield return new WaitForSeconds(timeBetweenSlides);
            }

            for (int i = 0; i < PlayerZones.Count; i++)
            {
                if (Board.Instance.EnemyZones[i].HeldCard == null)
                {
                    emptyEnemyZones.Add(Board.Instance.EnemyZones[i]);
                    continue;
                }
                else
                {
                    if (emptyEnemyZones.Count > 0)
                    {
                        emptyEnemyZones[0].AddCardToZone(Board.Instance.EnemyZones[i].HeldCard);
                        Board.Instance.EnemyZones[i].RemoveCardFromZone();
                        emptyEnemyZones.RemoveAt(0);
                        emptyEnemyZones.Add(Board.Instance.EnemyZones[i]);
                    }
                }
                yield return new WaitForSeconds(timeBetweenSlides);
            }
            battleHandler.StartAttacks();
        }

        public void SpawnLastDestroyedCard(Card callingCard)
        {
            if (PlayerDestroyedCards.Count < 2) return;
            for (int i = 0; i < PlayerDestroyedCards.Count - 1; i--)
            {
                if (PlayerDestroyedCards[i] == callingCard) continue;
                Debug.Log($"Card to spawn: {PlayerDestroyedCards[i]}");
            }


            if (EnemyDestroyedCards.Count < 2) return;
            for (int i = 0; i < EnemyDestroyedCards.Count - 1; i--)
            {
                if (EnemyDestroyedCards[i] == callingCard) continue;
                Debug.Log($"Card to spawn: {EnemyDestroyedCards[i]}");
            }
        }

        // private bool IsPlayerCard(Card card)
        // {
        //     foreach (Card card in PlayerCards)
        //     {

        //     }
        // }

        public void ResetBoard()
        {
            PlayerZones.Clear();
            EnemyZones.Clear();
            PlayerDestroyedCards.Clear();
            EnemyDestroyedCards.Clear();
        }
    }
}
