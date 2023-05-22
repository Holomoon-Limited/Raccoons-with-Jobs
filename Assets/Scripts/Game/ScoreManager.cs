using UnityEngine;

[CreateAssetMenu(fileName = "Score Manager", menuName = "Game/New Score Manager", order = 0)]
public class ScoreManager : ScriptableObject
{
    public int PlayerScore { get; private set; }
    public int EnemyScore { get; private set; }

    public void IncreasePlayerScore()
    {
        PlayerScore++;
        if (PlayerScore >= 3)
        {

        }
    }

    public void IncreaseEnemyScore()
    {

    }

    public void ResetScores()
    {
        PlayerScore = 0;
        EnemyScore = 0;
    }
}