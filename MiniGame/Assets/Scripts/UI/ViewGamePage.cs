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
        UIManager.Instance.ShowUI<UIGame>((ui) =>
        {
            UIManager.Instance.GetUI<UIMain>().Hide();
            UIManager.Instance.GetUI<UIMainBottom>().Hide();
        });
        GameManager.Instance.Start2048Game();

    }
}
