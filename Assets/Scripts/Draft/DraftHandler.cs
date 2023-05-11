using System;
using UnityEngine;

namespace Holo.Racc.Draft
{
    [CreateAssetMenu(fileName = "Draft Handler", menuName = "Draft/New Draft Handler", order = 0)]
    public class DraftHandler : ScriptableObject
    {
        public event Action OnStartDraft;

        private DraftPhase draftPhase;

        public void StartDraft()
        {
            this.draftPhase = DraftPhase.FirstPick;
            OnStartDraft?.Invoke();
        }

        public void ProgressPhase()
        {
            switch (draftPhase)
            {
                case DraftPhase.FirstPick:
                    break;
                case DraftPhase.SecondPick:
                    break;
                case DraftPhase.ThirdPick:
                    break;
                case DraftPhase.FourthPick:
                    break;
                default:
                    break;
            }
        }
    }
}
