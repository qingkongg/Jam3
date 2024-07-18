using UnityEngine;

public class CameraZoomAndMove2D : MonoBehaviour
{
    public Camera mainCamera; // 指向你想控制的相机
    public Transform target; // 目标物体
    public float targetOrthographicSize = 5f; // 目标正交大小
    public float moveDuration = 2f; // 移动持续时间

    private Vector3 initialPosition; // 初始位置
    private float initialOrthographicSize; // 初始正交大小
    private float elapsedTime = 0f; // 已经过的时间
    private bool isMovingAndZooming = false; // 是否正在移动和缩放

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // 如果没有指定相机，则使用主相机
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
