using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public List<Goal> goals { get; set; }
    public string questName;
    public string description;
    public bool autoTurnIn;
    public string questType { get; set; }
    public int experienceReward;
    public int goldReward;
    public bool questCompleted { get; set; }
}
