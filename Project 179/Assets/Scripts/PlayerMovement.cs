using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraTransform;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public Animator animator;
    public float speed = 10f;
    public float gravity = -9.81f;
    private Vector3 velocity;
    private float dodgeSpeedMultiplier = 5.0f;
    private float dodgeDuration = 0.15f; 
    private float headDownHeight = 0.3f;
    private float lastDodgeTime;
    private bool isDodging = false;
    private bool isAttacking = false;
    private bool isGrounded;
    private float attackRange = 7.0f;
    
    void Start()
    {
        lastDodgeTime = 0f;
    }
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0.0f)
        {
            velocity.y = -2.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isDodging)
        {
            StartCoroutine(Dodge());
            StartCoroutine(HeadDown());
        }

        if (!isDodging)
        {
            NormalMovement();
        }

        if (Input.GetKeyDown(KeyCode.J) && !isAttacking)
        {
            Attack();
        }

        // Add gravity to the player
        // Equation: Y - Y0 = (1/2) * g * t^2
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void Attack()
    {
        isAttacking = true;
        
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, attackRange))
        {
            Debug.Log("Attacked: " + hit.collider.name);

            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<SandBag>().TakeDamage(50);
                Debug.Log("This is called");
            }
            
            
        }
        isAttacking = false;
    }

    // Combine the Dodge and HeadDown together later if needed
    // Move the player faster for a certain duration, making a dodge feel
    IEnumerator Dodge()
    {
        isDodging = true;
        lastDodgeTime = Time.time;

        float dodgeEndTime = Time.time + dodgeDuration;
        while (Time.time < dodgeEndTime)
        {
            // Same calculation as NormalMovement
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 forward = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
            Vector3 right = new Vector3(cameraTransform.right.x, 0, cameraTransform.right.z).normalized;

            Vector3 directionalInput = new Vector3(x, 0, z);
            if (directionalInput.magnitude < 0.1f)
            {
                Vector3 move = -1 * forward;
                controller.Move(move * speed * dodgeSpeedMultiplier * Time.deltaTime);
            }
            else
            {
                Vector3 move = (right * x + forward * z).normalized; 
                controller.Move(move * speed * dodgeSpeedMultiplier * Time.deltaTime);
            }
            yield return null; 
        }

        isDodging = false;
    }

    // Slightly Lower the head (y-component of camera) when dodging
    // Add a dodge feel when the player dodges
    IEnumerator HeadDown()
    {
        float elapsedTime = 0;
        Vector3 originalPosition = cameraTransform.localPosition;
        Vector3 dodgePosition = originalPosition - new Vector3(0, headDownHeight, 0); 

        // Head's Down
        while (elapsedTime < dodgeDuration / 2)
        {
            cameraTransform.localPosition = Vector3.Lerp(originalPosition, dodgePosition, (elapsedTime / (dodgeDuration / 2)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0; 

        // Head's Up
        while (elapsedTime < dodgeDuration / 2)
        {
            cameraTransform.localPosition = Vector3.Lerp(dodgePosition, originalPosition, (elapsedTime / (dodgeDuration / 2)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraTransform.localPosition = originalPosition; 
    }


    void NormalMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Player movement is based on the camera, not the player body
        // So, change the forward and right directions
        Vector3 forward = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        Vector3 right = new Vector3(cameraTransform.right.x, 0, cameraTransform.right.z).normalized;
        Vector3 move = right * x + forward * z;

        controller.Move(move * speed * Time.deltaTime);
    }

    
}