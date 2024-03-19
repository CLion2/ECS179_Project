using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private PlayerMovement DamagePlayersHealth;
    [SerializeField] public bool AggroEnemy { get; set; }
    private float Rage = 10f;
    private float TimeSinceLastATTK = 0f;
    private bool Blocking = false;
    private bool DamageDone = false;
    private float LastATK = 0f;
    private float Delay = 0.5f; // delay when out of range
    private Prisoner Tutorial;
    private Gladiator Boss;
    [SerializeField] private bool Stage1Done;
    [SerializeField] private float TetherDistance = 1.0f; // set it to nav mesh
    [SerializeField] private float speed = 5f; // set this to nav mesh
    // private GameObject Player;

    // following a vid here
    // public UnityEngine.AI.NavMeshAgent navMeshAgent;
    public GameObject PlayerLocation;
    // public Transform playerTransform;
    public Animator animateEnemy; // idk following vid here
    BoxCollider EnemyAttack;
    // TODO: implement wakeup

    public Transform target;

    void Start()
    { // should work the second its created
        // Stage1Done = false;
        Tutorial = new Prisoner();
        Boss = new Gladiator();
        PlayerLocation = GameObject.Find("Player Body");
        target = GameObject.Find("Player Body").transform;
        EnemyAttack = GetComponentInChildren<BoxCollider>();
        animateEnemy = GetComponentInChildren<Animator>();
        DamagePlayersHealth = GameObject.Find("First Person Player").GetComponent<PlayerMovement>();
    }
    public void Attack()
    // TODO: physics.Raycast for attacking with the swords
    {
        if(this.Blocking == true)
        {
            this.Blocking = false;
        }
        float AttackRoll = Random.Range(0f,10f);
        if(Stage1Done == false && TimeSinceLastATTK > this.Tutorial.AttackSpd) // then we are in tutorial
        {
            if(AttackRoll <= 6f) // then attack goes through
            {
                Debug.Log("Attack is made");
                animateEnemy.SetTrigger("Attack");
                GetComponent<BoxCollider>().isTrigger = true;
                DamageDone = false;
            }
            else
            {
                //FOR TESTING PURPOSES
                // Debug.Log("block is done");
                GetComponent<BoxCollider>().isTrigger = false;
                Block();
            }
            GetComponent<BoxCollider>().isTrigger = false;
            TimeSinceLastATTK = 0;
        }
        else if(Stage1Done == true && TimeSinceLastATTK > this.Boss.AttackSpd)// then we are in boss fight
        {
            if(AttackRoll <= 6f) // then attack goes through
            {
                Debug.Log("Attack is made by gladiator");
                animateEnemy.SetTrigger("Combo1");
                GetComponent<Collider>().isTrigger = true;
                //TODO: get information from soma and then finish up attack here
                // OnAttackCheck(); // not fully done
            }
            else // then block
            {
                GetComponent<Collider>().isTrigger = false;
                Block();
            }
            GetComponent<Collider>().isTrigger = false;
            TimeSinceLastATTK = 0;
        }
        TimeSinceLastATTK += Time.deltaTime;
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
    public void EnemyTakeDamage(float DamageToTake) // TakeDamage deals with the enemies direct health
    {
        if(Stage1Done == false)
        {
            if(Tutorial.Health <=0)
            {
                Debug.Log("Prisoner Health already 0");
                return;
            }
            Tutorial.Health = Tutorial.Health - DamageToTake;
        }
        else if(Stage1Done == true)
        {
            if(Boss.Health <= 0)
            {
                Debug.Log("Gladiator Health already 0");
                return;
            }
            Boss.Health = Boss.Health - DamageToTake; // decrease health
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
        Debug.Log("Prisoner health:" + Tutorial.Health);
        Debug.Log("Gladiator health: " + Boss.Health);
    }
    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.gameObject.CompareTag("Player"));
        
        if(this.Blocking == false)
        {
            // Debug.Log(other.gameObject);
            if(other.gameObject.CompareTag("Player")&& gameObject.CompareTag("Prisoner"))
            {
                if(this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("Attack") && DamageDone == false)
                {
                    // Debug.Log("Prisoner attack lands");
                    DamagePlayersHealth.TakeDamage(10f); // Base Damage for Now
                    DamageDone = true;
                }
            }
            else if (other.gameObject.CompareTag("Player")&& gameObject.CompareTag("Gladiator"))
            {
                Debug.Log("Gladiator attack lands");
                DamagePlayersHealth.TakeDamage(10f); // Base Damage for Now
            }

        }
    }
    void Block()
    {
        this.Blocking = true;
        animateEnemy.SetTrigger("Block");
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
    /*
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
    */
        if(AggroEnemy == true)
        {
            float step = speed * Time.deltaTime; 
        
            // If the enemy is close to the player
            target.position = new Vector3(target.position.x,0f,target.position.z);
            this.transform.LookAt(target);
            if (Vector3.Distance(transform.position, target.position) < TetherDistance || LastATK >= 1.0f)
            {
                // The enemy is stationaty
                step = 0;
                Attack();
                LastATK = 0;
            }
            else
            {
                // The enemy moves toward the player
                transform.position = Vector3.MoveTowards(transform.position, target.position, step);
                LastATK += Time.deltaTime;
            }
            animateEnemy.SetFloat("Speed",step);
        }
    }
}