using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float PlayerReach = 3f;
    Interactable currentInteractable;

    void Update()
    {
        CheckInteraction();

        // Primary interact (F)
        if (Input.GetKeyDown(KeyCode.F) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }

        // Secondary interact (E)
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.SecondaryInteract();
        }
    }

    void CheckInteraction()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out hit, PlayerReach))
        {
            if (hit.collider.tag == "Interactable")
            {
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();

                if (newInteractable != null && newInteractable.enabled)
                {
                    SetNewCurrentInteractable(newInteractable);
                }
                else
                {
                    DisableCurrentInteraction();
                }
            }
            else
            {
                DisableCurrentInteraction();
            }
        }
        else
        {
            DisableCurrentInteraction();
        }
    }

    void SetNewCurrentInteractable(Interactable newInteractable)
    {
        currentInteractable = newInteractable;

        // Build UI text with keys
        if (currentInteractable.hasSecondaryInteraction)
        {
            HUDController.Instance.EnableInteractionText(
                $"{currentInteractable.message} (F)\n{currentInteractable.secondaryMessage} (E)"
            );
        }
        else
        {
            HUDController.Instance.EnableInteractionText(
                $"{currentInteractable.message} (F)"
            );
        }
    }

    void DisableCurrentInteraction()
    {
        HUDController.Instance.DisableInteractionText();
        if (currentInteractable)
        {
            currentInteractable = null;
        }
    }
}
