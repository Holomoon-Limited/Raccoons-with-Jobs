using System;
using System.Collections;
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
        public event Action OnDraftEnd;
        public event Action OnPlayEnd;
        public event Action OnBattleEnd;
        public event Action OnGameEnd;
        

        private void OnEnable()
        {
            this.hideFlags = HideFlags.DontUnloadUnusedAsset;
        }

        // called by Start button in MainMenu 
        public void StartGame()
        {
            PlayCardZoneCount = 3;
            
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

        public IEnumerator Co_EndBattlePhase()
        {
            OnBattleEnd?.Invoke();

            // time delay to allow score board to update 
            yield return new WaitForSeconds(2);

            if (scoreManager.GameOver)
            {
                OnGameEnd?.Invoke();
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
