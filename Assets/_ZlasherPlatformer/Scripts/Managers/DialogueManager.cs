using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class DialogueManager : ContextManager
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI talkingNPCText;
    public Image talkingNPCImage;


    [Header("Inut Settings")]
    public InputActionReference continueDialogueAction;

    private string[] currentLines;
    private int currentLineIndex;
    private bool isDialogueActive;

    private void Awake()
    {
        dialoguePanel.SetActive(false);
        
        if(continueDialogueAction !=null)
        {
            continueDialogueAction.action.started += OnContinueDialogueInput;

        }
    }

    private void OnEnable()
    {
        if(continueDialogueAction != null)
        {
            continueDialogueAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if(continueDialogueAction != null)
        {
            continueDialogueAction.action.Disable();
        }

    }

    private void OnDestroy()
    {
        if(continueDialogueAction != null)
        {
            continueDialogueAction.action.started -= OnContinueDialogueInput;
        }
    }

    private void OnContinueDialogueInput(InputAction.CallbackContext context)
    {
        if(!isDialogueActive)
        {
            return;
        }
        DisplayNextLine();
    }

    private void StartDialogue(string SpeakerName, string[] lines)
    {
        currentLines = lines;
        currentLineIndex = 0;
        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        talkingNPCText.text = SpeakerName;

        DisplayNextLine();

    }

    private void DisplayNextLine()
    {
        if(currentLineIndex >= currentLines.Length)
        {
            EndDialogue();
            return;
        } 
    }


    private void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
    }

    
    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}
   

