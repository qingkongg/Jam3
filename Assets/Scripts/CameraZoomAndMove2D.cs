using UnityEngine;

public class CameraZoomAndMove2D : MonoBehaviour
{
    public Camera mainCamera; // ָ��������Ƶ����
    public Transform target; // Ŀ������
    public float targetOrthographicSize = 5f; // Ŀ��������С
    public float moveDuration = 2f; // �ƶ�����ʱ��

    private Vector3 initialPosition; // ��ʼλ��
    private float initialOrthographicSize; // ��ʼ������С
    private float elapsedTime = 0f; // �Ѿ�����ʱ��
    private bool isMovingAndZooming = false; // �Ƿ������ƶ�������

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // ���û��ָ���������ʹ�������
        }
        initialPosition = mainCamera.transform.position;
        initialOrthographicSize = mainCamera.orthographicSize;
    }

    void Update()
    {
        if (isMovingAndZooming)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;
            mainCamera.transform.position = Vector3.Lerp(initialPosition, target.position + new Vector3(0, 0, -10), t);
            mainCamera.orthographicSize = Mathf.Lerp(initialOrthographicSize, targetOrthographicSize, t);

            if (t >= 1f)
            {
                isMovingAndZooming = false;
            }
        }
    }

    public void MoveAndZoomCamera(Transform newTarget, float newSize, float duration)
    {
        target = newTarget;
        targetOrthographicSize = newSize;
        moveDuration = duration;
        initialPosition = mainCamera.transform.position;
        initialOrthographicSize = mainCamera.orthographicSize;
        elapsedTime = 0f;
        isMovingAndZooming = true;
    }
}
