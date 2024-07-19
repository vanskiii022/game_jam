using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPanel : PanelBase
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameManager.Instance.ToLevel(1);
            gameObject.SetActive(false);
        }
    }
}
