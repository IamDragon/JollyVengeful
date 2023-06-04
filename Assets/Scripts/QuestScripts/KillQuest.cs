using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class KillQuest : Quest
{
    public string enemyType;
    public int currentAmount { get; set; }
    public int requiredAmount;
    private void Start()
    {
        goals = new List<Goal>();
    }    
}
