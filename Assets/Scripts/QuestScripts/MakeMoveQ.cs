using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeMoveQ : MoveQuest
{
    private void Start()
    {
        goals.Add(new MoveGoal(this, moveObject, description, false));
    }    
}
