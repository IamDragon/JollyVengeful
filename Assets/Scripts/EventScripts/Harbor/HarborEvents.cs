using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarborEvents : MonoBehaviour
{
    [SerializeField] GameObject q1;
    [SerializeField] GameObject harborConvo_1;
    [SerializeField] GameObject harborEnemy;
    GameObject questList;
    Transform player;

    private QuestList ql;
    private bool first;
    private void Awake()
    {
    }
    public void Start()
    {
        player = PlayerController.instance.transform;

        questList = QuestList.instance.gameObject;

        DialogueManager.dialogueEvent += HarborProgress;       
        ql = questList.GetComponent<QuestList>();
        first = false;
        if (WorldState.harborFirst)
        {
            StartConversation(harborConvo_1.GetComponent<Tester>());
            WorldState.harborFirst = false;
        }
        if(WorldState.harborRuffian)
        {
            harborEnemy.SetActive(true);
            WorldState.harborRuffian = false;
        }
    }
    private void StartConversation(Tester conver)
    {
        if (conver.name == harborConvo_1.GetComponent<Tester>().name)
        {
            DeActivatePlayer();
            harborConvo_1.GetComponent<Tester>().StartConvo();
        }
    }
    private void HarborProgress()
    {
        if (!first)
        {
            ActivatePlayer();
            ql.AddQuest(q1.GetComponent<MoveQuest>());
        }
    }
    private void ActivatePlayer()
    {
        player.GetComponent<PlayerController>().active = true;
    }
    private void DeActivatePlayer()
    {
        player.GetComponent<PlayerController>().active = false;
    }
    
    private void OnDestroy()
    {
        DialogueManager.dialogueEvent -= HarborProgress;
    }
}
