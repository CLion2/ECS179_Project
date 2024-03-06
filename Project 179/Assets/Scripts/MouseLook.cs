using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 1500f;
    [SerializeField] private float angleSpeed = 10f;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform lockTarget;
    bool isLockedOnTarget = false;
    float xRotation = 0f;
    private Quaternion lastRotation;
    

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lastRotation = transform.rotation; 
    }

    void Update()
    {
        // Pressing the L key toggles on and off for camera lock feature
        if (Input.GetKeyDown(KeyCode.L))
        {
            isLockedOnTarget = !isLockedOnTarget;
            if (!isLockedOnTarget)
            {
                lastRotation = transform.rotation;
            }
        }

        // Manual camera movement if camera is not locked
        if (!isLockedOnTarget)
        {
            ManualCameraMovement();
        }
    
        else if (lockTarget != null)
        {
            // Lock the camera @ the target 
            //PositionLockCamera();

        }

    }

    void LateUpdate()
    {
        if (isLockedOnTarget && lockTarget != null)
        {
            PostionFollowCamera();
        }
        else if (!isLockedOnTarget && lockTarget != null)
        {
            transform.rotation = lastRotation;
        }
    }

    void ManualCameraMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        transform.rotation = lastRotation;
    }

    // Lock the camera @ the target
    void PositionLockCamera()
    {
        transform.LookAt(lockTarget);
    }

    // Need Adjustment
    // Follow the target
    void PostionFollowCamera()
    {
        Quaternion targetRotation = Quaternion.LookRotation(lockTarget.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, angleSpeed * Time.deltaTime);
    }

}
