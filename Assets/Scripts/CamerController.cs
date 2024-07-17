using UnityEngine;

public class CameraMoveAndZoom : MonoBehaviour
{
    public Transform targetPoint; // 目标点的Transform
    public float moveSpeed = 5f; // 相机移动速度
    public float zoomSpeed = 2f; // 相机放大速度
    public float targetOrthographicSize = 2f; // 目标视野大小（仅适用于正交相机）
    public float targetFOV = 30f; // 目标视野大小（仅适用于透视相机）

    private Camera cam;
    private bool isOrthographic;
    private bool hasReachedTarget = false;

    void Start()
    {
        cam = GetComponent<Camera>();
        isOrthographic = cam.orthographic;
    }

    void Update()
    {
        if (hasReachedTarget) return;

        // 移动相机
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        // 检查是否到达目标点
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            transform.position = targetPoint.position;
            hasReachedTarget = true;
        }

        // 缩放相机视野
        if (isOrthographic)
        {
            cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, targetOrthographicSize, zoomSpeed * Time.deltaTime);
            if (Mathf.Abs(cam.orthographicSize - targetOrthographicSize) < 0.1f)
            {
                cam.orthographicSize = targetOrthographicSize;
            }
        }
        else
        {
            cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);
            if (Mathf.Abs(cam.fieldOfView - targetFOV) < 0.1f)
            {
                cam.fieldOfView = targetFOV;
            }
        }

        // 检查是否同时到达目标位置和缩放目标
        if (hasReachedTarget &&
            ((isOrthographic && cam.orthographicSize == targetOrthographicSize) ||
             (!isOrthographic && cam.fieldOfView == targetFOV)))
        {
            enabled = false; // 停止更新
        }
    }
}


