using Holo.Racc.Game;
using UnityEngine;

namespace Holo.Racc.UI
{
    public class ScoreDisplay : MonoBehaviour
    {
        [Header("Asset References")] [SerializeField]
        private ScoreManager scoreManager;

        [SerializeField] private PhaseHandler phaseHandler;

        [Header("Component References")] 
        [SerializeField] private GameObject[] children;
        [SerializeField] private GameObject[] playerScoreMarkers;
        [SerializeField] private GameObject[] enemyScoreMarkers;

        private void OnEnable()
        {
            scoreManager.OnScoreUpdated += UpdateDisplayScore;
            
            phaseHandler.OnGameStart += EnableScoreDisplay;
            phaseHandler.OnGameEnd += DisableScoreDisplay;
        }

        private void OnDisable()
        {
            scoreManager.OnScoreUpdated -= UpdateDisplayScore;

            phaseHandler.OnGameStart -= EnableScoreDisplay;
            phaseHandler.OnGameEnd -= DisableScoreDisplay;
        }

        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            DisableScoreDisplay();
        }

        public void UpdateDisplayScore()
        {
            // enable score markers equal to points 
            for (int i = 0; i < scoreManager.PlayerScore; i++)
            {
                playerScoreMarkers[i].SetActive(true);
                if (i >= playerScoreMarkers.Length) return; // return if score somehow goes above score marker count 
            }

            for (int i = 0; i < scoreManager.EnemyScore; i++)
            {
                enemyScoreMarkers[i].SetActive(true);
                if (i >= enemyScoreMarkers.Length) return;
            }
        }

        // enables whole score display and resets it 
        private void EnableScoreDisplay()
        {
            // disables score markers
            for (int i = 0; i < playerScoreMarkers.Length; i++)
            {
                playerScoreMarkers[i].SetActive(false);
                enemyScoreMarkers[i].SetActive(false);
            }
            
            // disables rest of children 
            for (int i = 0; i < children.Length; i++)
            {
                children[i].SetActive(true);
            }
        }

        // disable score display
        private void DisableScoreDisplay()
        {
            for (int i = 0; i < children.Length; i++)
            {
                children[i].SetActive(false);
            }
        }
    }
}
