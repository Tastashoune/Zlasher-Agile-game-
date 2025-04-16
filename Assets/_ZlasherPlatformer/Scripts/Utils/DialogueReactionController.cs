using System.Collections;
using System.IO.Pipes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueReactionController : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public Animator playerAnimator;
    public Necromancer necromancer;


    public float autoWalkDuration = 2f;
    public float walkSpeed = 2f;
    private bool isAutoWalking = false;
    private float autoWalkTimer = 0f;
    private Rigidbody2D rb;

    public CanvasGroup fadePanel;
    public float fadeDuration = 1f;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(isAutoWalking)
        {
            rb.linearVelocity = new Vector2(walkSpeed, rb.linearVelocityY);
        }
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
        playerAnimator.enabled = true;

        playerAnimator.Play("Attack", 0, 0f);

        StartCoroutine(DelayedDeath(0.5f));

        StartCoroutine(AutoWalkThenFade(1.2f));
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
        float timer = 0;

        while(timer<fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadePanel.alpha = alpha;
            yield return null;
        }
        fadePanel.alpha = 1f;

        SceneManager.LoadScene("MainScene");


    }



    private IEnumerator DelayedDeath(float delay)
    {
        yield return new WaitForSeconds(delay);
        necromancer.Die();
    }



}
