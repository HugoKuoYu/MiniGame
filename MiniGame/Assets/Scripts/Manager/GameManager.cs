using System;
using System.Collections;
using System.Collections.Generic;
using UISystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : Singleton<GameManager>
{
    private UI_Game2048 game2048;
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
    public async void Start2048Game()
    {
        var gameStartHandle = Addressables.LoadAssetAsync<GameObject>("UI_Game2048");
        await gameStartHandle.Task;
        if (gameStartHandle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            if (game2048 == null)
            {
                Debug.Log("目前game2048是空的");
                Transform parent = UIManager.Instance.GetSubUITransform<UIGame>("Panel/UIGameMain");
                var handle = Addressables.InstantiateAsync(typeof(UI_Game2048).Name, parent);
                await handle.Task;
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    game2048 = handle.Result.GetComponent<UI_Game2048>();
                    //UIManager.Instance.ShowUI<UIGame>();
                    game2048.Init();
                }
            }
            else
            {
                UIManager.Instance.ShowUI<UIGame>();
                game2048.NewGame();
            }
        }
    }
    public void Close2048Game()
    {
        Debug.Log("關閉Close2048Game");
    }
}
