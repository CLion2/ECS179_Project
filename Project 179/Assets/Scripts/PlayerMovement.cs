using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Bar PlayerHealth;
    [SerializeField] private float maxhHealth = 100.0f;
    [SerializeField] private Bar PlayerStamina;
    [SerializeField] private float maxStamina = 50.0f;

    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cameraTransform;
    // [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Animator animator;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float gravity = -9.81f;
    private float currentHealth;
    private float currentStamina;
    private Vector3 velocity;
    private float dodgeSpeedMultiplier = 5.0f;
    private float dodgeDuration = 0.15f; 
    private float headDownHeight = 0.3f;
    private float lastDodgeTime;
    private bool isDodging = false;
    private bool isAttacking = false;
    private bool isBlocking = false;
    private bool isGrounded;
    private float attackRange = 7.0f;
    
    
    void Start()
    {
        lastDodgeTime = 0f;
        currentHealth = maxhHealth;
        currentStamina = maxStamina;
        
    }
    void Update()
    {
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        // if (isGrounded && velocity.y < 0.0f)
        // {
        //     velocity.y = -2.0f;
        // }
        velocity.y = -2.0f;

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

        if (Input.GetKeyDown(KeyCode.I))
        {
            Block();
        }

        // Add gravity to the player
        // Equation: Y - Y0 = (1/2) * g * t^2
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //Debug for now to check health
    }

    void Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, attackRange))
        {
            // Debug.Log("Attacked: " + hit.collider.name);
            // Debug.Log("enemy Tag is: " + hit.collider.tag);

            if (hit.collider.tag =="Prisoner" || hit.collider.tag == "Gladiator")
            {
                // Debug.Log("got into the attack");
                hit.collider.GetComponent<EnemyAi>().EnemyTakeDamage(50f); // probably to high for right now
                // Debug.Log("This is called");
            }
            
            
            
        }
        isAttacking = false;
        
    }

    void Block()
    {
        isBlocking = true;
        animator.SetTrigger("Block");
        // Block Logic Here
        
        isBlocking = false;
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

    void GameOver()
    {
        // Call the Game Over screen
    }

    public void TakeDamage (float damage)
    {
        // Damega = 0 if isDodging and dodgingTimer < 0.1f
        if (isDodging)
        {
            damage = 0f;
        }

        if (isBlocking)
        {
            damage = 0f;
            // Add Blocking Sound Here
        }
        currentHealth -= damage;

        if (currentHealth < 0f)
        {
            GameOver();
        }
        // Debug.Log("health is: " + currentHealth);
    }

    
}