using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldState :ScriptableObject
{
    public static bool harborFirst { get; set; } = true;
    public static bool tavernFirst { get; set; } = true;
    public static bool harborRuffian { get; set; } = false;

    public static void Reset()
    {
        harborFirst = true;
        tavernFirst = true;
        harborRuffian = false;
    }
}
