using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Speeds")]
    public float walkSpeed = 7f;
    public float sprintSpeed = 14f;
    public float crouchSpeed = 3f;
    private float currentSpeed;

    [Header("Crouch")]
    public float crouchHeight = 1f;
    private float originalHeight;

    [Header("Jumping & Gravity")]
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    private Vector3 velocity;
    private bool isGrounded;

    [Header("Slope & Step Handling")]
    public float slopeLimit = 45f;
    public float stepOffset = 0.5f;

    [Header("References")]
    public CharacterController controller;
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    public Transform cameraTransform;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalHeight = controller.height;
        controller.slopeLimit = slopeLimit;
        controller.stepOffset = stepOffset;

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Camera-relative movement
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Ensure movement is horizontal
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 move = right * x + forward * z;
        move = Vector3.ClampMagnitude(move, 1f);

        // Speed logic
        if (Input.GetKey(KeyCode.C))
        {
            currentSpeed = crouchSpeed;
            controller.height = crouchHeight;
        }
        else
        {
            controller.height = originalHeight;
            currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        }

        // Move player horizontally
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}