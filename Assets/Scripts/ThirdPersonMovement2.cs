using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement2 : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;

    float turnSmoothVelocity;


    private void Update() {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f) {
            // float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            // transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * transform.forward;
            // controller.Move(moveDir.normalized * speed * Time.deltaTime);
            controller.Move(moveDir * speed * Time.deltaTime);
        }

        if(Input.GetMouseButton(1) && Input.GetMouseButton(0)) {
            transform.forward = new Vector3(cam.forward.x, 0, cam.forward.z);
        }
    }
}