using System;
using System.Collections;
using System.Collections.Generic;
using UISystem;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        UIManager.Instance.ShowUI<UIStart>(OnStartLoading => OnStartLoading.Loading());
    }
}
