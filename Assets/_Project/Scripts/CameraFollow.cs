
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public Vector3 offset = new Vector3(0, 4, -8);

    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (player == null)
            return;

        // Follow the player forward while staying centered
        Vector3 targetPosition = new Vector3(
            0,
            player.position.y + offset.y,
            player.position.z + offset.z
        );

        // Smoothly move the camera toward the target
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}
