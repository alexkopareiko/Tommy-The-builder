using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.NikfortGames.MyGame {
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
    public class ThirdPersonMovement : MonoBehaviour
    {

        #region Public Fields
        public bool groundedPlayer;
        public float groundedLength = 0.05f;
        public float jumpHeight = 1.0f;
        public float gravityValue = -30f;
        public float playerSpeed = 6f;
        public float turnSmoothTime = 0.1f;

        public Transform groundCheck;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;
        public CharacterController controller;
        public Transform cameraLookHere;
        public PhotonView photonView;

        #endregion

        
        #region Private Fields

        private Transform cam;
        private Vector3 playerVelocity;
        private Animator animator;
        float turnSmoothVelocity;
        private AudioSource audioSource;
        private Player player;

        #endregion


        #region MoboBehaviour Callbacks
        private void Start() {
            photonView = GetComponentInChildren<PhotonView>();
            audioSource = GetComponent<AudioSource>();
            animator = GetComponentInChildren<Animator>();
            controller = GetComponent<CharacterController>();
            player = GetComponent<Player>();
            cam = FindObjectOfType<Camera>().transform;
        }

        private void Update() {
            if(player.currentHealth > 0) {
                if(!controller.enabled) controller.enabled = true;
                if(photonView.IsMine) Move();
            } else {
                if(controller.enabled) controller.enabled = false;
            }
        }

        #endregion


        #region Private Methods
        void Move() {
            groundedPlayer = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            // groundedPlayer = controller.isGrounded;
            animator.SetBool("grounded", groundedPlayer);
            if(groundedPlayer && playerVelocity.y < 0) {
                animator.SetLayerWeight(animator.GetLayerIndex("Jump"), 0);
                playerVelocity.y = -2f;
            }

            float horizontal = Input.GetAxisRaw("Horizontal") * 0.5f;
            float vertical = Input.GetAxisRaw("Vertical");

            vertical = vertical < 0 ? vertical * 0.5f: vertical;
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
                animator.SetLayerWeight(animator.GetLayerIndex("Jump"), 1);
                animator.SetTrigger("jump");
            }

            playerVelocity.y += gravityValue * Time.deltaTime;

            controller.Move(playerVelocity * Time.deltaTime);
            animator.SetFloat("vertical", vertical);
            animator.SetFloat("horizontal", horizontal);
        }

        #endregion
        
    }



}

