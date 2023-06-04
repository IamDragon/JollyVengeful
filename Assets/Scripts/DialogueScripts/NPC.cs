using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NPC : MonoBehaviour
{
    public Transform ChatBackGround;
    public Transform NPCCharacter;
    public DialogueSystem dialogueSystem;
    public bool active;
    public string nameNpc;

    [TextArea(5, 10)]
    public string[] sentences;

    private void Start()
    {
        active = true;
        dialogueSystem = FindObjectOfType<DialogueSystem>();
    }
    private void Update()
    {
        Vector3 Pos = Camera.main.WorldToScreenPoint(NPCCharacter.position);
        Pos.y += 75;
        ChatBackGround.position = Pos;
    }
    public virtual void OnTriggerStay(Collider other)
    {
        //this.gameObject.GetComponent<NPC>().enabled = true;
        FindObjectOfType<DialogueSystem>().EnterRangeOfNPC();
        if ((other.gameObject.tag == "Player") && Input.GetKeyDown(KeyCode.F) && active)
        {
            this.gameObject.GetComponent<NPC>().enabled = true;
            dialogueSystem.name = name;
            dialogueSystem.dialogueLines = sentences;
            FindObjectOfType<DialogueSystem>().NPCName();
        }
    }
    public virtual void OnTriggerExit()
    {
        FindObjectOfType<DialogueSystem>().OutOfRange();
        this.gameObject.GetComponent<NPC>().enabled = false;
    }
}
