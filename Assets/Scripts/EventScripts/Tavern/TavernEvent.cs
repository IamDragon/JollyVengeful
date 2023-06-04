using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernEvent : MonoBehaviour
{
    [SerializeField] GameObject tavConvo_1;
    [SerializeField] GameObject exitTavern;    
    [SerializeField] Transform bartender;
    [SerializeField] GameObject q1;
    private Transform player;
    private GameObject questList;
    private bool setStage1;

    private void Start()
    {
        player = PlayerController.instance.transform;
        questList = QuestList.instance.gameObject;
        if (WorldState.tavernFirst)
        {
            StartConversation(tavConvo_1.GetComponent<Tester>());
            setStage1 = false;
            WorldState.tavernFirst = false;
        }
        DialogueManager.dialogueEvent += ProgressTavern;
        QuestList.QuestAccept += OnRuffians;
    }
    private void StartConversation(Tester convo)
    {
        if(convo.name==tavConvo_1.GetComponent<Tester>().name)
        {
            DeActivatePlayer();
            exitTavern.SetActive(false);
            tavConvo_1.GetComponent<Tester>().StartConvo();
        }
    }
    private void ProgressTavern()
    {
        if(!setStage1)
        {
            ActivatePlayer();
            setStage1 = true;
            exitTavern.SetActive(true);
            bartender.GetComponent<QuestGiver>().SetMarker();
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
    private void OnRuffians(Quest quest)
    {
        if (quest.name == q1.name)
            WorldState.harborRuffian = true;
    }
    private void OnDestroy()
    {
        DialogueManager.dialogueEvent -= ProgressTavern;
        QuestList.QuestAccept -= OnRuffians;
    }
}
