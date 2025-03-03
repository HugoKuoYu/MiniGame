using System.Collections;
using System.Collections.Generic;
using UISystem;
using Unity.VisualScripting;
using UnityEngine;

public class UIMain : UIBase
{
    public override void Initialize()
    {
        base.Initialize();
    }
    public void Start()
    {
        UIManager.Instance.ShowUI<UIMainTop>();
        UIManager.Instance.ShowUI<UIMainBottom>();
    }
}
