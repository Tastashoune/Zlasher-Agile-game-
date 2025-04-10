using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Zlasher/DialogueData")]
public class DialogueData : ScriptableObject
{

    [Header("NPC Settings")]

    public string npcName;

    [Header("Dialogue")]
    [TextArea(3, 10)]

    public string[] dialogueLines;


}
