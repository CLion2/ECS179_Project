using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 1500f;
    [SerializeField] private float angleSpeed = 10f;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform lockTarget;
    [SerializeField] public enum LockTargetEnemy {NONE, PRISONER, BOSS, GUARD};
    [SerializeField] private List<string> tagList = new List<string>{"Untagged", "PrisonerAnchor", "GladiatorAnchor","GuardAnchor"};
    bool isLockedOnTarget = false;
    [SerializeField] private LockTargetEnemy currentTarget = LockTargetEnemy.PRISONER;
    float xRotation = 0f;
    private Quaternion lastRotation;
    

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void SetTargetLocking(int lockTargetEnum)
    {
        isLockedOnTarget = true;
        if (lockTargetEnum != 0)
        {
            currentTarget = (LockTargetEnemy)lockTargetEnum;
            lockTarget = GameObject.FindWithTag(tagList[(int)currentTarget]).transform;
        }
    }
    void Update()
    {
        // Pressing the L key toggles on and off for camera lock feature
        if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.Mouse2))
        {
            isLockedOnTarget = !isLockedOnTarget;
            // GameObject Enemy = GameObject.FindGameObjectsWithTag("Prisoner");
            // lockTarget = Enemy.transform;
            lockTarget = GameObject.FindWithTag(tagList[(int)currentTarget]).transform;
        }

        // Manual camera movement if camera is not locked
        if (!isLockedOnTarget)
        {
            ManualCameraMovement();
        }
    
        else if (lockTarget != null)
        {
            // Lock the camera @ the target 
            // PositionLockCamera();

        }

    }

    void LateUpdate()
    {
        if (isLockedOnTarget && lockTarget != null)
        {
            PostionFollowCamera();
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

    }

    // Lock the camera @ the target
    void PositionLockCamera()
    {
        transform.LookAt(lockTarget);
    }

    void LockOnPrisoner()
    {
        lockTarget = GameObject.FindWithTag("PrisonerAnchor").transform;
    }
    void LockOnBoss()
    {
        lockTarget = GameObject.FindWithTag("GladiatorAnchor").transform;
    }

    // Need Adjustment
    // Follow the target
    void PostionFollowCamera()
    {
        Quaternion targetRotation = Quaternion.LookRotation(lockTarget.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, angleSpeed * Time.deltaTime);
    }

}
