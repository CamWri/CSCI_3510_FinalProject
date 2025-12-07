using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance;

    [SerializeField] TMP_Text interactionText;
    [SerializeField] TMP_Text roundText;
    [SerializeField] TMP_Text floorText;
    [SerializeField] TMP_Text moneyText;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Hide interaction text initially
        if (interactionText != null)
            interactionText.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (FloorManager.Instance != null)
            UpdateFloorText(FloorManager.Instance.currentFloor.ToString());

        if (PlayerMoneyManager.Instance != null)
            UpdateMoneyText(PlayerMoneyManager.Instance.moneyCount.ToString());
    }

    // Interaction text
    public void EnableInteractionText(string text)
    {
        if (interactionText != null)
        {
            interactionText.text = text + " (F)";
            interactionText.gameObject.SetActive(true);
        }
    }

    public void DisableInteractionText()
    {
        if (interactionText != null)
            interactionText.gameObject.SetActive(false);
    }

    // HUD updates
    public void UpdateFloorText(string text)
    {
        if (floorText != null)
            floorText.text = "FLOOR: " + text;
    }

    public void UpdateRoundText(string text)
    {
        if (roundText != null)
            roundText.text = "ROUND: " + text;
    }

    public void UpdateMoneyText(string text)
    {
        if (moneyText != null)
            moneyText.text = "$" + text;
    }
}