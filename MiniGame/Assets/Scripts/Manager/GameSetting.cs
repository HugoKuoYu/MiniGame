using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetting 
{
    private GameSetting() { } //私有建構子，防止外部實例化
    public bool sfxSound = true; //預設為開啟音樂
    public bool bgmSound = true; //預設開啟背景
    public static GameSetting GetSetting()
    {
        string dataJson = PlayerPrefs.GetString(typeof(GameSetting).Name, "");//GameSetting為存取PlayerPrefs的key，若沒有存過則傳回""字串
        GameSetting gameSetting;
        if (string.IsNullOrEmpty(dataJson)) //如果GameSetting沒有存檔過
        {
            gameSetting = new GameSetting();//使用預設設定
            gameSetting.Save();//把預設設定存到PlayPrefs中
        }
        else //如果有存檔過
        {
            try
            {
                gameSetting = JsonConvert.DeserializeObject<GameSetting>(dataJson);//將json轉回gamesetting物件
            }
            catch //如果失敗
            {
                gameSetting = new GameSetting(); //重新建立新的gamesetting
                gameSetting.Save();//重新存到PlayerPrefs中
            }
        }
        return gameSetting; //回傳gamesetting

    }
    public void Save()
    { //將GameSetting轉換成json來存檔
        PlayerPrefs.SetString(typeof(GameSetting).Name,JsonConvert.SerializeObject(this));
        PlayerPrefs.Save();//將PlayerPrefs存到硬碟中
    }
}
