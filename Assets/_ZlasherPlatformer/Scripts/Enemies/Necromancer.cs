using UnityEngine;

public class Necromancer : MonoBehaviour, IInteractable
{
    [Header("Necromancer Name")]
    public string necromancerName;

    [Header("Dialogue Data")]
    public DialogueData dialogueData;

    public void Interact()
    {
        Debug.Log("NPC INTERATED WITH");
        ContextManager.Instance.dialogueManager.StartDialogue(necromancerName, dialogueData.dialogueLines);

    }

    public string GetInteractionPrompt()
    {
        return "Press E to talk to the necromancer " + necromancerName;
    }

   public void Die()
    {
        gameObject.SetActive(false);
    }
    public void ResetNecromancer()
    {
        gameObject.SetActive(true);
        
    }
}
