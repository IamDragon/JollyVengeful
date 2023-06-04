using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public static class Loader
{

    private class LoadingMonoBehaviour : MonoBehaviour { }
    public enum Scene
    {
        DesertedIsland,
        Harbor,
        StartMenu,
        LoadingScene,
        Tavern,
        DeathScreen,
        ToBeContinued
    }
    public static Scene CurrentScene;

    private static Action onLoaderCallback;
    private static AsyncOperation loadingAsyncOperation;

    public static void Load(Scene scene)
    {
        CurrentScene = scene;
        // Set the loader callback action to load the target scene
        if (PlayerController.instance != null && GameManager.instance != null)
        {
            PlayerController.instance.gameObject.SetActive(false);
            GameManager.instance.HudToggle(false);
        }

        onLoaderCallback = () =>
        {
            GameObject loadingGameObject = new GameObject("Loading Game Object");
            loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAync(scene));

        };

        // Load the loading scene
        SceneManager.LoadScene(Scene.LoadingScene.ToString());

        //if (SpawnManager.instance != null)
        //    SpawnManager.instance.SpawnPlayer(scene);

    }

    private static IEnumerator LoadSceneAync(Scene scene)
    {
        yield return null;

        loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());
      
        while (!loadingAsyncOperation.isDone)
        {
            yield return null;
        }
    }

    public static float GetLoadingProgress()
    {
        if (loadingAsyncOperation != null)
        {
            if (loadingAsyncOperation.progress < 0.85)
                return loadingAsyncOperation.progress;
            else
            {
                return 1;
            }
        }
        else
            return 0f;
    }

    public static void LoaderCallback()
    {
        // Triggered after the first update which lets the screen refresh
        // Execute the loader callback action which will load the target scene
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }

    public static void ResetLoadingProgress()
    {
        if (loadingAsyncOperation != null)
            loadingAsyncOperation = null;
    }
}
