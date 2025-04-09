using System;
using System.Collections;
using System.Collections.Generic;
using UISystem;
using UnityEngine;
using UnityEngine.UI;

public class UISetting : UIBase
{
    [SerializeField] Button closeButton;
    [SerializeField] Button restartButton;
    [SerializeField] Button backMenuButton;
    [SerializeField] Toggle bgmToggle;
    [SerializeField] Toggle sfxToggle;
    [SerializeField] GameObject bgmOffGameobject;
    [SerializeField] GameObject sfxOffGameobject;
    [SerializeField] GameObject GamePause;
    private GameSetting gameSetting;
    public static bool isGamePause = false;


    public override void Initialize()
    {
        base.Initialize();
        gameSetting = GameSetting.GetSetting();
        closeButton.onClick.AddListener(() => UIManager.Instance.Hide<UISetting>());
        backMenuButton.onClick.AddListener(backToMenu);

        setUpToggle(bgmToggle, bgmOffGameobject, () => gameSetting.bgmSound, (value) => gameSetting.bgmSound = value);
        setUpToggle(sfxToggle, sfxOffGameobject, () => gameSetting.sfxSound, (value) => gameSetting.sfxSound = value);
    }
    public override void Show()
    {
        string a = isGamePause ? "有" : "沒有";
        base.Show();
        Debug.Log($"目前   {a}　　　在遊戲中按暫停");
        GamePause.SetActive(isGamePause);
    }

    private void backToMenu()
    {

        UIManager.Instance.ShowUI<UIMain>();
        UIManager.Instance.ShowUI<UIMainBottom>();
        GameManager.Instance.Close2048Game();
        UIManager.Instance.Hide<UIGame>();
        Hide();
    }

    private void setUpToggle(Toggle _toggle, GameObject _offIcon, System.Func<bool> getValue, System.Action<bool> setValue)
    {
        bool isOn = getValue(); //取得目前存的值
        _toggle.isOn = isOn; //設定初始值
        _offIcon.SetActive(!isOn); //設定開關顯示

        _toggle.onValueChanged.RemoveAllListeners();
        _toggle.onValueChanged.AddListener((isOn) =>
        {
            _offIcon.SetActive(!isOn);
            setValue(isOn);
            gameSetting.Save(); //儲存目前設定
        });
    }

}
