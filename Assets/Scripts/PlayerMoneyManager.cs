using UnityEngine;

public class PlayerMoneyManager : MonoBehaviour
{
    public static PlayerMoneyManager Instance;

    public int moneyCount = 3000;

    private void Awake()
    {
        // Simple singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Safe: HUDController.Instance should exist by now
        if (HUDController.Instance != null)
            HUDController.Instance.UpdateMoneyText(moneyCount.ToString());
        else
            Debug.LogWarning("HUDController.Instance not found!");
    }

    public void AddMoney(int amount)
    {
        moneyCount += amount;
        HUDController.Instance.UpdateMoneyText(moneyCount.ToString());
    }

    // Returns true if money was successfully spent
    public bool SpendMoney(int amount)
    {
        if (moneyCount >= amount)
        {
            moneyCount -= amount;
            HUDController.Instance.UpdateMoneyText(moneyCount.ToString());
            return true;
        }
        return false;
    }
}