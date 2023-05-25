using UnityEngine;
using System.Collections.Generic;

namespace Holo.Cards
{
    /// <summary>
    /// Base Effect class, to be inherited from
    /// </summary>
    public abstract class Effect : ScriptableObject
    {
        [field: SerializeField] public EffectTiming Timing { get; private set; }
        public abstract void Use(Card callingCard, Board board);
    }
}
