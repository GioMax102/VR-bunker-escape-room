using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class VRJump : MonoBehaviour
{
    public InputActionReference jumpAction;
    public Transform groundCheck;
    public LayerMask groundMask;

    public float groundDistance = 0.2f;
    public float jumpHeight = 0.8f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private float verticalVelocity;
    private bool isGrounded;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        if (jumpAction != null)
            jumpAction.action.Enable();
    }

    void OnDisable()
    {
        if (jumpAction != null)
            jumpAction.action.Disable();
    }

    void Update()
    {
        CheckGround();
        HandleJump();
        ApplyGravity();
    }

    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;
    }

    void HandleJump()
    {
        if (jumpAction != null && jumpAction.action.WasPressedThisFrame() && isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void ApplyGravity()
    {
        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }
}