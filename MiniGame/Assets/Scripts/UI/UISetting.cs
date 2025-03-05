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
    private GameSetting gameSetting;


    public override void Initialize()
    {
        base.Initialize();
        gameSetting = GameSetting.GetSetting();
        closeButton.onClick.AddListener(() => UIManager.Instance.Hide<UISetting>());

        setUpToggle(bgmToggle, bgmOffGameobject, () => gameSetting.bgmSound, (value) => gameSetting.bgmSound = value);
        setUpToggle(sfxToggle, sfxOffGameobject, () => gameSetting.sfxSound, (value) => gameSetting.sfxSound = value);
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
