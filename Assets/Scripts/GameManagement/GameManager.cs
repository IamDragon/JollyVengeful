using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private bool showCursor = false;
    private bool playerDead;
    [SerializeField] GameObject userInterface;

    public enum Scene
    {
        Harbor, Tavern, DesertedIsland
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        playerDead = false;
        Cursor.visible = showCursor;
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Update()
    {
        if (Cursor.visible != showCursor)
            Cursor.visible = showCursor;
    }

    public void SetShowCursor(bool show)
    {
        showCursor = show;
    }

    public void PlayerDeath(float delay)
    {
        playerDead = true;
        WorldState.Reset();
        Destroy(this.gameObject, delay);
        Destroy(PlayerController.instance.gameObject, delay);
        Destroy(MainCameraScript.instance.gameObject, delay);




        PlayerController.instance = null;
    }

    private void OnDestroy()
    {
        if (instance == this && playerDead)
        {
            Loader.Load(Loader.Scene.DeathScreen);
            Cursor.visible = true;
        }
    }

    public void GameComplete()
    {
        WorldState.Reset();
        Destroy(PlayerController.instance.gameObject);
        Destroy(MainCameraScript.instance.gameObject);
        Destroy(this.gameObject);
        Loader.Load(Loader.Scene.ToBeContinued);
        Cursor.visible = true;
    }

    public void HudToggle(bool activated)
    {
        userInterface.SetActive(activated);
    }
}

