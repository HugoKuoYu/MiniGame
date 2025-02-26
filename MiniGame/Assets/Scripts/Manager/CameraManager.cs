using UnityEngine;

[RequireComponent(typeof(Camera))] //只能套用在有camera組件的物件上。
public class CameraManager : MonoBehaviour
{
    public Color32 BackgroundColor;
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main; // 快取 Camera，避免每次存取 Camera.main
    }

    private void Start()
    {
        cam.orthographicSize = GetSize();
        cam.backgroundColor = BackgroundColor;
    }

    float GetSize()
    {
        float baseRatio = 1920f / 1080f; //基本以16:9為基準
        float screenRatio = (float)Screen.height / Screen.width;//取得當前螢幕比例
        float fixedSize = screenRatio / baseRatio * 10f; //取得偏差比例

        return Mathf.Clamp(fixedSize, 8.5f, 10f); // 設定可接受偏差的上下限
    }
}
