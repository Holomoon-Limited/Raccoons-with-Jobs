using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Holo.Racc.Game
{
    [CreateAssetMenu(fileName = "Phase Handler", menuName = "Game/New Phase Handler", order = 0)]
    public class PhaseHandler : ScriptableObject
    {
        public int PlayCardZoneCount { get; private set; }
        public int PlayerScore { get; private set; }
        public int OpponentScore { get; private set; }
        
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
            PlayerScore = 0;
            OpponentScore = 0;
            
            // DeckManager and PlayerHand reset their data in response to this 
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

            if (PlayerScore == 3 || OpponentScore == 3)
            {
                SceneManager.LoadScene("MainMenu");
            }

            else
            {
                StartDraftPhase();
            }
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
