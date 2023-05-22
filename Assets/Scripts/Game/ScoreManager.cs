using UnityEngine;

[CreateAssetMenu(fileName = "Score Manager", menuName = "Game/New Score Manager", order = 0)]
public class ScoreManager : ScriptableObject
{
    public int PlayerScore { get; private set; }
    public int EnemyScore { get; private set; }

    public bool GameOver => (PlayerScore >= 3) || (EnemyScore >= 3);

    public void IncreasePlayerScore()
    {
        PlayerScore++;
        if (PlayerScore >= 3)
        {
            Debug.Log("Player wins!");
        }
    }

    public void IncreaseEnemyScore()
    {
        EnemyScore++;
        if (EnemyScore >= 3)
        {
            Debug.Log("Enemy wins!");
        }
    }

    public void ResetScores()
    {
        PlayerScore = 0;
        EnemyScore = 0;
    }
}