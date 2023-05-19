using System;
using UnityEngine;

namespace Holo.Racc.Game
{
    [CreateAssetMenu(fileName = "Play Handler", menuName = "Game/New Play Handler", order = 0)]
    public class PlayHandler : ScriptableObject
    {
        [Header("Asset References")]
        [SerializeField] private PhaseHandler phaseHandler;
        
        private void OnEnable()
        {
            this.hideFlags = HideFlags.DontUnloadUnusedAsset;
        }

        // called by EndPlayPhaseButton.cs
        public void EndPlayPhase()
        {
            phaseHandler.EndPlayPhase();
        }
    }
}
