using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    private float DamageTaken = 20f;
    private float Rage = 10f;
    private bool Stage1;
    private float Delay = 0.5f; // delay when out of range
    private Prisoner Tutorial;
    private Gladiator Boss;
    private bool Stage1Done;
    [SerializeField] private float TetherDistance = 5f; // set it to nav mesh
    [SerializeField] private float speed = 5f; // set this to nav mesh
    // private GameObject Player;

    // following a vid here
    public NavMeshAgent navMeshAgent;
    public Transform playerTransform;
    public Animator animator; // idk following vid here
    // TODO: implement wakeup

    void Start()
    { // should work the second its created
        Stage1 = true;
        Stage1Done = false;
        Tutorial = new Prisoner();
        Boss = new Gladiator();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = this.speed;
        // navMeshAgent.
    }
    // void LookAtPlayer(Vector3 player)
    // {
    //     navMeshAgent.SetDestination(player);
    //     if(Vector3.Distance(transofrm.position, player) <= 0.3f)
    //     {
    //         if(waitTime <= 0)
    //         {
    //             NearPlayer = false;
    //             Move(this.speed);
    //         }
    //     }
    // }
    void Attack()
    {
        if(Stage1 == true) // then we are in tutorial
        {

        }
        else if(Stage1 == false)// then we are in boss fight
        {

        }
    }
    void CheckDead()
    {
        if(Stage1 == true && Tutorial.Health == 0)
        {
            Stage1Done = true;
            Stage1 = false;
            // need to add in a death animation or ragdoll here
        }
        else if(Stage1 == false && Boss.Health == 0)
        {
            // boss is dead then add in a death animation or ragdoll here
        }
    }

    void TakeDamage() // TAkeDamage deals with the enemies direct health
    {
        if(Stage1 == true && Stage1Done == false)
        {
            Tutorial.Health = Tutorial.Health - DamageTaken;
        }
        else if(Stage1Done == true && Stage1 == false)
        {
            Boss.Health = Boss.Health - DamageTaken; // decrease health
            Boss.RageMeter = Boss.RageMeter + Rage; // increase ragemeter
            if(Boss.RageMeter == 100f)
            {
                Boss.AttackDmg = Boss.AttackDmg + 5;
                Boss.AttackSpd = Boss.AttackSpd + 0.5f;
            }
            else if(Boss.RageMeter < 100f && Boss.RageMeter >= 50f)
            {
                Boss.AttackDmg = Boss.AttackDmg + 5;
                Boss.AttackSpd = Boss.AttackSpd + 0.5f;
            }
        }
    }
    void Block()
    {

    }
    void Update()
    { 
        // TODO: add delay when distance is 5
        // TODO: add look at player
        // TODO: make sure it works with prisoner and gladiator
        // TODO: make sure that when in stage1 then gladiator doesnt move
        if(Stage1 == true)
        {
            if(navMeshAgent.remainingDistance == 5)
            {
                Debug.Log("distance is 0");
            }
            navMeshAgent.SetDestination(playerTransform.position);

            //TODO: add attack
        }
        else if(Stage1 == false)
        {
            navMeshAgent.SetDestination(playerTransform.position);
            
            //TODO: add attack 
        }
    }
}