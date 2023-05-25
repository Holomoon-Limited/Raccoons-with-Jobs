using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Holo.Racc.Game
{
    [CreateAssetMenu(fileName = "Transition Handler", menuName = "Game/New Transition Handler", order = 0)]
    public class TransitionHandler : ScriptableObject
    {
        [Header("Design Values")] [Tooltip("time in seconds")] 
        [SerializeField] private float transitionTime;
        public float TransitionTime => transitionTime;

        public event Action OnTransitionOver;

        public void TransitionOver()
        {
            OnTransitionOver?.Invoke();
        }
    }
}
