using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera camera;
    public float zoomSpeed = 10f;
    public float minZoom = 20f;
    public float maxZoom = 60f;

    public Transform Target;
    public float SpeedFactor;

    private int MoveTimes= 10;
    private int m_nowTime = 0;

    void Update()
    {
        Vector3 offset = transform.position - camera.transform.position;
        camera.fieldOfView -= SpeedFactor * zoomSpeed;
        camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, minZoom, maxZoom);
        
        if(m_nowTime < MoveTimes)
        {
            m_nowTime += 1;
            camera.transform.position += offset * Mathf.Pow(SpeedFactor, m_nowTime);
        }
    }
}
