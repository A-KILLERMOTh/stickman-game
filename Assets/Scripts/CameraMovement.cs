using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 4f;

    public float levelLeft;
    public float levelRight;
    public float levelBottom;
    public float levelTop;

    void LateUpdate()
    {
        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        float minX = levelLeft + cameraWidth / 2;
        float maxX = levelRight - cameraWidth / 2;

        float minY = levelBottom + cameraHeight / 2;
        float maxY = levelTop - cameraHeight / 2;

        Vector3 targetPosition = new Vector3(
            Mathf.Clamp(player.position.x, minX, maxX),
            Mathf.Clamp(player.position.y, minY, maxY),
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}