using UnityEngine;
using TMPro;
public class HUDController : MonoBehaviour
{
    public static HUDController instance;

    private void Awake() {
        instance = this;
        instance.UpdateFloorText(FloorManager.Instance.currentFloor.ToString());
    }

    [SerializeField] TMP_Text interactionText;
    [SerializeField] TMP_Text roundText;
    [SerializeField] TMP_Text floorText;

    
    public void EnableInteractionText(string text)
    {
        interactionText.text = text + " (F)";
        interactionText.gameObject.SetActive(true);
    }

    public void DisableInteractionText()
    {
        interactionText.gameObject.SetActive(false);
    }

    public void UpdateFloorText(string text)
    {
        floorText.text = "FLOOR: " + text;
    }

    public void UpdateRoundText(string text)
    {
        roundText.text = "ROUND: " + text;
    }
}
