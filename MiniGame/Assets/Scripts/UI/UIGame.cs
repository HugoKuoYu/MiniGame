using System.Collections;
using System.Collections.Generic;
using UISystem;
using UnityEngine;

public class UIGame : UIBase
{
    public override void Initialize()
    {
        base.Initialize();
    }
    // Start is called before the first frame update
    public override void Show()
    {
        base.Show();
        UISetting.isGamePause = true;
        Debug.Log("Show UIGame");
    }
    public override void Hide()
    {
        base.Hide();
        UISetting.isGamePause = false;
    }


}
