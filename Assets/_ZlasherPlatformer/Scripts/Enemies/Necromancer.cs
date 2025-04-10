using UnityEngine;

public class Necromancer : MonoBehaviour, IInteractable
{
    [Header("Necromancer Name")]
    public string necromancerName;

    public void Interact()
    {

    }

    public string GetInteractionPrompt()
    {
        return "Press E to talk to the necormancer " + necromancerName;
    }

}
