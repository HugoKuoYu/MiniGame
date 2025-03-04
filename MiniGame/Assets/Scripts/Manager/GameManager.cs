using System;
using System.Collections;
using System.Collections.Generic;
using UISystem;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        Application.targetFrameRate = 60;
        base.Awake();
    }
    private void Start()
    {
        UIManager.Instance.ShowUI<UIStart>(OnStartLoading => OnStartLoading.Loading()); //載入開始畫面時loading
        UIManager.Instance.ShowUI<UIMain>((ui)=> ui.Hide()); //預先載入UIMain
    }
}
