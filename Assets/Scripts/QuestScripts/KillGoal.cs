using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGoal : Goal
{
    public string enemyType { get; set; }
    public KillGoal(KillQuest quest, string enemyType, string description, bool completed, int currentAmount, int requiredAmount)
    {
        this.quest = quest;
        this.enemyType = enemyType;
        this.description = description;
        this.completed = completed;
        this.currentAmount = 0;
        this.requiredAmount = requiredAmount;
    }    
}
