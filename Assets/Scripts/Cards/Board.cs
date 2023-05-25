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

        [SerializeField] private Transform playerGraveyard;
        [SerializeField] private Transform enemyGraveyard;

        [SerializeField] private float timeBetweenSlides = 0.5f;

        public List<CardZone> PlayerZones = new List<CardZone>();
        public List<CardZone> EnemyZones = new List<CardZone>();

        public List<Card> PlayerDestroyedCards = new List<Card>();
        public List<Card> EnemyDestroyedCards = new List<Card>();

        public List<Card> PlayerCards = new List<Card>();
        public List<Card> EnemyCards = new List<Card>();

        public int DestroyedPlayerCardsNumber = 0;
        public int DestroyedEnemyCardsNumber = 0;

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
            if (PlayerCards.Contains(callingCard))
            {
                if (PlayerDestroyedCards.Count < 2) return;
                for (int i = PlayerDestroyedCards.Count - 1; i >= 0; i--)
                {
                    if (PlayerDestroyedCards[i] == callingCard) continue;
                    PlayerZones[callingCard.Position].AddCardToZone(PlayerDestroyedCards[i]);
                    PlayerDestroyedCards.Remove(PlayerDestroyedCards[i]);
                    Debug.Log($"Card to spawn: {PlayerDestroyedCards[i].CardData.CardName}");
                    return;
                }
            }
            else
            {

                if (EnemyDestroyedCards.Count < 2) return;
                for (int i = EnemyDestroyedCards.Count - 1; i >= 0; i--)
                {
                    if (EnemyDestroyedCards[i] == callingCard) continue;
                    EnemyZones[callingCard.Position].AddCardToZone(EnemyDestroyedCards[i]);
                    EnemyDestroyedCards.Remove(EnemyDestroyedCards[i]);
                    Debug.Log($"Card to spawn: {EnemyDestroyedCards[i].CardData.CardName}");
                    return;
                }
            }
            EffectHandler.Instance.ApplyContinuousEffects();
        }

        public void ResetCardPower()
        {
            foreach (Card card in PlayerCards)
            {
                card.ResetPower();
            }
            foreach (Card card in EnemyCards)
            {
                card.ResetPower();
            }
        }

        public void DestroyEnemyCard(CardZone zone)
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
            EffectHandler.Instance.ApplyContinuousEffects();
        }

        public void DestroyPlayerCard(CardZone zone)
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
            EffectHandler.Instance.ApplyContinuousEffects();
        }

        public void ResetBoard()
        {
            foreach (Card card in PlayerCards)
            {
                card.ResetPower();
                PlayerHand.Instance.AddCardToHand(card);
            }

            DestroyedPlayerCardsNumber = 0;
            DestroyedEnemyCardsNumber = 0;

            PlayerZones.Clear();
            EnemyZones.Clear();
            PlayerCards.Clear();
            EnemyCards.Clear();
            PlayerDestroyedCards.Clear();
            EnemyDestroyedCards.Clear();
        }
    }
}
