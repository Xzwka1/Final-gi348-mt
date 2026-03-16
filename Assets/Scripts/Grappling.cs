using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private PlayerMovement pm;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;

    [Header("Grappling")]
    public float maxGrappleDistance = 25f; // ระยะยิงสูงสุด
    public float grappleDelayTime = 0.2f;  // ดีเลย์ก่อนกระตุกเชือกดึงตัว
    public float overshootYAxis = 2f;      // ความสูงของการกระโดด (ยิ่งเยอะยิ่งลอยสูง)

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    public float grapplingCd = 2.5f; // คูลดาวน์การยิง
    private float grapplingCdTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1; // คลิกขวาเพื่อยิง

    private bool grappling;

    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(grappleKey)) StartGrapple();

        if (grapplingCdTimer > 0)
            grapplingCdTimer -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (grappling)
            lr.SetPosition(0, gunTip.position);
    }

    private void StartGrapple()
    {
        if (grapplingCdTimer > 0) return;

        grappling = true;
        pm.freeze = true; // หยุดตัวละครกลางอากาศแป๊บนึงตอนยิงสลิง

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }

    private void ExecuteGrapple()
    {
        pm.freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        pm.JumpToPosition(grapplePoint, highestPointOnArc);
        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple()
    {
        pm.freeze = false;
        grappling = false;
        grapplingCdTimer = grapplingCd;
        lr.enabled = false;
    }
}