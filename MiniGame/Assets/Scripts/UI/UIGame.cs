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
    public void Start()
    {
        GameManager.Instance.Start2048Game();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
