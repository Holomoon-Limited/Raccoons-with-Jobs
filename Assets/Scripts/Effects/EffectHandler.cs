using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Holo.Cards
{
    public class EffectHandler : MonoBehaviour
    {
        public static EffectHandler Instance;

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

        [SerializeField][Min(0f)] private float timeBetweenEffects;


        public void ApplyContinuousEffects()
        {
            Board.Instance.ResetCardPower();
            for (int i = 0; i < Board.Instance.PlayerZones.Count; i++)
            {
                CardZone zone = Board.Instance.PlayerZones[i];
                if (zone.HasCard && zone.HeldCard.HasEffect && zone.HeldCard.Effect.Timing == EffectTiming.Continuous)
                {
                    zone.HeldCard.Effect.Use(zone.HeldCard, Board.Instance);
                }
                zone = Board.Instance.EnemyZones[i];
                if (zone.HasCard && zone.HeldCard.HasEffect && zone.HeldCard.Effect.Timing == EffectTiming.Continuous)
                {
                    zone.HeldCard.Effect.Use(zone.HeldCard, Board.Instance);
                }
            }
        }

        public IEnumerator Co_RunBattleStartEffects()
        {
            yield return new WaitForSeconds(timeBetweenEffects);
            for (int i = 0; i < Board.Instance.PlayerZones.Count; i++)
            {
                CardZone zone = Board.Instance.PlayerZones[i];
                if (zone.HasCard && zone.HeldCard.HasEffect && zone.HeldCard.Effect.Timing == EffectTiming.OnBattleStart)
                {
                    zone.HeldCard.ActivateEffect();
                    zone.HeldCard.Effect.Use(zone.HeldCard, Board.Instance);
                    yield return timeBetweenEffects;
                }
                zone = Board.Instance.EnemyZones[i];
                if (zone.HasCard && zone.HeldCard.HasEffect && zone.HeldCard.Effect.Timing == EffectTiming.OnBattleStart)
                {
                    zone.HeldCard.ActivateEffect();
                    zone.HeldCard.Effect.Use(zone.HeldCard, Board.Instance);
                    yield return timeBetweenEffects;
                }
            }
        }

    }
}