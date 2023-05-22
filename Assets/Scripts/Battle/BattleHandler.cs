using System.Collections.Generic;
using Holo.Cards;
using UnityEngine;
using System;

namespace Holo.Racc.Battle
{
    /// <summary>
    /// Battle phase event bus
    /// </summary>
    [CreateAssetMenu(fileName = "Battle Handler", menuName = "Battle/New Battle Handler", order = 0)]
    public class BattleHandler : ScriptableObject
    {
        [field: SerializeField] public List<CardData> PlayerCards { get; private set; } = new List<CardData>();
        [field: SerializeField] public List<CardData> EnemyCards { get; private set; } = new List<CardData>();

        public int NumberOfCards { get; private set; } = 0;

        public event Action OnBattleStart;
        public event Action OnAttackPhase;
        public event Action OnShuffleDown;

        public void StartBattle()
        {
            OnBattleStart?.Invoke();
        }

        public void StartAttacks()
        {
            OnAttackPhase?.Invoke();
        }

        public void ShuffleDown()
        {
            OnShuffleDown?.Invoke();

        }
    }

}