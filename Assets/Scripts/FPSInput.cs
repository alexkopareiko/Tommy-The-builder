using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class FPSInput : MonoBehaviour
{
    public VariableJoystick joystick;
    public Sprite[] axisJoystickSprites;
    public Image joystickBackground;
    public bool groundedPlayer;
    public Transform forcePoint;
    public float kickForce = 10f;
    public float legLength = 2.2f;

    private Vector3 playerVelocity;
    [SerializeField]
    private float playerSpeed = 5.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    private CharacterController _charController;
    private Animator animator;
    private bool isOnStairs = false;

    void Start()
    {
        _charController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        SetJoystickSprite();
        Move();
    }

    void Move() {
        groundedPlayer = _charController.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0 && !isOnStairs)
        {
            playerVelocity.y = 0f;
        }

        float vertical = isOnStairs ? joystick.Vertical : 0;
        Vector3 move = new Vector3(joystick.Horizontal, vertical, 0);

        _charController.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        _charController.Move(playerVelocity * Time.deltaTime);
        float maxMove = Mathf.Max(Mathf.Abs(joystick.Horizontal), Mathf.Abs(joystick.Vertical));
        animator.SetFloat("speed", maxMove);
    }

    void SetJoystickSprite() {
        if(!isOnStairs) {
            joystick.AxisOptions = AxisOptions.Horizontal;
            joystickBackground.sprite = axisJoystickSprites[1];
        } else {
            joystick.AxisOptions = AxisOptions.Both;
            joystickBackground.sprite = axisJoystickSprites[0];
        }
    }

    public void Attack() {
        StartCoroutine(AttackFunc());
    }

    private IEnumerator AttackFunc() {
        animator.SetLayerWeight(animator.GetLayerIndex("Attack"), 1);
        animator.SetTrigger("kick_leg_1");

        RaycastHit hit;
        Ray forwardRay = new Ray(forcePoint.position, transform.forward);
        Debug.DrawRay(forcePoint.position, transform.forward * 100, Color.green);
        if (Physics.Raycast(forwardRay, out hit))
        {
            Debug.Log(hit.distance);
            hit.transform.gameObject.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            if(hit.distance <= legLength) {
                // hit.rigidbody.AddForceAtPosition(transform.forward * kickForce, hit.transform.position, ForceMode.Acceleration);
                hit.rigidbody.AddForce(transform.forward * kickForce, ForceMode.Acceleration);
            }
        }

        yield return new WaitForSeconds(0.9f);
        animator.SetLayerWeight(animator.GetLayerIndex("Attack"), 0);
    }
}