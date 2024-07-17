using UnityEngine;

public class CameraMoveAndZoom : MonoBehaviour
{
    public Transform targetPoint; // Ŀ����Transform
    public float moveSpeed = 5f; // ����ƶ��ٶ�
    public float zoomSpeed = 2f; // ����Ŵ��ٶ�
    public float targetOrthographicSize = 2f; // Ŀ����Ұ��С�������������������
    public float targetFOV = 30f; // Ŀ����Ұ��С����������͸�������

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

        // �ƶ����
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        // ����Ƿ񵽴�Ŀ���
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            transform.position = targetPoint.position;
            hasReachedTarget = true;
        }

        // ���������Ұ
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

        // ����Ƿ�ͬʱ����Ŀ��λ�ú�����Ŀ��
        if (hasReachedTarget &&
            ((isOrthographic && cam.orthographicSize == targetOrthographicSize) ||
             (!isOrthographic && cam.fieldOfView == targetFOV)))
        {
            enabled = false; // ֹͣ����
        }
    }
}


