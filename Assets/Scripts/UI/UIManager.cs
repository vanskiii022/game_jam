using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public MainMenuPanel mainMenuPanel;
    public DeathPanel deathPanel;
    public WinPanel winPanel;
    public HintPanel hintPanel;

    public void Show(PanelBase panel)
    {
        if (panel == null) return;
        panel.gameObject.SetActive(true);
        panel.OnShow();
    }

    public void Hide(PanelBase panel)
    {
        if (panel == null) return;
        panel.gameObject.SetActive(false);
        panel.OnHide();
    }
}
