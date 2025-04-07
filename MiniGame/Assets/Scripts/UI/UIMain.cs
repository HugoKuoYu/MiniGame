using System.Collections;
using System.Collections.Generic;
using UISystem;
using Unity.VisualScripting;
using UnityEngine;

public class UIMain : UIBase
{
    [SerializeField] ViewEventPage viewEventPage;
    [SerializeField] ViewShopPage viewShopPage;
    [SerializeField] ViewGamePage viewGamePage;
    [SerializeField] ViewRankingPage viewRankingPage;
    public override void Initialize()
    {
        base.Initialize();
        viewGamePage.Initialize();
        UIManager.Instance.ShowUI<UIMainTop>((ui)=>ui.clickSettingButton(() => UIManager.Instance.ShowUI<UISetting>()));
        UIManager.Instance.ShowUI<UIMainBottom>((ui)=>
        {
            ui.SetToggleCallback(UIMainBottom.MainBottomToggleTypes.Event,(isOn) => viewActive(viewEventPage,isOn));
            ui.SetToggleCallback(UIMainBottom.MainBottomToggleTypes.Shop, (isOn) => viewActive(viewShopPage, isOn));
            ui.SetToggleCallback(UIMainBottom.MainBottomToggleTypes.Game, (isOn) => viewActive(viewGamePage, isOn), true);
            ui.SetToggleCallback(UIMainBottom.MainBottomToggleTypes.Ranking, (isOn) => viewActive(viewRankingPage, isOn));
        });
    }
    private void viewActive(UIBase _uiBase,bool _isOn)
    {
        if (_isOn) _uiBase.Show();
        else _uiBase.Hide();
    }
}
