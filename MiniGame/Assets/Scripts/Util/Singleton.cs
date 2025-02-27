using UnityEngine;


public class Singleton<T> : MonoBehaviour where T: MonoBehaviour //T可以是任何繼承Monobahaviour的類別，可以附加到Unity中
{
    private static T instance; //所有<T>的類別實例都會共用這個instance
    public static T Instance
    {
        get 
        {
            if (instance == null)
            { 
                instance = FindObjectOfType<T>(); //尋找場景中是否已存在類型<T>的單例
                if (instance == null) //場景中沒有，動態創建一個新的Gameobject
                {
                    GameObject obj = new GameObject(typeof(T).Name); //此Gameobject的名字設定為類別名稱
                    instance = obj.AddComponent<T>(); //將<T>類別附加到Gameobject上，確保它成為Monobehaviour並存在於場景中。
                }
            }
            return instance; //最後回傳instance
        }
    }
    protected virtual void Awake() //防止重複實例化
    {
        if (instance == null) //如果instance尚未初始化
        {
            instance = this as T; //則將目前物件設為instance
            DontDestroyOnLoad(gameObject); //確保這個單例化不會在切換場景時銷毀
        }
        else //如果instance已經存在
        { 
            Destroy(gameObject); //刪除新創建的物件
        }
    }
}
