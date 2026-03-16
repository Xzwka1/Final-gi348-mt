using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;          // ความเร็วตอนเดิน
    public float jumpHeight = 1.5f;   // ความสูงตอนกระโดด
    public float gravity = -15f;      // แรงโน้มถ่วง
    public float mouseSensitivity = 200f; // ความไวเมาส์

    [Header("References (ห้ามลืมใส่!)")]
    public Transform playerCamera;    // *** ต้องลาก Main Camera มาใส่ช่องนี้ ***

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // เช็คว่าลืมใส่กล้องไหม ถ้าลืมให้แจ้งเตือนสีแดง
        if (playerCamera == null)
        {
            Debug.LogError("คุณยังไม่ได้ลาก Main Camera มาใส่ในช่อง Player Camera ที่สคริปต์ครับ!");
        }

        // ล็อคเมาส์ซ่อนไว้กลางจอเวลาเล่น
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 1. เช็คว่าตัวละครเหยียบพื้นอยู่ไหม
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 2. หมุนกล้องขึ้นลงและหันซ้ายขวา
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // หันขึ้นลง (แกน X ของกล้อง)
        xRotation -= mouseY;

        // หมายเหตุ: ตรงนี้คือการล็อคคอไม่ให้เงยหรือก้มเกิน 90 องศา (ไม่ให้ตีลังกา)
        // ถ้าอยากให้อิสระแบบหมุนคอได้ 360 องศา ให้ลบหรือใส่ // หน้าบรรทัด Mathf.Clamp นี้ครับ
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        // หันซ้ายขวา (แกน Y ของตัวละคร)
        transform.Rotate(Vector3.up * mouseX);

        // 3. ระบบเดิน (WASD)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // 4. ระบบกระโดด (Spacebar)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 5. คำนวณแรงโน้มถ่วงให้ตกลงมา
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}