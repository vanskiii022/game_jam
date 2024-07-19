using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public string initMapName = "DemoMap";
    public string[] levelMaps = new string[0];

    private UIManager uiManager;

    void Start()
    {
        uiManager = UIManager.Instance;

        if (GetCurSceneName() == "MainScene")
        {
            MapManager.Instance.InitCameraActive();
            uiManager.Show(uiManager.hintPanel);
            MapManager.Instance.ChangeMap(initMapName);
        }
        else if (GetCurSceneName() == "Launch")
        {
            ToLevel(1);
        }
    }

    public void ToStartMenu()
    {
        Debug.Log("ToStartMenu");
        uiManager.Show(uiManager.winPanel);
        uiManager.Hide(uiManager.hintPanel);
        if (PlayerManager.Instance) PlayerManager.Instance.player.isLocked = true;
    }

    public void ToLevel(int i)
    {
        Debug.Log("ToLevel" + i);

        ToScene("MainScene", () =>
        {
            uiManager.Show(uiManager.hintPanel);
            if (PlayerManager.Instance) PlayerManager.Instance.player.isLocked = false;

            if (i <= levelMaps.Length)
            {
                MapManager.Instance.ChangeMap(levelMaps[i - 1]);
            }
            else
            {
                Debug.LogError("invalid level: " + i);
            }
        });
    }

    //public void ToDeathMenu()
    //{
    //    Debug.Log("ToDeathMenu");
    //    ToScene("MainScene");
    //}

    public void ToWinMenu()
    {
        Debug.Log("ToWinMenu");
        ToScene("MainScene", () =>
        {
            uiManager.Show(uiManager.winPanel);
            uiManager.Hide(uiManager.hintPanel);
            if (PlayerManager.Instance) PlayerManager.Instance.player.isLocked = true;
        });
    }

    private string GetCurSceneName()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        return currentScene.name;
    }

    private void ToScene(string sceneName, Action onComplete)
    {
        if (sceneName == GetCurSceneName())
        {
            onComplete();
            return;
        }

        StartCoroutine(LoadAsyncScene(sceneName, onComplete));
    }

    IEnumerator LoadAsyncScene(string sceneName, Action onComplete)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (sceneName == "MainScene")
        {
            MapManager.Instance.BindCameras();
        }
        else
        {
            MapManager.Instance.UnbindCameras();
        }

        onComplete();
    }
}
