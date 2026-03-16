using UnityEngine;

public class Swinging : MonoBehaviour
{
    [Header("References")]
    public LineRenderer lr;
    public Transform gunTip, cam, player;
    public LayerMask whatIsGrappleable;
    private PlayerMovement pm;

    [Header("Swinging")]
    public float maxSwingDistance = 25f; // ระยะที่สามารถยิงเชือกเกาะได้
    private Vector3 swingPoint;
    private SpringJoint joint;

    [Header("Input")]
    public KeyCode swingKey = KeyCode.Mouse0; // คลิกซ้ายเพื่อโหน

    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        // กดค้างเพื่อโหน / ปล่อยเพื่อหลุดจากเชือก
        if (Input.GetKeyDown(swingKey)) StartSwing();
        if (Input.GetKeyUp(swingKey)) StopSwing();
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    private void StartSwing()
    {
        // ป้องกันไม่ให้กดสลิงพร้อมกับ Grapple
        if (pm.activeGrapple) return;

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxSwingDistance, whatIsGrappleable))
        {
            pm.swinging = true;
            swingPoint = hit.point;

            // สร้าง SpringJoint จำลองแรงดึงของเชือก
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

            // ระยะยืด/หดของเชือก (ปรับตัวคูณเพื่อเปลี่ยนความตึงของเชือกได้)
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            // ค่าความเด้งดึ๋งของเชือก
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.enabled = true;
        }
    }

    private void StopSwing()
    {
        pm.swinging = false;
        lr.enabled = false;
        if (joint != null) Destroy(joint);
    }

    private void DrawRope()
    {
        if (!joint) return;

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, swingPoint);
    }
}