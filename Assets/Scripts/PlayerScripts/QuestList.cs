using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestList : MonoBehaviour
{

    public static QuestList instance;
    public static event Action<Quest> QuestEvent;
    public static event Action<Quest> QuestMarkEvent;
    public static event Action<Quest> QuestAccept;
    [SerializeField] GameObject playerStatus;
    [SerializeField] List<Quest> questList = new List<Quest>();
    [SerializeField] Text insertLog;
    private PlayerStatus ps;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public void Start()
    {
        ps = playerStatus.GetComponent<PlayerStatus>();
        CombatEvents.onEnemyDeath += CheckQGoal;
    }

    public void AddQuest(Quest quest)
    {
        questList.Add(quest);
        ShowList();
        QuestAccept?.Invoke(quest);
    }
    public void ShowList()
    {
        
        insertLog.text = "";
        foreach(Quest quest in questList)
        {
            if(quest!=null)
            {
                if (quest.questCompleted)
                    insertLog.color = Color.green;
                else
                    insertLog.color = Color.black;
                if (quest as KillQuest)
                {
                    KillQuest killQuest = (KillQuest)quest;
                    insertLog.text = "=" + insertLog.text + killQuest.name + "=\n" + "Kill: " + killQuest.enemyType + " " + killQuest.currentAmount + "/" + killQuest.requiredAmount + "\n";
                }
                else if (quest as MoveQuest)
                {
                    MoveQuest moveQuest = (MoveQuest)quest;
                    insertLog.text = "=" + insertLog.text + moveQuest.name + "=\n" + "Move to: " + moveQuest.moveObject.name + "\n";
                }
            }                                   
        }
    }

    public void CheckQGoal(Fighter fighter)
    {
        foreach(Quest quest in questList)
        {
            if(quest!=null)
            {
                if (quest as KillQuest)
                {
                    KillQuest killQuest = (KillQuest)quest;
                    if (killQuest.enemyType == fighter.enemyType/* && !quest.questCompleted*/)
                    {
                        killQuest.currentAmount++;
                        if (killQuest.currentAmount >= killQuest.requiredAmount)
                        {
                            quest.questCompleted = true;

                            if (quest.autoTurnIn)
                            {
                                ShowList();

                                DoDelayAction(5, quest);
                                return;
                            }

                            QuestMarkEvent?.Invoke(quest);

                            
                        }                                                   
                        ShowList();
                    }
                }
            }              
        }
    }

    public void CheckLocation(Transform mObject)
    {
        foreach(Quest quest in questList)
        {
            if(quest!=null)
            {
                if (quest as MoveQuest)
                {
                    MoveQuest m = (MoveQuest)quest;
                    if (m.moveObject == mObject)
                    {
                        if (quest.autoTurnIn)
                            QuestCompleted(quest);
                        else
                            quest.questCompleted = true;                          
                    }
                    ShowList();
                    break;
                }
            }           
        }
    }

   

    public void QuestCompleted(Quest quest)
    {       
        ps.Rewarded(quest.goldReward,quest.experienceReward);
        questList.Remove(quest);
        insertLog.text = string.Empty;
        ShowList();
        QuestEvent?.Invoke(quest);
        if (quest.autoTurnIn && quest is KillQuest)
            Destroy(quest.gameObject);
    }

    void DoDelayAction(float delayTime, Quest quest)
    {
        StartCoroutine(DelayAction(delayTime, quest));
    }

    IEnumerator DelayAction(float delayTime, Quest quest)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);

        QuestCompleted(quest);
        //Do the action after the delay time has finished.
    }
}
