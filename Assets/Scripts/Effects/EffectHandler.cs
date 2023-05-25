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

        Dictionary<Card, Effect> effects = new Dictionary<Card, Effect>();

        public void RegisterEffect(Card card)
        {
            if (effects.ContainsKey(card)) return;
            if (card.HasEffect == false) return;
            effects.Add(card, card.Effect);
        }

        public void UnRegisterEffect(Card card)
        {
            if (effects.ContainsKey(card))
            {
                effects.Remove(card);
            }
        }

        public void ClearEffects()
        {
            effects.Clear();
        }

        public void ApplyContinuousEffects()
        {
            Debug.Log("Applying Continuous Effects");
            foreach (KeyValuePair<Card, Effect> effect in effects)
            {
                if (effect.Value.Timing == EffectTiming.Continuous)
                {
                    effect.Value.Use(effect.Key, Board.Instance);
                }
            }
        }

        public IEnumerator Co_RunBattleStartEffects()
        {
            Debug.Log("Running Battle Start Effects");
            foreach (KeyValuePair<Card, Effect> effect in effects)
            {
                if (effect.Value.Timing == EffectTiming.OnBattleStart)
                {
                    effect.Value.Use(effect.Key, Board.Instance);
                    yield return new WaitForSeconds(timeBetweenEffects);
                }
            }
        }

    }
}