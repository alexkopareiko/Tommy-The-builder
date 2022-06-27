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
    public float kickCooldown = 0.9f;
    public Transform lookTo;
    public Button jumpButton;
    public Button kickButton;
    public float jumpHeight = 1.0f;
    

    private Vector3 playerVelocity;
    [SerializeField]
    private float playerSpeed = 5.0f;
    private float gravityValue = -9.81f;
    private CharacterController _charController;
    private Animator animator;
    private bool isOnStairs = false;
    private bool recentAttack = false;

    void Start()
    {
        _charController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        jumpButton.onClick.AddListener(Jump);
        kickButton.onClick.AddListener(Attack);
    }
    void Update()
    {
        SetJoystickSprite();
        Move();
    }

    void Jump() {
        if(_charController.isGrounded) {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.SetLayerWeight(animator.GetLayerIndex("Jump"), 1);
            animator.SetTrigger("jump");
        }
    }

    void Move() {
        groundedPlayer = _charController.isGrounded;
        animator.SetBool("grounded", _charController.isGrounded);
        if (groundedPlayer && playerVelocity.y < 0 && !isOnStairs)
        {
            animator.SetLayerWeight(animator.GetLayerIndex("Jump"), 0);
            playerVelocity.y = 0f;
        }

        float vertical = isOnStairs ? joystick.Vertical : 0;
        Vector3 move = new Vector3(joystick.Horizontal, vertical, 0);

        _charController.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
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
        if(!recentAttack)
            StartCoroutine(AttackFunc());
    }

    private IEnumerator AttackFunc() {
        animator.SetLayerWeight(animator.GetLayerIndex("Attack"), 1);
        animator.SetTrigger("kick_leg_1");

        RaycastHit hit;
        Ray forwardRay = new Ray(forcePoint.position, transform.forward);
        if (Physics.Raycast(forwardRay, out hit))
        {
            if(hit.distance <= legLength && hit.rigidbody) {
                hit.rigidbody.AddForce(transform.forward * kickForce, ForceMode.Acceleration);
            }
        }
        recentAttack = true;
        yield return new WaitForSeconds(kickCooldown);
        animator.SetLayerWeight(animator.GetLayerIndex("Attack"), 0);
        recentAttack = false;
    }
}