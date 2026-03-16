using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX = 400f;
    public float sensY = 400f;
    public Transform orientation;

    float xRotation;
    float yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // ล็อคคอไม่ให้กล้องตีลังกา

        // หมุนกล้องขึ้นลง และหมุน Orientation ให้หันซ้ายขวาตามเมาส์
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}