using UnityEngine;
using UnityEngine.UI;

public class UIZoomAndCenter : MonoBehaviour
{
    public RectTransform uiElement; // Ҫ���Ƶ�UIԪ��
    public Image blackOverlay; // ��ɫ����
    public float targetScale = 1f; // Ŀ�����ű���
    public float duration = 2f; // ��������ʱ��

    private Vector3 initialScale; // ��ʼ���ű���
    private Vector3 initialPosition; // ��ʼλ��
    private static float elapsedTime = 0f; // �Ѿ�����ʱ��
    private static bool isAnimating = false; // �Ƿ����ڶ���

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

        // ȷ����ɫ����һ��ʼ��ȫ��
        blackOverlay.color = new Color(0, 0, 0, 0);
    }

    void Update()
    {
        if (isAnimating)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // ��ֵ��ʽƽ���ı����ű�����λ��
            uiElement.localScale = Vector3.Lerp(initialScale, Vector3.one * targetScale, t);
            uiElement.localPosition = Vector3.Lerp(initialPosition, Vector3.zero, t);

            // ��ֵ��ʽƽ���ı��ɫ���ֵ�͸����
            blackOverlay.color = new Color(0, 0, 0,t);

            if (t >= 1f)
            {
                isAnimating = false;
            }
        }
    }

    // ͨ�����������ʼ����
    public static void StartAnimation()
    {
        elapsedTime = 0f;
        isAnimating = true;
    }
}


