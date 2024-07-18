using UnityEngine;
using UnityEngine.UI;

public class UIZoomAndCenter : MonoBehaviour
{
    public RectTransform uiElement; // 要控制的UI元素
    public Image blackOverlay; // 黑色遮罩
    public float targetScale = 1f; // 目标缩放比例
    public float duration = 2f; // 动画持续时间

    private Vector3 initialScale; // 初始缩放比例
    private Vector3 initialPosition; // 初始位置
    private static float elapsedTime = 0f; // 已经过的时间
    private static bool isAnimating = false; // 是否正在动画

    void Start()
    {
        if (uiElement == null)
        {
            Debug.LogError("UI Element is not assigned.");
            return;
        }

        if (blackOverlay == null)
        {
            Debug.LogError("Black Overlay is not assigned.");
            return;
        }

        initialScale = uiElement.localScale;
        initialPosition = uiElement.localPosition;

        // 确保黑色遮罩一开始是全黑
        blackOverlay.color = new Color(0, 0, 0, 0);
    }

    void Update()
    {
        if (isAnimating)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // 插值方式平滑改变缩放比例和位置
            uiElement.localScale = Vector3.Lerp(initialScale, Vector3.one * targetScale, t);
            uiElement.localPosition = Vector3.Lerp(initialPosition, Vector3.zero, t);

            // 插值方式平滑改变黑色遮罩的透明度
            blackOverlay.color = new Color(0, 0, 0,t);

            if (t >= 1f)
            {
                isAnimating = false;
            }
        }
    }

    // 通过这个方法开始动画
    public static void StartAnimation()
    {
        elapsedTime = 0f;
        isAnimating = true;
    }
}


