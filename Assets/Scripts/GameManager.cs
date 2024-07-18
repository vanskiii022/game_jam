using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public string[] levelMaps = new string[0];

    void Start()
    {
        MapManager.Instance.ChangeMap("World1");
    }

    public void ToStartMenu()
    {

    }

    public void ToLevel(int i)
    {
        if (i < levelMaps.Length)
        {
            MapManager.Instance.ChangeMap(levelMaps[i]);
        }
        else
        {
            Debug.LogError("invalid level: " + i);
        }
    }

    public void ToDeathMenu()
    {

    }

    public void ToWinMenu()
    {

    }
}
