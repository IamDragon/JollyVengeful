using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGoal : Goal
{
    public Transform moveObject { get; set; }
    public MoveGoal(MoveQuest quest, Transform moveObject, string description, bool completed)
    {
        this.quest = quest;
        this.moveObject = moveObject;
        this.description = description;
        this.completed = completed;
    }
}
