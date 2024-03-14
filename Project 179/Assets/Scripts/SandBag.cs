using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandBag : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("TakeDamege called");

        if (currentHealth <= 0) {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
        Debug.Log("Object Destroyed");
    }

    
    void Update()
    {
        
    }
}
