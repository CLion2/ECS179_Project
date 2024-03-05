using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5f;
    public Transform target;
    
    // Update is called every frame
    void Update()
    {
        float step = speed * Time.deltaTime; 
    
        // If the enemy is close to the player
        if (Vector3.Distance(transform.position, target.position) < 3.0f)
        {
            // The enemy is stationaty
        }
        else
        {
            // The enemy moves toward the player
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }
    }

    
}
