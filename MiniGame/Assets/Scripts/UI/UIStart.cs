using System;
using System.Collections;
using System.Collections.Generic;
using UISystem;
using UnityEngine;
using UnityEngine.UI;

public class UIStart : UIBase
{
    [SerializeField] Button startButton;
    [SerializeField] Transform loadingImage;
    [SerializeField] bool isloading = false;
    private const float rotationSpeed = 90;

    public override void Initialize()
    {
        base.Initialize();
    }
    private void LateUpdate()
    {
        
    }
    public void Loading()
    {
        isloading = true;
        startButton.gameObject.SetActive(false);
        loadingImage.gameObject.SetActive(true);

        StartCoroutine(startLoading());
        Debug.Log("LoadingGame");
    }

    IEnumerator startLoading()
    {
        return null;
    }
}
