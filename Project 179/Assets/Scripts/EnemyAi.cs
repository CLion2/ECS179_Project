using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private float DamageTaken = 20f;
    private float Rage = 10f;
    private float Delay = 0.5f; // delay when out of range
    private Prisoner Tutorial;
    private Gladiator Boss;
    private bool Stage1Done;
    [SerializeField] private float TetherDistance = 5f; // set it to nav mesh
    [SerializeField] private float speed = 5f; // set this to nav mesh
    // private GameObject Player;

    // following a vid here
    public NavMeshAgent navMeshAgent;
    public GameObject PlayerLocation;
    // public Transform playerTransform;
    public Animator animator; // idk following vid here
    BoxCollider EnemyAttack;
    // TODO: implement wakeup

    void Start()
    { // should work the second its created
        Stage1Done = false;
        Tutorial = new Prisoner();
        Boss = new Gladiator();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = this.speed;
        navMeshAgent.stoppingDistance = this.TetherDistance;
        PlayerLocation = GameObject.Find("Player Body");
        EnemyAttack = GetComponentInChildren<BoxCollider>();
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
    public void Attack()
    // TODO: physics.Raycast for attacking with the swords
    {
        float AttackRoll = Random.Range(0f,10f);
        EnableAttack();
        if(Stage1Done == false) // then we are in tutorial
        {
            if(AttackRoll <= 6f) // then attack goes through
            {
                Debug.Log("Attack is done");
                //TODO: get information from soma and then finish up attack here
                // OnAttackCheck(); // not fully done

            }
        }
        else if(Stage1Done == true)// then we are in boss fight
        {
            if(AttackRoll <= 6f) // then attack goes through
            {
                // OnAttackCheck(); // not fully done
            }
            else // then block
            {
                Block();
            }
        }
        DisableAttack();
    }
    void EnableAttack()
    {
        EnemyAttack.enabled = true;
    }
    void DisableAttack()
    {
        EnemyAttack.enabled = false;
    }
    void OnAttackCheck(Collider player)
    {
        var playerbody = player.GetComponent<PlayerMovement>();

        if(playerbody != null)
        {
            print("player is hit");
        }
    }
    public void CheckDead()
    {
        if(Stage1Done == false && Tutorial.Health == 0)
        {
            Stage1Done = true;
            // need to add in a death animation or ragdoll here
        }
        else if(Stage1Done == true && Boss.Health == 0)
        {
            // boss is dead then add in a death animation or ragdoll here
        }
    }
    public void TakeDamage() // TakeDamage deals with the enemies direct health
    {
        if(Stage1Done == false)
        {
            Tutorial.Health = Tutorial.Health - DamageTaken;
        }
        else if(Stage1Done == true)
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
    public void IsStageDone(bool stage)
    {
        this.Stage1Done = stage;
    }
    public bool StageDone()
    {
        return Stage1Done;
    }
    void Update()
    { 
        // TODO: add delay when distance is 5
        // TODO: add look at player
        // TODO: make sure it works with prisoner and gladiator
        // TODO: make sure that when in stage1 then gladiator doesnt move
        if(Stage1Done == false)
        {
            if(navMeshAgent.remainingDistance == 5)
            {
                Debug.Log("distance is 0");
            }
            navMeshAgent.SetDestination(PlayerLocation.transform.position);
            //TODO: if distance greater

            //TODO: add attack [VERY IMPORTANT TO DO THIS]
            Attack();
        }
        else if(Stage1Done == true)
        {
            navMeshAgent.SetDestination(PlayerLocation.transform.position);
            
            //TODO: add attack [VERY IMPORTANT TO DO THIS]
            Attack();
        }
    }
}