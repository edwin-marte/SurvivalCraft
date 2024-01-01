using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FirstPersonController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Transform head;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    public Animator armAnimator;
    public Animator headAnimator;
    public Animator storageAnimator;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    private bool canJump = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        bool isMoving = (new Vector3(horizontalInput, 0f, verticalInput).magnitude > 0.1f);

        // Press Left Shift to run
        bool isRunning = (Input.GetKey(KeyCode.LeftShift) && isMoving);

        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * verticalInput : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * horizontalInput : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded && canJump)
        {
            moveDirection.y = jumpSpeed;
            canJump = false;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (Input.GetButtonUp("Jump"))
        {
            canJump = true;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        if (characterController.isGrounded)
        {
            if (isMoving && !isRunning)
            {
                // Walk
                armAnimator.SetFloat("Speed", 0.5f, 0.2f, Time.deltaTime);
                headAnimator.SetFloat("Speed", 0.5f, 0.2f, Time.deltaTime);
            }
            else if (isMoving && isRunning)
            {
                // Running
                armAnimator.SetFloat("Speed", 1f, 0.2f, Time.deltaTime);
                headAnimator.SetFloat("Speed", 1f, 0.2f, Time.deltaTime);
            }
            else if (!isMoving && !isRunning)
            {
                // Idle
                armAnimator.SetFloat("Speed", 0f, 0.6f, Time.deltaTime);
                headAnimator.SetFloat("Speed", 0f, 0.6f, Time.deltaTime);
            }
        }
        else
        {
            // Set Idle On Jumping
            armAnimator.SetFloat("Speed", 0f, 0.01f, Time.deltaTime);
        }

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            head.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        Interaction();
    }

    private void Interaction()
    {
        float raycastDistance = 2f;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                if (hit.collider.gameObject.tag == "Storage")
                {
                    storageAnimator.SetBool("chestOpen", true);
                }

                if (Input.GetMouseButtonDown(0))
                {
                    interactable.Interact();
                }
            }
        }
        else
        {
            storageAnimator.SetBool("chestOpen", false);
        }
    }
}
