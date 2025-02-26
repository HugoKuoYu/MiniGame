using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{

    public CanvasGroup canvasGroup;
    public virtual void Initialize()
    { 
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public virtual void Hide()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;//畫布隱形
            canvasGroup.interactable = false;//不可互動
            canvasGroup.blocksRaycasts = false;//關閉雷射
        }
        else//若畫布為null
        { 
            gameObject.SetActive(false);//直接禁用
        }
    }
    public virtual void Show()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1;//畫布顯示
            canvasGroup.interactable = true;//可互動
            canvasGroup.blocksRaycasts = true;//雷射開啟
        }
        else//若畫布為null
        {
            gameObject.SetActive(true);//直接顯示物件
        }
    }
    public virtual void Close()
    {
        Hide();
        Destroy(gameObject);
    }
}
