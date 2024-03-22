// Debug Script currently not in use
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUDController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Bar playerHealth;//gameobject reference
    [SerializeField]
    private GameObject playerStamina;
    [SerializeField]
    private GameObject playerToolTip;
    [SerializeField]
    private Bar enemyHealth;
    [SerializeField]
    private GameObject enemyTitle;
    private float maxHealth = 100f;
    private float currentHealth;
    private bool HPactive = true;
    private bool enemyActive = false;
    void Start()
    {
        currentHealth = maxHealth;
        playerHealth.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentHealth -= 20;
            playerHealth.SetHealth(currentHealth);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            HPactive = !HPactive;
            playerHealth.gameObject.SetActive(HPactive);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            enemyActive = !enemyActive;
            playerHealth.gameObject.SetActive(enemyActive);
        }
    }
}
