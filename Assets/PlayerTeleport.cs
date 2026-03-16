using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    [Header("Settings")]
    public Transform playerCamera;    // ลาก Main Camera มาใส่
    public float interactDistance = 15f; // ระยะที่มองเห็นและกดวาปได้
    public GameObject uiPrompt;       // ลาก UI Text มาใส่ช่องนี้

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // ซ่อนข้อความไว้ก่อนตอนเริ่มเกม
        if (uiPrompt != null)
        {
            uiPrompt.SetActive(false);
        }
    }

    void Update()
    {
        RaycastHit hit;
        // ยิงเลเซอร์ล่องหนจากกล้องพุ่งตรงไปข้างหน้า
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, interactDistance))
        {
            // ถ้าเป้าหมายที่มองอยู่ มี Tag ชื่อว่า "TeleportPoint"
            if (hit.collider.CompareTag("TeleportPoint"))
            {
                // โชว์ข้อความ "Press Q to TP"
                if (uiPrompt != null) uiPrompt.SetActive(true);

                // ถ้ายืนยันการกดปุ่ม Q
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    TeleportTo(hit.transform.position);
                }
            }
            else
            {
                // ถ้ามองโดนอย่างอื่น ให้ปิดข้อความ
                if (uiPrompt != null) uiPrompt.SetActive(false);
            }
        }
        else
        {
            // ถ้ามองฟ้า มองอากาศ ให้ปิดข้อความ
            if (uiPrompt != null) uiPrompt.SetActive(false);
        }
    }

    void TeleportTo(Vector3 targetPos)
    {
        // กฎของ Unity: ต้องปิด Character Controller ก่อนวาป ไม่งั้นมันจะดึงตัวกลับมาที่เดิม
        controller.enabled = false;

        // วาปตัวละครไปที่เป้าหมาย (+ แกน Y ให้ลอยขึ้นมา 2 เมตร จะได้ไม่จมลงไปในก้อนนั้น)
        transform.position = targetPos + new Vector3(0, 2f, 0);

        // เปิด Controller กลับมาให้เดินต่อได้
        controller.enabled = true;
    }
}