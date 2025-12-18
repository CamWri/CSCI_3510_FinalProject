using UnityEngine;

public class GameStatsManager : MonoBehaviour
{
    public static GameStatsManager Instance;

    [Header("Run Stats")]
    public int totalEnemiesKilled = 0;
    public int totalRoundsSurvived = 0;
    public int totalMoneyEarned = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void AddMoney(int amount)
    {
        totalMoneyEarned += amount;
    }

    public void AddEnemyKill()
    {
        totalEnemiesKilled++;
    }

    public void AddRoundSurvived()
    {
        totalRoundsSurvived++;
    }
}
