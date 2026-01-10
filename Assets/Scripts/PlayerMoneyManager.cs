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
        if (HUDController.Instance != null)
            AddMoney(0);
        else
            Debug.LogWarning("HUDController.Instance not found!");
    }

    public void AddMoney(int amount)
    {
        moneyCount += amount;
        HUDController.Instance.UpdateMoneyText(moneyCount, amount, true);
    }

    // Returns true if money was successfully spent
    public bool SpendMoney(int amount)
    {
        if (moneyCount >= amount)
        {
            moneyCount -= amount;
            HUDController.Instance.UpdateMoneyText(moneyCount, amount,  false);
            return true;
        }
        return false;
    }
}