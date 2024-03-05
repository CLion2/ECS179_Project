using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 nextPosition;

    void Start()
    {
        SetNextLocation();
    }

    // Update is called every frame
    void Update()
    {
        // Move to the next location set by SetNextLocation
        float step = speed * Time.deltaTime; 
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, step);

        // If Enemy reaches the next location, call SetNextLocation again
        // so that Enemy updates the next location
        if (Vector3.Distance(transform.position, nextPosition) < 0.1f)
        {
            SetNextLocation();
        }
    }

    // Choose the next location at random but within the range
    void SetNextLocation()
    {
        float range = 10f;
        nextPosition = new Vector3(Random.Range(-range, range), 3f, Random.Range(-range, range));

    }
}
