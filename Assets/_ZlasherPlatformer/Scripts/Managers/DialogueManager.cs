using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;


public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI talkingNPCText;
    public Sprite talkingNPCImage;


    [Header("Inut Settings")]
    public InputActionReference continueDialogueAction;

    [Header("Typing Settings")]
    [SerializeField] private float typingSpeed = 0.05f;
    private string[] currentLines;
    private int currentLineIndex;
    private bool isDialogueActive;

    private Coroutine typingCoroutine;
    private bool isTyping;


    public event Action OnDialogueEnd;

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
        if (!isDialogueActive)
        {
            return;
        }

        if (isTyping)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            dialogueText.text = currentLines[currentLineIndex - 1];
            isTyping = false;
        }
        else
        {
            DisplayNextLine();

        }
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

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeLine(currentLines[currentLineIndex]));
        currentLineIndex++;
    }


    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach( char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }


    private void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        OnDialogueEnd?.Invoke();
    }

    
    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}
   

