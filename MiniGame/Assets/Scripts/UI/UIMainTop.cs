using System;
using System.Collections;
using System.Collections.Generic;
using UISystem;
using UnityEngine;
using UnityEngine.UI;

public class UIMainTop : UIBase
{
    [SerializeField] Button settingButton;
    public override void Initialize()
    {
        base.Initialize();
        
    }

    public void clickSettingButton(Action CallBack)
    {
        settingButton.onClick.RemoveAllListeners();
        settingButton.onClick.AddListener(() => CallBack.Invoke());
    }
}
