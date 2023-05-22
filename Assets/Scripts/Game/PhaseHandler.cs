using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Holo.Racc.Game
{
    [CreateAssetMenu(fileName = "Phase Handler", menuName = "Game/New Phase Handler", order = 0)]
    public class PhaseHandler : ScriptableObject
    {
        [Header("Asset References")] 
        [SerializeField] private ScoreManager scoreManager;
        
        public int PlayCardZoneCount { get; private set; }

        // Actions
        public event Action OnGameStart;

        // Draft
        public event Action OnDraftEnd;

        // Play
        public event Action OnPlayEnd;

        // Battle
        public event Action OnBattleEnd;

        private void OnEnable()
        {
            this.hideFlags = HideFlags.DontUnloadUnusedAsset;
        }

        public void StartGame()
        {
            // all data resets 
            PlayCardZoneCount = 3;

            // DeckManager resets in response to this 
            OnGameStart?.Invoke();

            StartDraftPhase();
        }

        // Draft
        private void StartDraftPhase()
        {
            SceneManager.LoadScene("Draft");
        }
        public void EndDraftPhase()
        {
            OnDraftEnd?.Invoke();
            StartPlayPhase();
        }

        // Play
        private void StartPlayPhase()
        {
            SceneManager.LoadScene("Play");
        }
        public void EndPlayPhase()
        {
            OnPlayEnd?.Invoke();

            if (PlayCardZoneCount < 5)
            {
                PlayCardZoneCount++;
            }

            StartBattlePhase();
        }

        // Battle
        private void StartBattlePhase()
        {
            SceneManager.LoadScene("Battle");
        }

        public void EndBattlePhase()
        {
            OnBattleEnd?.Invoke();

            if (scoreManager.GameOver)
            {
                SceneManager.LoadScene("GameEnd");
            }

            else
            {
                StartDraftPhase();
            }
        }

        // called on the Replay button in the GameEnd Scene 
        public void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
