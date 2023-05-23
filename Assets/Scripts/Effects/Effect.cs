using UnityEngine;
using System.Collections.Generic;

namespace Holo.Cards.Effects
{
    /// <summary>
    /// Base Effect class, to be inherited from
    /// </summary>
    public abstract class Effect : ScriptableObject
    {
        [SerializeField] protected EffectTiming timing;

        public virtual void Register()
        {

        }

        public virtual void UnRegister()
        {

        }

        public abstract void Use(List<Card> friendlyCards, List<Card> enemyCards);
    }
}
