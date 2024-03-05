using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraTransform;
    public float speed = 10f;
    private float dodgeSpeedMultiplier = 3.5f;
    private float dodgeDuration = 0.2f; 
    private float headDownHeight = 0.25f;
    private float lastDodgeTime;
    private bool isDodging = false;
    
    void Start()
    {
        lastDodgeTime = 0f;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDodging)
        {
            StartCoroutine(Dodge());
            StartCoroutine(HeadDown());
        }

        if (!isDodging)
        {
            NormalMovement();
        }
    }

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
            Vector3 move = (right * x + forward * z).normalized; 

            controller.Move(move * speed * dodgeSpeedMultiplier * Time.deltaTime);
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