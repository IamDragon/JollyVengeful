using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] private GameObject createQuest;

    [Header("Inputtable")]
    [SerializeField] GameObject questLog;
    private GameObject questCanvas;
    private Text questTitle;
    private Text questDescription;
    [Header("RewardInput")]
    private Text goldReward;
    private Text expReward;
    private Text enemyType;
    private Text enemyAmount;
    private Button acceptButton;
    private Button declineButton;
    [SerializeField] Transform thisNPC;
    private GameObject questList;
    private QuestMarker mark;
    public bool active { get; set; }

    public KillQuest Quest { get; set; }
    public bool QuestWindow { get; set; }
    public bool AssignedQuest { get; set; }
    public bool Helped { get; set; }
    public string[] AfterQuestTalk { get; set; }
    private QuestList ql;
    private bool playerInRange = false;

    private void Start()
    {
        questList = QuestList.instance.gameObject;
        questCanvas = GameObject.Find("QuestCanvas");
        questTitle = GameObject.Find("QuestTitle").GetComponent<Text>();
        questDescription = GameObject.Find("QuestDescription").GetComponent<Text>();
        goldReward = GameObject.Find("GoldReward").GetComponent<Text>();
        expReward = GameObject.Find("ExpReward").GetComponent<Text>();
        enemyType = GameObject.Find("EnemyType").GetComponent<Text>();
        enemyAmount = GameObject.Find("EnemyAmount").GetComponent<Text>();
        acceptButton = GameObject.Find("AcceptButton").GetComponent<Button>();
        declineButton = GameObject.Find("DeclineButton").GetComponent<Button>();
        ql = questList.GetComponent<QuestList>();        
        QuestWindow = false;
        mark = thisNPC.GetComponent<QuestMarker>();
        AssignQuest();
    }

    private void Update()
    {
        ShowQuestsToPlayer();
    }

    private void ShowQuestsToPlayer()
    {
        if ((playerInRange && Input.GetKeyDown(KeyCode.F) && active))
        {
            if (!AssignedQuest && !Helped)
            {
                GameManager.instance.SetShowCursor(true);
                AssignQuest();
                GetQuestInfo();
                QuestWindow = true;
                questCanvas.GetComponent<Canvas>().enabled = true;
                acceptButton.onClick.AddListener(this.AcceptQuest);
                declineButton.onClick.AddListener(DisableQuestCanvas);
            }
            else if (AssignedQuest && !Helped)
            {
                CheckQuest();
            }           
        }
    }

    public /*override*/ void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
            DisableQuestCanvas();
        }
    }

    void DisableQuestCanvas()
    {
        questCanvas.GetComponent<Canvas>().enabled = false;
        GameManager.instance.SetShowCursor(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    void AssignQuest()
    {                             
        Quest = createQuest.gameObject.GetComponent<KillQuest>();        
    }
    public void AcceptQuest()
    {
        acceptButton.onClick = null;
        questCanvas.GetComponent<Canvas>().enabled = false;
        if(!AssignedQuest)
        {
            ql.AddQuest(Quest);
            AssignedQuest = true;
            createQuest.transform.parent = GameManager.instance.transform;
            mark.TriggerTake();
        }       
        DisableQuestCanvas();
    }
    public void DeclineQuest()
    {
        questCanvas.GetComponent<Canvas>().enabled = false;
        DisableQuestCanvas();
    }

    void CheckQuest()
    {
        if(Quest.questCompleted)
        {
            questLog.GetComponent<QuestList>().QuestCompleted(createQuest.GetComponent<KillQuest>());
            Helped = true;
            AssignedQuest = false;
            mark.TriggerTurnIn();        
            DisableQuestCanvas();
        }        
    }
    void GetQuestInfo()
    {
        questTitle.text = Quest.questName;
        questDescription.text = Quest.description;
        expReward.text = Quest.experienceReward.ToString();
        goldReward.text = Quest.goldReward.ToString();
        enemyType.text = Quest.enemyType;
        enemyAmount.text = Quest.requiredAmount.ToString();
    }    

    public void SetMarker()
    {
        active = true;
        mark.SetMarker();
    }
}
