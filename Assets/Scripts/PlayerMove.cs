using UnityEngine;


public class PlayerMove : MonoBehaviour
{
    Animator anim;
    public Transform cameraTransform;
    public CharacterController characterController;
    public GameManager gameManager;
    public float moveSpeed = 10f;
    public float jumpSpeed = 10f;
    public float gravity = -20f;
    public float yVelocity = 0;
    public float dashT = 2f;
    float coltime = 0;
    public float dashSpeed = 10f;
    Vector3 d_dir;

    [SerializeField] private float mouseSpeed = 8f;
    private float mouseX = 180f;
    private float mouseY = 0f;

    void Start()
    {
        anim = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Function()
    {
        Move();
        Mouse();
    }
    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
        {
            anim.SetBool("Moving", true);
        }
        else
        {
            anim.SetBool("Moving", false);
        }

        Vector3 moveDirection = new Vector3(h, 0, v);

        moveDirection = cameraTransform.TransformDirection(moveDirection);

        if (coltime < 0 && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonDown(1)))
        {
            anim.SetTrigger("Dash");
            coltime = dashT;
            d_dir = moveDirection;
        }
        else if (coltime > dashT * 0.67f)
        {
            if (anim.GetBool("Moving"))
            {
                characterController.Move(d_dir * Time.deltaTime * dashSpeed);
            }
            else
            {
                characterController.Move(transform.forward * Time.deltaTime * dashSpeed);
            }
            coltime -= Time.deltaTime;
        }
        else
        {
            coltime -= Time.deltaTime;
        }

        moveDirection *= moveSpeed;

        if (characterController.isGrounded)
        {
            yVelocity = 0;
            anim.SetBool("jumping", false);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetBool("jumping", true);
                yVelocity = jumpSpeed;
            }
        }

        yVelocity += (gravity * Time.deltaTime);

        moveDirection.y = yVelocity;
        Vector3.Normalize(moveDirection);

        characterController.Move(moveDirection * Time.deltaTime);
    }
    void Mouse()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        mouseY += Input.GetAxis("Mouse Y") * mouseSpeed * -1f;

        mouseY = Mathf.Clamp(mouseY, -75f, 80f);

        transform.localEulerAngles = new Vector3(mouseY, mouseX, 0);
    }
}
