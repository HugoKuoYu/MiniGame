using System;
using UISystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIMainBottom : UIBase
{
    public enum MainBottomToggleTypes { Event, Shop, Game, Ranking }

    [Serializable]
    [SerializeField]
    private class BottomToggleSet
    {
        public Toggle Toggle;
        public Image Image;
    }
    [SerializeField] private Material GrayImageMaterial;
    [SerializeField] private BottomToggleSet EventToggle;
    [SerializeField] private BottomToggleSet ShopToggle;
    [SerializeField] private BottomToggleSet GameToggle;
    [SerializeField] private BottomToggleSet RankingToggle;
    public override void Initialize()
    {
        base.Initialize();
        EventToggle.Toggle.onValueChanged.AddListener((_isOn) => SetImageMaterial(_isOn, EventToggle.Image));
        ShopToggle.Toggle.onValueChanged.AddListener((_isOn) => SetImageMaterial(_isOn, ShopToggle.Image));
        GameToggle.Toggle.onValueChanged.AddListener((_isOn) => SetImageMaterial(_isOn, GameToggle.Image));
        RankingToggle.Toggle.onValueChanged.AddListener((_isOn) => SetImageMaterial(_isOn, RankingToggle.Image));
    }
    public void SetToggleCallback(MainBottomToggleTypes mainBottomToggleTypes, UnityAction<bool> toggleCallBack, bool isOn = false)
    {
        switch (mainBottomToggleTypes)
        {
            case MainBottomToggleTypes.Event:
                EventToggle.Toggle.onValueChanged.AddListener(toggleCallBack);
                EventToggle.Toggle.isOn = isOn;
                SetImageMaterial(isOn, EventToggle.Image);
                break;
            case MainBottomToggleTypes.Shop:
                ShopToggle.Toggle.onValueChanged.AddListener(toggleCallBack);
                ShopToggle.Toggle.isOn = isOn;
                SetImageMaterial(isOn, ShopToggle.Image);
                break;
            case MainBottomToggleTypes.Game:
                GameToggle.Toggle.onValueChanged.AddListener(toggleCallBack);
                GameToggle.Toggle.isOn = isOn;
                SetImageMaterial(isOn, GameToggle.Image);
                break;
            case MainBottomToggleTypes.Ranking:
                RankingToggle.Toggle.onValueChanged.AddListener(toggleCallBack);
                RankingToggle.Toggle.isOn = isOn;
                SetImageMaterial(isOn, RankingToggle.Image);
                break;
        };
    }

    private void SetImageMaterial(bool _isOn,Image _image)
    { 
        _image.material = _isOn ? null : GrayImageMaterial;
    }
}
