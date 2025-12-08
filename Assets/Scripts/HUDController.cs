using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance;

    private int totalMoneyCount;
    private int totalRounds;
    private int totalSkeletonsKilled;

    [Header("In Game UI")]
    //Durring Game Text
    [SerializeField] GameObject InGameStatsPanel;
    [SerializeField] TMP_Text interactionText;
    [SerializeField] TMP_Text roundText;
    [SerializeField] TMP_Text floorText;
    [SerializeField] TMP_Text moneyText;
    [SerializeField] TMP_Text weaponAmmoText;


    [Header("Healthbar")]
    [SerializeField] Slider healthBarSlider;
    [SerializeField] TMP_Text healthBarValueText;


    [Header("End Of Game UI")]
    //Stats Sheet Text
    [SerializeField] GameObject EndOfGameStatsPanel;
    [SerializeField] TMP_Text TotalMoneyEarnedText;
    [SerializeField] TMP_Text TotalRoundsSurvivedText;
    [SerializeField] TMP_Text TotalFloorsSurvivedText;
    [SerializeField] TMP_Text TotalSkeletonsKilledText;

    [Header("Hit Marker")]
    [SerializeField] GameObject hitMarker;  // assign in inspector
    [SerializeField] float hitMarkerDuration = 0.2f; // how long it shows

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

        EndOfGameStatsPanel.SetActive(false);
        hitMarker.SetActive(false);

        // Hide interaction text initially
        if (interactionText != null)
            interactionText.gameObject.SetActive(false);
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
    public void UpdateFloorText(int FloorNumber)
    {
        if (floorText != null)
            floorText.text = "FLOOR: " + FloorNumber.ToString();
    }

    public void UpdateRoundText(int RoundNumber)
    {
        if (roundText != null)
            totalRounds += 1;
            roundText.text = "ROUND: " + RoundNumber.ToString();
    }

    public void UpdateMoneyText(int NewMoneyTotal, int moneyChange, bool isIncrease)
    {
        if (moneyText != null)
            if(isIncrease)
                totalMoneyCount += moneyChange;
            moneyText.text = "$" + NewMoneyTotal.ToString();
    }

    public void SkeletonKilled()
    {
        totalSkeletonsKilled += 1;
    }

    public void ReturnToMainMenuAfterDelay(float delaySeconds)
    {
        StartCoroutine(ReturnToMainMenuRoutine(delaySeconds));
    }

    private IEnumerator ReturnToMainMenuRoutine(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        // Unfreeze before changing scenes
        Time.timeScale = 1f;

        DDOLCleaner.CleanDDOL();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("StartScene");
    }

    public void DeathUI()
    {
        TotalMoneyEarnedText.text = "Total Money Earned: $" + totalMoneyCount.ToString();
        TotalRoundsSurvivedText.text = "Total Rounds Survived is " + totalRounds.ToString();
        TotalSkeletonsKilledText.text = "Total Skeletons Killed is " + totalSkeletonsKilled.ToString();
        TotalFloorsSurvivedText.text = "Total Floors Visited is " + FloorManager.Instance.currentFloor.ToString();

        InGameStatsPanel.SetActive(false);
        EndOfGameStatsPanel.SetActive(true);

        // Freeze gameplay
        Time.timeScale = 0f;

        // Go back to main menu after 10 seconds of REAL TIME
        ReturnToMainMenuAfterDelay(10f);
    }

    public void UpdateHealthBar(float currentHP, float maxHP)
    {
        healthBarValueText.text = currentHP.ToString() + "/" + maxHP.ToString();

        healthBarSlider.value = currentHP;
        healthBarSlider.maxValue = maxHP;
    }

    public void UpdateWeaponText(int amount)
    {
        weaponAmmoText.text = amount.ToString();
    }

    private Coroutine hitMarkerRoutine;

    public void ShowHitMarker()
    {
        if (hitMarker == null) return;

        // Stop previous coroutine if already running
        if (hitMarkerRoutine != null)
            StopCoroutine(hitMarkerRoutine);

        hitMarker.SetActive(true);
        hitMarkerRoutine = StartCoroutine(HideHitMarkerAfterDelay());
    }

    private IEnumerator HideHitMarkerAfterDelay()
    {
        yield return new WaitForSeconds(hitMarkerDuration);
        hitMarker.SetActive(false);
    }
}