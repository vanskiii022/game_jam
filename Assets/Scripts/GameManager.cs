using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // Start is called before the first frame update
    void Start()
    {
        MapManager.Instance.ChangeMap("DemoMap");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
