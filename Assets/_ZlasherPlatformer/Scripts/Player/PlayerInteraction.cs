using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("UI References")]
    public GameObject interactionPromptPanel;
    public TextMeshProUGUI interactionPromptText;

    private IInteractable currentInteractable;

    private void Awake()
    {
        if(interactionPromptPanel != null)
        {
            interactionPromptPanel.SetActive(false);
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if(currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

        currentInteractable = interactable;

        if(currentInteractable != null && interactionPromptPanel != null)
        {
            interactionPromptPanel.SetActive(true);
            interactionPromptText.text = currentInteractable.GetInteractionPrompt();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if(currentInteractable == interactable)
        {
            currentInteractable = null;
            if(interactionPromptPanel != null)
            {
                interactionPromptPanel.SetActive(false);
            }

        }
    }

}
