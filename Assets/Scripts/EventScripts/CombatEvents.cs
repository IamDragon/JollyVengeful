using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CombatEvents : MonoBehaviour
{
    public delegate void EnemyEventHandler(Fighter enemy);
    public static event EnemyEventHandler onEnemyDeath;
    public static void EnemyDeath(Fighter enemy)
    {
        if (onEnemyDeath!=null)
            onEnemyDeath(enemy);           
    }        
}
