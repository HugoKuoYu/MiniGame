using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] GraphicRaycaster graphicRaycaster;

    [SerializeField] RectTransform safeAreaBaseRectTransform;
    [SerializeField] RectTransform safeAreaMiddleRectTransform;
    [SerializeField] RectTransform safeAreaFrontTopRectTransform;
    [SerializeField] RectTransform FullFrontRectTransform;


    protected override void Awake()
    { 
        base.Awake();

    }
    private void SetSafeArea()
    { 
        
    }
}
