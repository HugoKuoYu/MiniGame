using System;
using System.Collections;
using System.Collections.Generic;
using UISystem;
using UnityEngine;
using UnityEngine.UI;

public class ViewGamePage : UIBase
{
    public Button gameButton;
    public override void Initialize()
    {
        base.Initialize();
        gameButton.onClick.AddListener(goToGame);
    }

    private void goToGame()
    {
        Debug.Log("goToGame");
        UIManager.Instance.ShowUI<UIGame>();
    }
}
