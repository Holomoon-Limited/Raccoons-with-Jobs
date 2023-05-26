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
            List<int> emptyIndexes = new List<int>();

            for (int i = 0; i < PlayerZones.Count; i++)
            {
                if (PlayerZones[i].HeldCard == null)
                {
                    emptyIndexes.Add(i);
                    continue;
                }
                else
                {
                    if (emptyIndexes.Count > 0)
                    {
                        PlayerZones[emptyIndexes[0]].AddCardToZone(Board.Instance.PlayerZones[i].HeldCard);
                        PlayerZones[emptyIndexes[0]].HeldCard.Position = emptyIndexes[0];
                        PlayerZones[i].RemoveCardFromZone();
                        emptyIndexes.RemoveAt(0);
                        emptyIndexes.Add(i);
                    }
                }
                yield return new WaitForSeconds(timeBetweenSlides);
            }

            emptyIndexes.Clear();

            for (int i = 0; i < EnemyZones.Count; i++)
            {
                if (Board.Instance.EnemyZones[i].HeldCard == null)
                {
                    emptyIndexes.Add(i);
                    continue;
                }
                else
                {
                    if (emptyIndexes.Count > 0)
                    {
                        EnemyZones[emptyIndexes[0]].AddCardToZone(Board.Instance.EnemyZones[i].HeldCard);
                        EnemyZones[emptyIndexes[0]].HeldCard.Position = emptyIndexes[0];
                        Board.Instance.EnemyZones[i].RemoveCardFromZone();
                        emptyIndexes.RemoveAt(0);
                        emptyIndexes.Add(i);
                    }
                }
                yield return new WaitForSeconds(timeBetweenSlides);
            }
            EffectHandler.Instance.ApplyContinuousEffects();
            battleHandler.StartAttacks();
        }

        public void SpawnLastDestroyedCard(Card callingCard, Effect effect)
        {
            if (PlayerCards.Contains(callingCard))
            {
                if (PlayerDestroyedCards.Count < 2) return;
                for (int i = PlayerDestroyedCards.Count - 1; i >= 0; i--)
                {
                    if (PlayerDestroyedCards[i].HasEffect && PlayerDestroyedCards[i].Effect == effect) continue;
                    PlayerZones[callingCard.Position].AddCardToZone(PlayerDestroyedCards[i]);
                    PlayerDestroyedCards.Remove(PlayerDestroyedCards[i]);
                    return;
                }
            }
            else
            {

                if (EnemyDestroyedCards.Count < 2) return;
                for (int i = EnemyDestroyedCards.Count - 1; i >= 0; i--)
                {
                    if (EnemyDestroyedCards[i].HasEffect && EnemyDestroyedCards[i].Effect == effect) continue;
                    EnemyZones[callingCard.Position].AddCardToZone(EnemyDestroyedCards[i]);
                    EnemyDestroyedCards.Remove(EnemyDestroyedCards[i]);
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

        public void DestroyEnemyCard(CardZone zone, bool triggerEffect = true)
        {
            if (zone.HasCard == false) return;
            Card card = zone.HeldCard;
            bool negated = card.Negated;
            zone.RemoveCardFromZone();
            card.MoveToPoint(enemyGraveyard.position, enemyGraveyard.rotation);
            EnemyDestroyedCards.Add(card);
            DestroyedEnemyCardsNumber++;
            if (card.HasEffect && card.Effect.Timing == EffectTiming.OnCardDestroyed && triggerEffect && negated == false)
            {
                card.Effect.Use(card, Board.Instance);
            }
            EffectHandler.Instance.ApplyContinuousEffects();
        }

        public void DestroyPlayerCard(CardZone zone, bool triggerEffect = true)
        {
            if (zone.HasCard == false) return;
            Card card = zone.HeldCard;
            bool negated = card.Negated;
            zone.RemoveCardFromZone();
            card.MoveToPoint(playerGraveyard.position, playerGraveyard.rotation);
            PlayerDestroyedCards.Add(card);
            DestroyedPlayerCardsNumber++;
            if (card.HasEffect && card.Effect.Timing == EffectTiming.OnCardDestroyed && triggerEffect && negated == false)
            {
                card.Effect.Use(card, Board.Instance);
            }
            EffectHandler.Instance.ApplyContinuousEffects();
        }

        public void ResetBoard()
        {
            foreach (Card card in PlayerCards)
            {
                card.ResetCardEffects();
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
