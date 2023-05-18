using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Holo.Racc.Game
{
    [CreateAssetMenu(fileName = "Phase Handler", menuName = "Game/New Phase Handler", order = 0)]
    public class PhaseHandler : ScriptableObject
    {
        public static event Action OnDraftStart = () => { };
        public static event Action OnPlayStart = () => { };
        public static event Action OnBattleStart = () => { };
        
        private void OnEnable()
        {
            this.hideFlags = HideFlags.DontUnloadUnusedAsset;
        }
        
    }
}
