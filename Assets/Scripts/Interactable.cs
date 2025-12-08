using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Primary Interaction")]
    public string message = "Interact";
    public UnityEvent onInteraction;

    [Header("Secondary Interaction (Optional)")]
    public bool hasSecondaryInteraction = false;
    public string secondaryMessage = "Secondary Interact";
    public UnityEvent onSecondaryInteraction;

    public void Interact()
    {
        onInteraction?.Invoke();
    }

    public void SecondaryInteract()
    {
        if (!hasSecondaryInteraction) return;
        onSecondaryInteraction?.Invoke();
    }
}
