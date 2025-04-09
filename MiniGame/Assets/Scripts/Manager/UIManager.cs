using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace UISystem
{
    
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] GraphicRaycaster graphicRaycaster;

        [SerializeField] RectTransform safeAreaBaseRectTransform; //放底層ui，如顯示基本框架、背景
        [SerializeField] RectTransform safeAreaMiddleRectTransform; //放一般ui，如背包、任務、選單
        [SerializeField] RectTransform safeAreaFrontTopRectTransform; //放顯示在螢幕最高的ui，如錢、快捷鍵、狀態
        [SerializeField] RectTransform FullFrontRectTransform; // 放最高優先ui，如設定、全屏彈窗

        [SerializeField] RectTransform SafeAreaPaddingTopRectTransform; //補齊螢幕頂部UI(當被瀏海遮住時)
        [SerializeField] RectTransform SafeAreaPaddingBottomRectTransform; //補齊螢幕底部UI (當被狀態列遮住)


        Dictionary<string, (UIBase, AsyncOperationHandle)> UIDictionary = new();
        private ushort loadingUICount = 0; //計算載入中的UI數量


        protected override void Awake()
        {
            base.Awake();
            SetSafeArea();

        }
        private void SetSafeArea()
        {
            Rect safeArea = Screen.safeArea; //取得螢幕本身的safeArea(包含position與size)
            var minAnchor = safeArea.position; //安全區域的左下角座標
            var maxAnchor = safeArea.position + safeArea.size; //安全區域的右上角座標
            minAnchor.y /= Screen.height; //將minAnchor轉換成0~1的相對值
            maxAnchor.y /= Screen.height; //將maxAnchor轉換成0~1的相對值
            if (maxAnchor.y != 1) //如果被瀏海或狀態列擋住
            {
                SafeAreaPaddingTopRectTransform.gameObject.SetActive(true); //打開補齊最上面的ui圖
                SafeAreaPaddingTopRectTransform.anchorMin = new Vector2(0, maxAnchor.y); //設定ui的anchorMin來適應safeArea
                var anchorMax = new Vector2(1, maxAnchor.y);
                safeAreaBaseRectTransform.anchorMax = anchorMax; //讓所有ui層的anchorMax不超過safeArea上緣
                safeAreaMiddleRectTransform.anchorMax = anchorMax;
                safeAreaFrontTopRectTransform.anchorMax = anchorMax;
            }
            if (minAnchor.y != 0)
            {
                SafeAreaPaddingBottomRectTransform.gameObject.SetActive(true);
                SafeAreaPaddingBottomRectTransform.anchorMax = new Vector2(1, minAnchor.y);//設定ui的anchorMin來適應safeArea
                var anchorMin = new Vector2(0, minAnchor.y);
                safeAreaBaseRectTransform.anchorMax = anchorMin; //讓所有ui層的anchorMin不低於safeArea下緣
                safeAreaMiddleRectTransform.anchorMax = anchorMin;
                safeAreaFrontTopRectTransform.anchorMax = anchorMin;
            }
        }
        public void ShowUI<T>(Action<T> onLoaded = null) where T : UIBase //ShowUI只能載入UIBase類型物件，載入後會執行Onloaded函數
        {
            var key = typeof(T).Name;
            if (UIDictionary.ContainsKey(key))
            {
                var ui = UIDictionary[key].Item1 as T;
                if (ui != null)
                {
                    ui.Show();
                    onLoaded?.Invoke(ui);
                }
            }
            else
            {
                if (graphicRaycaster.enabled == true)
                    graphicRaycaster.enabled = false;
                loadingUICount += 1; //載入中的UI數量+1
                var handle = Addressables.LoadAssetAsync<GameObject>(key); //發送異步加載請求
                UIDictionary.Add(key, (null, handle)); //先存入字典中，表示正在載入，此時ui還沒被實例化，所以先存null
                bool onLoadedSuccess = false; //紀錄目前尚未載入

                handle.Completed += (_handle) => //當載入完成時
                {
                    if (_handle.Status == AsyncOperationStatus.Succeeded) //檢查是否載入成功
                    {
                        GameObject gameobj = Instantiate(handle.Result); //實例化ui
                        var ui = gameobj.GetComponent<T>(); //取得uiBase物件
                        if (ui != null)
                        {
                            onLoadedSuccess = true; //標記成功載入
                            gameobj.transform.SetParent(getLayerTransform(ui.uiLayerEnum), false); //將只要繼承uibase都可以設定uilayer的位置
                            UIDictionary[key] = (ui, _handle); //更新字典中的此UI
                            ui.Initialize();
                            ui.Show();
                            onLoaded?.Invoke(ui);
                        }
                    }
                    if (!onLoadedSuccess) //如果UI載入失敗
                    {
                        Debug.LogError($"ShowUI 的 {key} 載入失敗!"); //顯示錯誤
                        UIDictionary.Remove(key); //從字典中移除UI
                        Addressables.Release(handle); //釋放Addressables資源
                    }
                    loadingUICount -= 1;//載入中的UI數量-1
                    if (loadingUICount == 0) //如果全部載入完成
                    {
                        if (graphicRaycaster.enabled == false) 
                            graphicRaycaster.enabled = true; //打開射線，可跟ui互動
                    }
                };
            }
        }
        public T GetUI<T>() where T : UIBase //ShowUI只能載入UIBase類型物件
        { 
            var key = typeof(T).Name; //取得T的類別名稱
            if (UIDictionary.ContainsKey(key)) //如果字典中包含此類別
            {
                return UIDictionary[key].Item1 as T; //回傳該名稱的類別
            }
            else //如果字典中沒有包含此類別
            {
                Debug.LogError("UIManager GetUI " + key + " not exist!"); //顯示錯誤
                return null; //回傳null
            }
        }
        public void Hide<T>() where T : UIBase
        { 
            String key = typeof(T).Name;
            if (UIDictionary.ContainsKey(key))
            {
                UIDictionary[key].Item1.Hide();
            }
            else
                Debug.LogError("UIManager Hide " + key + " not exist!");
        }
        public void Release<T>() where T : UIBase
        {
            String key = typeof(T).Name;
            if (UIDictionary.ContainsKey(key))
            {
                if (UIDictionary[key].Item1 != null)
                    Destroy(UIDictionary[key].Item1.gameObject);
                if (UIDictionary[key].Item2.IsValid())
                    Addressables.Release(UIDictionary[key].Item2);
            }
            else
                Debug.LogError("UIManager Release " + key + " not exist!");
        }
        private Transform getLayerTransform(UILayerEnum uiLayerEnum)
        {
            return uiLayerEnum switch
            {
                UILayerEnum.SafeAreaBase => safeAreaBaseRectTransform,
                UILayerEnum.SafeAreaMiddle => safeAreaMiddleRectTransform,
                UILayerEnum.SafeAreaTop => safeAreaFrontTopRectTransform,
                UILayerEnum.FullFront => FullFrontRectTransform,
                _ => null
            };
        }
        public Transform GetSubUITransform<T>(string subObjectName) where T : UIBase
        { 
            var ui = GetUI<T>();
            var target = ui.transform.Find(subObjectName);
            if (target == null) Debug.Log($"找不到{typeof(T).Name}底下的{subObjectName}");
            return target;
        }
    }
}