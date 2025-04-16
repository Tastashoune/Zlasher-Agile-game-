using System.Collections;
using System.IO.Pipes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI.Table;

public class DialogueReactionController : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public Animator playerAnimator;
    public Necromancer necromancer;
    public PlayerMovementScript playerMovement;
    public MusicManager musicManager;
    public TextMeshProUGUI fadeText;

    public float autoWalkDuration = 2f;
    public float walkSpeed = 2f;
    private bool isAutoWalking = false;
    private float autoWalkTimer = 0f;
    private Rigidbody2D rb;

    public CanvasGroup fadePanel;
    public float fadeDuration = 1f;
    //private float typingSpeed = 0.02f;

    [SerializeField]
    float delayAfterFade; // best time is 8.5 after testing;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //musicManager = GetComponent<MusicManager>();
        musicManager = MusicManager.instance;
        musicManager.audioSource.Stop();
        if(fadePanel != null)
        {
            fadePanel.alpha = 0f;
        }
        if (playerMovement == null)
            playerMovement = GetComponent<PlayerMovementScript>();
        if(fadeText != null)
        {
            fadeText.alpha = 0f;
        }

    }

    private void Update()
    {
        if(isAutoWalking && rb !=null)
        {
            //rb.linearVelocity = new Vector2(walkSpeed, rb.linearVelocity.y);
            Vector2 targetPosition = rb.position + Vector2.right * walkSpeed * Time.deltaTime;
            rb.MovePosition(targetPosition);
        }
        //Debug.Log("Auto-walking... position: " + transform.position);
    }

    private void OnEnable()
    {
        dialogueManager.OnDialogueEnd += HandleDialogueEnd;
    }

    private void OnDisable()
    {
        dialogueManager.OnDialogueEnd -= HandleDialogueEnd;
    }

    private void HandleDialogueEnd()
    {
        if (playerMovement != null)
            playerMovement.enabled = false;
        playerAnimator.enabled = true;

        playerAnimator.Play("Attack", 0, 0f);

        StartCoroutine(DelayedDeath(0.2f));

        StartCoroutine(AutoWalkThenFade(1.0f));

        musicManager.audioSource.Play();

    }

    private IEnumerator AutoWalkThenFade(float delay)
    {
        
        yield return new WaitForSeconds(delay);

        isAutoWalking = true;
        autoWalkTimer = autoWalkDuration;

        yield return new WaitForSeconds(autoWalkDuration);

        isAutoWalking = false;

        StartCoroutine(FadeAndLoadScene());
    }


    public IEnumerator FadeAndLoadScene()
    {
        float timer = 0f;

        while(timer<fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadePanel.alpha = alpha;
            yield return null;
        }
        fadePanel.alpha = 1f;

        StartCoroutine(ShowFadeTextSequence());

        yield return new WaitForSeconds(delayAfterFade);

        SceneManager.LoadScene("MainScene");


    }


    private IEnumerator ShowFadeTextSequence()
    {
        if (fadeText == null) yield break;

        // The lines to display
        string[] lines = new string[]
        {
        "Did that schmuck just call me a soulles husk?",
        "I am FILLED with souls and I need more !",
        "Now....."
        };

        foreach (string line in lines)
        {

            fadeText.text = line;

            yield return StartCoroutine(FadeInText());

            if(line == "Now.....")
            {
                yield return StartCoroutine(PulseText(line));
            }

            yield return new WaitForSeconds(2f);

            yield return StartCoroutine(FadeOutText());



            //yield return StartCoroutine(FadeInText());
            //yield return StartCoroutine(TypeText(line));
            //if (line == "Now.....")
            //{
            //    yield return StartCoroutine(PulseText(line));
            //}
            //yield return new WaitForSeconds(1.0f); 
            //yield return StartCoroutine(FadeOutText());
        }

        //if (fadeText.text == null) yield break;

        //fadeText.text = "";
        //fadeText.alpha = 1f;

        //yield return new WaitForSeconds(0.5f);
        //fadeText.text = "Did that schmuck just called me a soulles husk?";

        //yield return new WaitForSeconds(1.5f);
        //fadeText.text = "I am FILLED with souls! And I want more!";

        //yield return new WaitForSeconds(1.5f);
        //fadeText.text = "Now.....";

        //yield return new WaitForSeconds(1.5f);
        //fadeText.alpha = 0f;




    }


    private IEnumerator DelayedDeath(float delay)
    {
        yield return new WaitForSeconds(delay);
        necromancer.Die();
    }

    private IEnumerator TypeText(string fullText)
    {
        fadeText.text = "";
        for (int i = 0; i < fullText.Length; i++)
        {
            fadeText.text += fullText[i];
            yield return new WaitForSeconds(0.025f);
            //foreach(char c in fullText)
            //{
            //    fadeText.text += c;
            //    yield return new WaitForSeconds(typingSpeed);
            //}
        }
        fadeText.text = fullText;
    }

    private IEnumerator FadeInText()
    {
        float duration = 0.5f;
        float timer = 0f;

        Color color = fadeText.color;
        color.a = 0f;
        fadeText.color = color;

        while(timer < duration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / duration);
            fadeText.color = color;
            yield return null;
        }
        color.a = 1f;
        fadeText.color = color;
    }


    private IEnumerator FadeOutText()
    {
        float duration = 0.5f;
        float timer = 0f;

        Color color = fadeText.color;
        color.a = 1f;
        fadeText.color = color;

        while(timer < duration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, timer / duration);
            fadeText.color = color;
            yield return null;
        }

        color.a = 0f;
        fadeText.color = color;
    }

    private IEnumerator PulseText(string text)
    {
        Color originalColor = fadeText.color;
        fadeText.color = Color.red;

        float pulseDuration = 0.8f;
        float pulseTimer = 0f;
        bool pulsingOut = false;

        while(pulseTimer < pulseDuration)
        {
            pulseTimer += Time.deltaTime;
            float lerpValue = Mathf.PingPong(pulseTimer, 0.5f) * 2f;

            fadeText.color = Color.Lerp(Color.red, originalColor, lerpValue);

            yield return null;


        }
        fadeText.color = originalColor;
    }


}
