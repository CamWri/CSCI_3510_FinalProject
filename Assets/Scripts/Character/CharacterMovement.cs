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
    private bool jumpRequested;

    [Header("Slope & Step Handling")]
    public float slopeLimit = 45f;
    public float stepOffset = 0.5f;

    [Header("References")]
    public CharacterController controller;
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;       // primary ground
    public LayerMask extraGroundMask;  // secondary ground
    public Transform cameraTransform;

    private bool isGrounded;
    private Vector3 moveInput;

    public static CharacterMovement Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

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
        // --- INPUT HANDLING ---

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        bool sprintPressed = Input.GetKey(KeyCode.LeftShift);
        bool crouchPressed = Input.GetKey(KeyCode.C);

        // Camera-relative movement
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        moveInput = right * x + forward * z;
        if (moveInput.magnitude > 1f)
            moveInput.Normalize();

        // Speed & crouch
        if (crouchPressed && controller.height != crouchHeight)
            controller.height = crouchHeight;
        else if (!crouchPressed && controller.height != originalHeight)
            controller.height = originalHeight;

        currentSpeed = crouchPressed ? crouchSpeed : (sprintPressed ? sprintSpeed : walkSpeed);

        // Jump input
        if (Input.GetButtonDown("Jump"))
            jumpRequested = true;
    }

    void FixedUpdate()
    {
        // --- GROUND CHECK ---
        int combinedMask = groundMask | extraGroundMask;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, combinedMask);

        if (isGrounded && velocity.y < 0f)
            velocity.y = -2f; // small downward force to stick to ground

        // --- MOVEMENT ---
        controller.Move(moveInput * currentSpeed * Time.fixedDeltaTime);

        // --- JUMPING ---
        if (jumpRequested && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpRequested = false;
        }

        // --- GRAVITY ---
        if (!isGrounded)
            velocity.y += gravity * Time.fixedDeltaTime;

        controller.Move(velocity * Time.fixedDeltaTime);
    }
}