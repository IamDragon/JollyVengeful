using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialEvent : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform willy;
    [SerializeField] GameObject questLog;
    [SerializeField] GameObject quest1, quest2, quest3, quest4;
    [SerializeField] Transform g1, g2, g3;
    [SerializeField] GameObject tutorial;
    [SerializeField] Text tutorialText;
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject convo;
    [SerializeField] GameObject tutConvo_1, tutConvo_2, tutConvo_3;
    [SerializeField] GameObject activeTree;
    private bool pastStart, pastFirst, pastSecond;
    private QuestList qList;

    private void Awake()
    {
        StartConversation(tutConvo_1.GetComponent<Tester>());
        pastStart = false;
        pastFirst = false;
        pastSecond = false;
        qList = questLog.GetComponent<QuestList>();
        PlayerActions.gunPickupEvent += CancelTutorialText;
        DialogueManager.dialogueEvent += TutorialWhen;
        QuestList.QuestAccept += SetTree;
        QuestList.QuestMarkEvent += TurnInMessage;
        activeTree.GetComponent<Tree>().enabled=false;
    }

    private void StartConversation(Tester conver)
    {       
        if(conver.name == tutConvo_1.GetComponent<Tester>().name)
        {
            DeActivatePlayer();
            tutConvo_1.GetComponent<Tester>().StartConvo();            
        }     
        else if(conver == tutConvo_2.GetComponent<Tester>())
        {
            DeActivatePlayer();           
            tutConvo_2.GetComponent<Tester>().StartConvo();            
        }
        else if (conver == tutConvo_3.GetComponent<Tester>())
        {
            DeActivatePlayer();
            tutConvo_3.GetComponent<Tester>().StartConvo();           
        }
    }

    private void TutorialWhen()
    {             
        if (!pastStart)
        {
            ActivatePlayer();
            if (qList != null)
                qList.AddQuest(quest1.GetComponent<MoveQuest>());
            else
                Debug.LogWarning("qList is null in TutorialEventScript");

            QuestList.QuestEvent += TutorialProgress;
            willy.GetComponent<QuestGiver>().active = false;
            tutorial.SetActive(true);
            tutorialText.text = "Press W A S D to move";
            pastStart = true;
            goto AfterWard;
        }
        else if (!pastFirst && pastStart)
        {
            ActivatePlayer();
            if (qList != null)
                qList.AddQuest(quest2.GetComponent<MoveQuest>());
            else
                Debug.LogWarning("qList is null in TutorialEventScript");

            tutorial.SetActive(true);
            tutorialText.text = "Press Space To Pick Up the Glowing weapon.";
            pastFirst = true;
            goto AfterWard;
        }
        else if (!pastSecond && pastFirst)
        {
            ActivatePlayer();
            willy.GetComponent<QuestGiver>().SetMarker();
            tutorial.SetActive(true);
            tutorialText.text = "Get Close To Willy And Press F To Get His Quest";
            pastSecond = true;
            goto AfterWard;
        }
    AfterWard:;
    }

    private void TutorialProgress(Quest q)
    {       
        if(q.name==quest1.name)
        {
            g1.gameObject.SetActive(false);
            g2.gameObject.SetActive(false);
            g3.gameObject.SetActive(false);
            CancelTutorialText();
            StartConversation(tutConvo_2.GetComponent<Tester>());          
        }
        else if(q.name==quest2.name)
        {
            //questLog.GetComponent<QuestList>().AddQuest(quest3.GetComponent<KillQuest>());           
            StartConversation(tutConvo_3.GetComponent<Tester>());
        }
        else if(q.name==quest3.name)
        {
            CancelTutorialText();
            qList.AddQuest(quest4.GetComponent<MoveQuest>());           
        }
        else if(q.name==quest4.name)
        {
            //GameManager.Load(GameManager.Scene.Harbor, GameManager.Scene.DesertedIsland);
            Loader.Load(Loader.Scene.Harbor);
            Debug.Log("Q4");
        }
    }
    private void CancelTutorialText()
    {
        tutorial.SetActive(false);
    }
    private void SetTree(Quest quest)
    {
        if(quest == willy.GetComponent<QuestGiver>().Quest)
        {
            tutorialText.text = "Press The Left Mouse Button To Fire Your Pistol.";
        }
    }
    private void TurnInMessage(Quest q)
    {
        if(q.name==willy.GetComponent<QuestGiver>().Quest.name)
        {
            tutorialText.text = "Talk to Willy to turn in the completed quest.";
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
        DialogueManager.dialogueEvent -= TutorialWhen;
        PlayerActions.gunPickupEvent -= CancelTutorialText;
        QuestList.QuestAccept -= SetTree;
        QuestList.QuestEvent -= TutorialProgress;
        QuestList.QuestMarkEvent -= TurnInMessage;
    }
}
