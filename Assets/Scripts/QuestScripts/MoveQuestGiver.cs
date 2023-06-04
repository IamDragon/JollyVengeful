using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MoveQuestGiver : NPC
{
    public bool assignedQuest { get; set; }
    public bool helped { get; set; }
    public GameObject player;
    [TextArea(5, 10)]
    public string[] duringQuestTalk;
    public string[] afterQuestTalk { get; set; }
    public GameObject questCanvas;
    public Text questTitle;
    public Text questDescription;
    public Text goldReward;
    public Text expReward;
    public Text enemyType;
    [SerializeField]
    private GameObject createQ;

    public bool questWindow { get; set; }
    public MoveQuest quest { get; set; }
    private void Start()
    {
        afterQuestTalk = sentences;
        questWindow = false;
    }
    public override void OnTriggerStay(Collider other)
    {
        this.gameObject.GetComponent<NPC>().enabled = true;
        FindObjectOfType<DialogueSystem>().EnterRangeOfNPC();
        if ((other.gameObject.tag == "Player") && Input.GetKeyDown(KeyCode.F))
        {
            this.gameObject.GetComponent<NPC>().enabled = true;


            if (!assignedQuest && !helped)
            {
                AssignQuest();
                GetQuestInfo();
                Debug.Log("Enter");
                questWindow = true;
                questCanvas.GetComponent<Canvas>().enabled = true;
            }
            else if (assignedQuest && !helped)
            {
                CheckQuest();
            }
            else
            {
                dialogueSystem.nameFor = name;
                dialogueSystem.dialogueLines = afterQuestTalk;
            }
            FindObjectOfType<DialogueSystem>().NPCName();
        }
    }
    public override void OnTriggerExit()
    {
        base.OnTriggerExit();
        Debug.Log("Exit");
        questCanvas.GetComponent<Canvas>().enabled = false;
    }

    void AssignQuest()
    {
        quest = createQ.gameObject.GetComponent<MoveQuest>();
    }
    public void AcceptQuest()
    {
        questCanvas.GetComponent<Canvas>().enabled = false;
        player.GetComponent<QuestList>().AddQuest(quest);

        assignedQuest = true;
        OnTriggerExit();
    }
    public void DeclineQuest()
    {
        questCanvas.GetComponent<Canvas>().enabled = false;
        OnTriggerExit();
    }

    void CheckQuest()
    {
        if (quest.questCompleted)
        {
            Debug.Log("here");
            player.GetComponent<QuestList>().QuestCompleted(quest);
            helped = true;
            assignedQuest = false;
            OnTriggerExit();
        }
        else
        {
            dialogueSystem.nameFor = name;
            dialogueSystem.dialogueLines = duringQuestTalk;
        }
    }
    void GetQuestInfo()
    {
        questTitle.text = quest.questName;
        questDescription.text = quest.description;
        expReward.text = quest.experienceReward.ToString();
        goldReward.text = quest.goldReward.ToString();
        enemyType.text = quest.moveObject.ToString();
    }
}
