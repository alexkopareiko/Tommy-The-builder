using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class ThirdPersonMovement2 : MonoBehaviour
{
    public Transform cam;
    public bool groundedPlayer;
    public float groundedLength = 0.05f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -30f;
    public float playerSpeed = 6f;
    public float turnSmoothTime = 0.1f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    

    private Vector3 playerVelocity;
    private Animator animator;
    float turnSmoothVelocity;
    private AudioSource audioSource;
    CharacterController controller;


    private void Start() {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();
        controller = GetComponentInChildren<CharacterController>();
        Cursor.visible = true;
    }

    private void Update() {
        Move();
    }


    void Move() {
        groundedPlayer = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(groundedPlayer && playerVelocity.y < 0) {
            playerVelocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f) {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            direction = Quaternion.Euler(0f, targetAngle, 0f) * transform.forward;
            controller.Move(direction * playerSpeed * Time.deltaTime);
        }

        if(Input.GetMouseButton(1) && Input.GetMouseButton(0)) {
            transform.forward = new Vector3(cam.forward.x, 0, cam.forward.z);
        }

        if(groundedPlayer && Input.GetButtonDown("Jump")) {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;

        controller.Move(playerVelocity * Time.deltaTime);
    }
}
