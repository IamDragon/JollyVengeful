using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
public class MakeKillQ : KillQuest
{
    [SerializeField] bool lastQuest = false;
    private void Start()
    {
        DontDestroyOnLoad(this);
        currentAmount = 0;
        goals.Add(new KillGoal(this, enemyType, description, false, currentAmount, requiredAmount));

        goals.ForEach(g => g.Init());
    }

    private void OnDestroy()
    {
        if (questCompleted && lastQuest)
        {
            GameManager.instance.GameComplete();
        }
    }
}
