using System;
using Holo.Input;
using Holo.Racc.Draft;
using UnityEngine;

namespace Holo.Racc.Game
{
    [CreateAssetMenu(fileName = "Draft Handler", menuName = "Draft/New Draft Handler", order = 0)]
    public class DraftHandler : ScriptableObject
    {
        [Header("Asset References")]
        [SerializeField] private PhaseHandler phaseHandler;
        [SerializeField] private InputManager input;
        public int Picks { get; private set; }

        public event Action OnStartDraft;
        public event Action<int> OnPlayerPick;
        public event Action<int> OnAIPick;

        private DraftPhase draftPhase;

        public void StartDraft()
        {
            Picks = 1;
            this.draftPhase = DraftPhase.FirstPick;
            OnStartDraft?.Invoke();
        }

        public void ProgressPhase()
        {
            draftPhase++;
            if ((int)draftPhase >= System.Enum.GetValues(typeof(DraftPhase)).Length) draftPhase = 0;

            switch (draftPhase)
            {
                case DraftPhase.FirstPick:
                    input.EnableInput();
                    Picks = 1;
                    OnPlayerPick?.Invoke(Picks);
                    break;
                case DraftPhase.SecondPick:
                    input.DisableInput();
                    OnAIPick?.Invoke(2);
                    break;
                case DraftPhase.ThirdPick:
                    input.EnableInput();
                    Picks = 2;
                    OnPlayerPick?.Invoke(Picks);
                    break;
                case DraftPhase.FourthPick:
                    input.DisableInput();
                    OnAIPick?.Invoke(1);
                    break;
                case DraftPhase.PicksCompleted:
                    input.EnableInput();
                    phaseHandler.EndDraftPhase();
                    break;
                default:
                    break;
            }
        }
    }
}
