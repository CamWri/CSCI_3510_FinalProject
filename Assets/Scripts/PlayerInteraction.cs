using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float PlayerReach = 3f;
    Interactable currentInteractable;

    // Update is called once per frame
    void Update()
    {
        CheckInteraction();
        if(Input.GetKeyDown(KeyCode.F) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void CheckInteraction()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if(Physics.Raycast(ray, out hit, PlayerReach))
        {
            if(hit.collider.tag == "Interactable")
            {
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();

                if (newInteractable.enabled)
                {
                    SetNewCurrentInteractable(newInteractable);
                } else
                {
                    DisableCurrentInteraction();
                }
            } else
            {
                DisableCurrentInteraction();
            }
        } else
        {
            DisableCurrentInteraction();
        }
    }

    void SetNewCurrentInteractable(Interactable newInteactable)
    {
        currentInteractable = newInteactable;
        HUDController.Instance.EnableInteractionText(currentInteractable.message);
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
