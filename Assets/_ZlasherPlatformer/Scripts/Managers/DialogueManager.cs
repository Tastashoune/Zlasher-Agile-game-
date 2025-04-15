using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;


public class DialogueManager : MonoBehaviour
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

    public void StartDialogue(string SpeakerName, string[] lines)
    {
        Debug.Log("STARTING DIALOGUE");
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
        dialogueText.text = currentLines[currentLineIndex];
        currentLineIndex++;
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
   

