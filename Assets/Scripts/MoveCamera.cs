using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;

    void Update()
    {
        // สั่งให้ CameraHolder บินตามตำแหน่งตาของ Player ตลอดเวลา
        transform.position = cameraPosition.position;
    }
}