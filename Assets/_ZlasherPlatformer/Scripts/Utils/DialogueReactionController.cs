using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Coffee.UIExtensions;


public class DialogueReactionController : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public Animator playerAnimator;
    public Necromancer necromancer;
    public PlayerMovementScript playerMovement;
    public MusicManager musicManager;
    public TextMeshProUGUI fadeText;
    public GameObject bloodDripPrefab;
    public GameObject bloodFX;

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

        yield return StartCoroutine(ShowFadeTextSequence());

        //yield return new WaitForSeconds(0.1f);

        SceneManager.LoadScene("MainScene");


    }


    private IEnumerator ShowFadeTextSequence()
    {
        {
            if (fadeText == null) yield break;

            // Store original styles
            float originalFontSize = fadeText.fontSize;
            Color originalColor = fadeText.color;
            Vector2 originalPosition = fadeText.rectTransform.anchoredPosition;

            // The lines to display
            string[] lines = new string[]
            {
        "Ahhahahahahah what an idiot! Raise and control me?",
        "Man was begging to be first on the list",
        "Now....."
            };

            foreach (string line in lines)
            {
                fadeText.text = line;

                if (line == "Now.....")
                {
                    // Set special styles
                    fadeText.fontSize = 140f;
                    fadeText.color = Color.red;
                    fadeText.rectTransform.anchoredPosition = Vector2.zero;

                    yield return StartCoroutine(FadeInText());

                    //Apply shake using DOTween                             ----------replaced by below for increased intensity

                    //fadeText.rectTransform.DOShakeAnchorPos(
                    //    duration: 4f,
                    //    strength: new Vector2(10f, 5f),
                    //    vibrato: 40,
                    //    randomness: 90,
                    //    snapping: false,
                    //    fadeOut: true
                    //);

                    //yield return StartCoroutine(ShakeTextAccelerating(10f, 10f, 10f));  //no DOTWeen coroutine

                    //yield return StartCoroutine(ShakeTextWithAcceleration()); // not dynaically procedural stages

                    //if (bloodFX != null)
                    //{
                    //    bloodFX.SetActive(true);

                    //    // Optional: Restart the animation from the beginning
                    //    Animator anim = bloodFX.GetComponent<Animator>();
                    //    if (anim != null)
                    //    {
                    //        anim.Play("BloodFXAnim", 0, 0f); // Replace with your actual clip name
                    //    }
                    //}

                    yield return StartCoroutine(ShakeTextWithAccelerationDynamic());
                    

                    yield break;
                    //yield return new WaitForSeconds(2f);

                    //yield return StartCoroutine(FadeOutText());



                    // Revert styles after
                    fadeText.fontSize = originalFontSize;
                    fadeText.color = originalColor;
                    fadeText.rectTransform.anchoredPosition = originalPosition;
                }
                else
                {
                    // Standard display
                    fadeText.fontSize = originalFontSize;
                    fadeText.color = originalColor;
                    fadeText.rectTransform.anchoredPosition = originalPosition;

                    yield return StartCoroutine(FadeInText());
                    yield return new WaitForSeconds(1.5f);
                    yield return StartCoroutine(FadeOutText());
                }
            }









            //if (fadeText == null) yield break;

            //// The lines to display
            //string[] lines = new string[]
            //{
            //"Did that schmuck just call me a soulles husk?",
            //"I am FILLED with souls and I need more !",
            //"Now....."
            //};

            //foreach (string line in lines)
            //{
            //fadeText.text = line;

            //yield return StartCoroutine(FadeInText());

            //if(line == "Now.....")
            //{
            //    yield return StartCoroutine(PulseText(line));
            //}

            //yield return new WaitForSeconds(2f);

            //yield return StartCoroutine(FadeOutText());



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
        fadeText.DOColor(Color.red, 0.3f);

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

    private IEnumerator ShakeTextAccelerating(float totalDuration, float maxFrequency, float shakeStrength)
    {
        float elapsed = 0f;
        Vector2 originalPos = fadeText.rectTransform.anchoredPosition;

        while (elapsed < totalDuration)
        {
            // Increase frequency over time
            float progress = elapsed / totalDuration;
            float frequency = Mathf.Lerp(2f, maxFrequency, progress); // from 2 Hz to maxFrequency

            // Calculate how fast the shake updates (inversely related to frequency)
            float timeStep = 1f / frequency;

            // Random offset
            float offsetX = Random.Range(-shakeStrength, shakeStrength);
            float offsetY = Random.Range(-shakeStrength, shakeStrength);

            fadeText.rectTransform.anchoredPosition = originalPos + new Vector2(offsetX, offsetY);

            elapsed += timeStep;
            yield return new WaitForSeconds(timeStep);
        }

        // Reset position
        fadeText.rectTransform.anchoredPosition = originalPos;
    }


    private IEnumerator ShakeTextWithAcceleration()  //- ------DOTween version with chained shakes
    {
        Vector2 originalPos = fadeText.rectTransform.anchoredPosition;

        // Define shake stages (duration, vibrato, strength)
        var shakeStages = new (float duration, float strength, int vibrato)[]
        {
        (6f, 10f, 10),
        (6f, 14f, 20),
        (6f, 18f, 40),
        (6f, 22f, 80)
        };

        foreach (var stage in shakeStages)
        {
            Tween shakeTween = fadeText.rectTransform.DOShakeAnchorPos(
                duration: stage.duration,
                strength: new Vector2(stage.strength, stage.strength),
                vibrato: stage.vibrato,
                randomness: 180,
                snapping: false,
                fadeOut: false
            );

            yield return shakeTween.WaitForCompletion();
        }

        // Reset to original position
        fadeText.rectTransform.anchoredPosition = originalPos;
    }

    private IEnumerator ShakeTextWithAccelerationDynamic(         //Even mroe dotween customization
    int stages = 8,
    float baseDuration = 0.5f,
    float baseStrength = 5f,
    int baseVibrato = 5,
    float strengthIncrement = 5f,
    int vibratoIncrement = 20
)
    {
        Vector2 originalPos = fadeText.rectTransform.anchoredPosition;

        for (int i = 0; i < stages; i++)
        {
            float duration = baseDuration;
            float strength = baseStrength + i * strengthIncrement;
            int vibrato = baseVibrato + i * vibratoIncrement;

            Tween shakeTween = fadeText.rectTransform.DOShakeAnchorPos(
                duration: duration,
                strength: new Vector2(strength, strength),
                vibrato: vibrato,
                randomness: 90,
                snapping: false,
                fadeOut: false
            );

            yield return shakeTween.WaitForCompletion();
        }


        // Reset position
        fadeText.rectTransform.anchoredPosition = originalPos;
    }

    //private void SpawnBloodDrip()
    //{
    //    // 1. Find the FX canvas
    //    Canvas fxCanvas = GameObject.Find("FXCanvas")?.GetComponent<Canvas>();
    //    if (fxCanvas == null)
    //    {
    //        Debug.LogWarning("FXCanvas not found in the scene!");
    //        return;
    //    }

    //    // 2. Instantiate the particle effect prefab
    //    GameObject bloodTrail = Instantiate(bloodDripPrefab);

    //    // 3. Set it as a child of the canvas WITHOUT keeping world position
    //    bloodTrail.transform.SetParent(fxCanvas.transform, worldPositionStays: false);

    //    // 4. Get the canvas's RectTransform to calculate top-center
    //    RectTransform canvasRect = fxCanvas.GetComponent<RectTransform>();
    //    Vector2 topCenter = new Vector2(0f, canvasRect.rect.height / 2f); // (0, height/2) = top center

    //    // 5. Position and scale the particle properly in UI space
    //    RectTransform particleRect = bloodTrail.GetComponent<RectTransform>();
    //    if (particleRect != null)
    //    {
    //        particleRect.anchoredPosition = topCenter;        // Align to top-center
    //        particleRect.localScale = Vector3.one;            // Make sure it's normal size
    //        particleRect.localRotation = Quaternion.identity; // Reset rotation
    //    }

    //    // 6. OPTIONAL: Make sure it's rendering on top
    //    UIParticle uiParticle = bloodTrail.GetComponent<UIParticle>();
    //    if (uiParticle == null)
    //    {
    //        Debug.LogWarning("Your particle prefab is missing a UIParticle component!");
    //    }



        //----------------------------------good position in viewport------------vvvvvvvvv---------------------------
        //    {
        //        Canvas fxCanvas = GameObject.Find("FXCanvas")?.GetComponent<Canvas>();
        //        if (fxCanvas == null)
        //        {
        //            Debug.LogWarning("FXCanvas not found in the scene!");
        //            return;
        //        }

        //        // 1. Choose a target position (world or UI element)
        //        Vector3 worldPosition = new Vector3(0, 20, 0); // Example world pos (you can target the player, or center screen)

        //        // Convert world to screen point
        //        Vector3 screenPoint = Camera.main.WorldToScreenPoint(worldPosition);

        //        // Convert screen point to canvas-local position
        //        Vector2 anchoredPos;
        //        RectTransformUtility.ScreenPointToLocalPointInRectangle(
        //            fxCanvas.transform as RectTransform,
        //            screenPoint,
        //            fxCanvas.worldCamera,
        //            out anchoredPos
        //        );

        //        // 2. Instantiate the prefab
        //        GameObject bloodTrail = Instantiate(bloodDripPrefab);

        //        // 3. Parent it to canvas (don't keep world pos!)
        //        bloodTrail.transform.SetParent(fxCanvas.transform, worldPositionStays: false);

        //        // 4. Set its anchored position to match the canvas
        //        RectTransform rect = bloodTrail.GetComponent<RectTransform>();
        //        if (rect != null)
        //        {
        //            rect.anchoredPosition = anchoredPos;
        //            rect.localScale = Vector3.one; // Fix scaling issues
        //            rect.localRotation = Quaternion.identity;
        //        }
        //        else
        //        {
        //            // Fallback if no RectTransform
        //            bloodTrail.transform.localPosition = anchoredPos;
        //            bloodTrail.transform.localScale = Vector3.one;
        //            bloodTrail.transform.localRotation = Quaternion.identity;
        //        }

        //        // Optional: set sorting layer
        //        var renderer = bloodTrail.GetComponent<ParticleSystemRenderer>();
        //        if (renderer != null)
        //        {
        //            renderer.sortingLayerName = "Front";
        //            renderer.sortingOrder = 500;
        //        }
        //    }
        //-------------------------------------------------------------------- good position in viewport-----------------------------^^^^^^^^
        //{
        //    Canvas fxCanvas = GameObject.Find("FXCanvas")?.GetComponent<Canvas>();
        //    if (fxCanvas == null)
        //    {
        //        Debug.LogWarning("FXCanvas not found in the scene!");
        //        return;
        //    }

        //    // Instantiate as a child of canvas
        //    GameObject bloodTrail = Instantiate(bloodDripPrefab, fxCanvas.transform);

        //    // Reset transform
        //    bloodTrail.transform.localPosition = Vector3.zero;
        //    bloodTrail.transform.localScale = Vector3.one;

        //    // (Optional) Align with center of screen
        //    RectTransform rect = bloodTrail.GetComponent<RectTransform>();
        //    if (rect != null)
        //    {
        //        rect.anchoredPosition = Vector2.zero;
        //    }
        //}
        //Canvas fxCanvas = GameObject.Find("FXCanvas")?.GetComponent<Canvas>();
        //if(fxCanvas == null)
        //{
        //    Debug.LogWarning("FXCanvas not found in the scene!");
        //    return;
        //}

        //Vector3 spawnPosition = new Vector3(0, 0, 0);
        //GameObject bloodTrail = Instantiate(bloodDripPrefab, spawnPosition, Quaternion.identity);

        //bloodTrail.transform.SetParent(fxCanvas.transform, false);

        //bloodTrail.transform.localPosition = new Vector3(0f, 20f, 0f);

        //var renderer = bloodTrail.GetComponent<ParticleSystemRenderer>();
        //if(renderer != null)
        //{
        //    renderer.sortingLayerName = "Front";
        //    renderer.sortingOrder = 500;
        //}



        //if (bloodDripPrefab == null) return;


        //GameObject bloodTrail = Instantiate(bloodDripPrefab);

        //bloodTrail.transform.position = new Vector3(0f, 20f, -1f);

        //var renderer = bloodTrail.GetComponent<ParticleSystemRenderer>();
        //if (renderer != null)
        //{
        //    renderer.sortingLayerName = "BloodFX";
        //    renderer.sortingOrder = 100;
        //}


        //Vector3 screenTopCenter = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, Camera.main.nearClipPlane + 5f));
        //screenTopCenter.z = 0f;
        //GameObject bloodTrail = Instantiate(bloodDripPrefab, screenTopCenter, Quaternion.identity);

        //Vector3 spawnPosition = new Vector3(0, 5, 0);
        //GameObject bloodTrail = Instantiate(bloodDripPrefab, spawnPosition, Quaternion.identity);
        //bloodTrail.transform.SetParent(this.transform, false);

    //}

    //public void OnParticleSystemStopped()
    //{
    //    Destroy(gameObject);
    //}

}
