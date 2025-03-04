using System.Collections;
using UISystem;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UIStart : UIBase
{
    [SerializeField] Button startButton;
    [SerializeField] Transform rotateLoadingImage;
    [SerializeField] bool isloading = false;
    private float rotationDuration = 0.5f;

    public override void Initialize()
    {
        base.Initialize();
    }
    private void LateUpdate()
    {
        
    }
    public void Loading()
    {
        Debug.Log("LoadingGame");
        isloading = true;
        startButton.gameObject.SetActive(false);
        rotateLoadingImage.gameObject.SetActive(true);
        rotateLoadingImage.DORotate(new Vector3(0, 0, 720), rotationDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                Debug.Log("ShowOnStart");
                startButton.gameObject.SetActive(true);
                rotateLoadingImage.gameObject.SetActive(false);
                isloading = false;
                startButton.onClick.AddListener(()=>onStartButton());
            });
        
    }

    private void onStartButton()
    {
        startButton.onClick.RemoveAllListeners();
        UIManager.Instance.Release<UIStart>();
        UIManager.Instance.ShowUI<UIMain>();
    }
}
