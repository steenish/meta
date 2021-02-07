using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float walkingSpeed;
    [SerializeField]
    private float runningSpeed;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private float gravityMultiplier;
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private float lookSpeed;
    [SerializeField]
    private float lookXLimit;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0.0f;

    private bool canMove = true;

    private void Start() {
        characterController = GetComponent<CharacterController>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0.0f;
        float curSpeedZ = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0.0f;
        float movementDirectionY = moveDirection.y;
        moveDirection = forward * curSpeedX + right * curSpeedZ;

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded) {
            moveDirection.y = jumpSpeed;
        } else {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded) {
            moveDirection.y -= Physics.gravity.y * gravityMultiplier;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove) {
            rotationX -= Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0.0f, 0.0f);
            transform.rotation *= Quaternion.Euler(0.0f, Input.GetAxis("Mouse X") * lookSpeed, 0.0f);
        }
    }
}
