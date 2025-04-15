using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("UI References")]
    public GameObject interactionPromptPanel;
    public TextMeshProUGUI interactionPromptText;

    private IInteractable currentInteractable;
    [SerializeField]
    private PlayerInput playerInput;

    private void Awake()
    {
        if(interactionPromptPanel != null)
        {
            interactionPromptPanel.SetActive(false);
        }
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentInteractable?.Interact();
            interactionPromptPanel.SetActive(false);
        }

    }

    public void RegisterInputActions()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            playerInput.actions["Interact"].started += OnInteract;
        }
        else
        {
            Debug.LogError("PlayerInput is null");
        }
    }


    public void UnregisterInputActions()
    {
        
        if (Input.GetKeyUp(KeyCode.E))
        {
            playerInput.actions["Interact"].started -= OnInteract;
        }
    }


    public void OnInteract(InputAction.CallbackContext context)
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
