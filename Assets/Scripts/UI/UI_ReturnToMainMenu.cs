using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ReturnToMainMenu : MonoBehaviour
{
    public void ReturnToStartMenu()
    {
        Loader.Load(Loader.Scene.StartMenu);
    }
}
