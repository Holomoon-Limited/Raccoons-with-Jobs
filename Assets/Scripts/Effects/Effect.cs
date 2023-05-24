using UnityEngine;
using System.Collections.Generic;

namespace Holo.Cards
{
    /// <summary>
    /// Base Effect class, to be inherited from
    /// </summary>
    [System.Serializable]
    public abstract class Effect
    {
        [field: SerializeField] public EffectTiming Timing { get; private set; }
        public abstract void Use(Card callingCard, List<Card> friendlyCards, List<Card> enemyCards);
    }
}
