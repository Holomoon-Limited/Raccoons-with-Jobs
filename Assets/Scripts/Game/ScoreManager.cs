using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Holo.Racc.Game
{
    [CreateAssetMenu(fileName = "Score Manager", menuName = "Game/New Score Manager", order = 0)]
    public class ScoreManager : ScriptableObject
    {
        public int PlayerScore { get; private set; }
        public int EnemyScore { get; private set; }

        public event Action OnScoreUpdated;

        public bool GameOver => (PlayerScore >= 3) || (EnemyScore >= 3);
        
        private void OnEnable()
        {
            this.hideFlags = HideFlags.DontUnloadUnusedAsset;
        }

        public void IncreasePlayerScore()
        {
            PlayerScore++;
            if (PlayerScore >= 3)
            {
                Debug.Log("Player wins!");
            }
            OnScoreUpdated?.Invoke();
        }

        public void IncreaseEnemyScore()
        {
            EnemyScore++;
            if (EnemyScore >= 3)
            {
                Debug.Log("Enemy wins!");
            }
            
            OnScoreUpdated?.Invoke();
        }

        public void ResetScores()
        {
            PlayerScore = 0;
            EnemyScore = 0;
            
            OnScoreUpdated?.Invoke();
        }
    }
}