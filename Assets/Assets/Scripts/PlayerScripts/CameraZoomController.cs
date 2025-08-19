using UnityEngine;
using Unity.Cinemachine;

public class CameraZoomController : MonoBehaviour
{
    public CinemachineCamera virtualCamera;
    public float zoomSpeed = 2f;
    public float minZoom = 5f;
    public float maxZoom = 15f;

    private void Update()
    {
        if (virtualCamera == null) return;

        float currentSize = virtualCamera.Lens.OrthographicSize;

        // Mouse scroll zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentSize -= scroll * zoomSpeed;

        // Keyboard zoom
        if (Input.GetKey(KeyCode.Equals) || Input.GetKey(KeyCode.Plus)) // '+' key
        {
            currentSize -= zoomSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.Underscore)) // '-' key
        {
            currentSize += zoomSpeed * Time.deltaTime;
        }

        // Clamp the size
        currentSize = Mathf.Clamp(currentSize, minZoom, maxZoom);

        virtualCamera.Lens.OrthographicSize = currentSize;
    }
}