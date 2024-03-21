using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
// using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private PlayerMovement DamagePlayersHealth;
    [SerializeField] public bool AggroEnemy;
    private float Rage = 10f;
    private float TimeSinceLastATTK = 0f;
    private bool Blocking = false;
    private bool ComboDone = true;
    private bool DamageDone = false;
    private bool Attacking;
    private float LastATK = 0f;
    private Prisoner Tutorial;
    private Gladiator Boss;
    private bool spawnDead = false;
    [SerializeField] private bool Stage1Done;
    [SerializeField] private float TetherDistance = 1.0f; // set it to nav mesh
    [SerializeField] private float speed = 5f; // set this to nav mesh
    // private GameObject Player;

    // following a vid here
    [SerializeField] private NavMeshAgent navMeshAgent;
    public GameObject PlayerLocation;
    // public Transform playerTransform;
    public Animator animateEnemy; // idk following vid here
    BoxCollider EnemyAttack;
    // TODO: implement wakeup

    public Transform target;

    private bool cutsceneControlled = false;
    private Transform currentAnchor;
    private bool lookAtPlayer = false;
    void Start()
    { // should work the second its created
        // Stage1Done = false;
        Tutorial = new Prisoner();
        Boss = new Gladiator();
        Attacking = false;
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
                Debug.Log("Prisoner Attack is made");
                Attacking = true;
                animateEnemy.SetTrigger("Attack");
                // GetComponentInChildren<BoxCollider>().isTrigger = true;
                DamageDone = false;
            }
            // else
            // {
            //     //FOR TESTING PURPOSES
            //     // Debug.Log("block is done");
            //     GetComponent<BoxCollider>().isTrigger = false;
            //     Block();
            // }
            GetComponentInChildren<BoxCollider>().isTrigger = false;
            TimeSinceLastATTK = 0;
        }
        else if(Stage1Done == true && TimeSinceLastATTK > this.Boss.AttackSpd)// then we are in boss fight
        {
            if(AttackRoll <= 6f && ComboDone == true) // then attack goes through
            {
                float WhatAttackRoll = Random.Range(0f,10f);
                Debug.Log("Attack is made by gladiator");
                Attacking = true;
                // GetComponentInChildren<Collider>().isTrigger = true;
                if(WhatAttackRoll >= 0f && WhatAttackRoll < 2f)
                {
                    DamageDone = false;
                    animateEnemy.SetTrigger("Combo1");
                    ComboDone = false;
                }
                else if (WhatAttackRoll >= 2f && WhatAttackRoll < 6f)
                {
                    DamageDone = false;
                    animateEnemy.SetTrigger("Attack");
                }
                else if (WhatAttackRoll >= 6f && WhatAttackRoll < 8f)
                {
                    DamageDone = false;
                    animateEnemy.SetTrigger("MediumAttk");
                }
                else if (WhatAttackRoll >= 8f && WhatAttackRoll < 10f)
                {
                    DamageDone = false;
                    animateEnemy.SetTrigger("HeavyAttk");
                }

                if(DamageDone == false)
                {
                    Boss.RageMeter += 1f;
                    // Attacking = false;
                }
                //TODO: get information from soma and then finish up attack here
            }
            else if(ComboDone == true) // then block
            {
                GetComponentInChildren<Collider>().isTrigger = false;
                Debug.Log("Blocking");
                Block();
            }
            TimeSinceLastATTK = 0;
            DamageDone = false;
        }
        TimeSinceLastATTK += Time.deltaTime;
    }
    public void initiateEnemy()
    {
        AggroEnemy = !AggroEnemy;
    }
    public bool checkStage()
    {
        return Stage1Done;
    }
    public void CheckDead()
    {
        if(Stage1Done == false && Tutorial.Health == 0)
        {
            Stage1Done = true;
            // need to add in a death animation or ragdoll here
            this.animateEnemy.SetTrigger("Death");
            spawnDead = true;
            
        }
        else if(Stage1Done == true && Boss.Health == 0)
        {
            // boss is dead then add in a death animation or ragdoll here
            this.animateEnemy.SetTrigger("Death");
            spawnDead = true;
        }
    }
    public void EnemyTakeDamage(float DamageToTake) // TakeDamage deals with the enemies direct health
    {
        if(this.Blocking == true)
        {
            return;
        }
        if(Tutorial.Health <= 0f || Boss.Health <= 0f)
        {
            CheckDead();
            return;
        }
        if(Stage1Done == false)
        {
            if(Tutorial.Health <= 0f)
            {
                Debug.Log("Prisoner Health already 0");
                CheckDead();
                return;
            }
            Tutorial.Health = Tutorial.Health - DamageToTake;
            animateEnemy.SetTrigger("TakeDamage");
        }
        else if(Stage1Done == true)
        {
            if(Boss.Health <= 0f)
            {
                Debug.Log("Gladiator Health already 0");
                CheckDead();
                return;
            }
            Boss.Health = Boss.Health - DamageToTake; // decrease health
            Boss.RageMeter = Boss.RageMeter + Rage; // increase ragemeter
            if(Boss.RageMeter == 100f)
            {
                Boss.AttackDmg = Boss.AttackDmg + Boss.AttackDmg;
                Boss.AttackSpd = Boss.AttackSpd - 0.5f;
            }
            else if(Boss.RageMeter == 50f)
            {
                Boss.AttackDmg = Boss.AttackDmg + 5f;
                Boss.AttackSpd = Boss.AttackSpd - 0.5f;
            }
            animateEnemy.SetTrigger("TakeDamage");
        }
        Debug.Log("Prisoner health: " + Tutorial.Health);
        Debug.Log("Gladiator health: " + Boss.Health);
        Debug.Log("Gladiator Rage Meter: "+ Boss.RageMeter);
    }
    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.gameObject.CompareTag("Player"));
        
        if(this.Blocking == false && spawnDead == false)
        {
            // Debug.Log(other.gameObject);
            if(other.gameObject.CompareTag("Player")&& gameObject.CompareTag("Prisoner"))
            {
                if(this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("Attack") && DamageDone == false)
                {
                    // Debug.Log("Prisoner attack lands");
                    DamagePlayersHealth.TakeDamage(Tutorial.AttackDmg); // Base Damage for Now
                    DamageDone = true;
                }
                // Attacking = false;
            }
            else if (other.gameObject.CompareTag("Player")&& gameObject.CompareTag("Gladiator"))
            {
                if((this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("atack2") ||  
                this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("Attack")) && DamageDone == false)
                {
                    // Debug.Log("Gladiator light attack lands");
                    DamagePlayersHealth.TakeDamage(Boss.AttackDmg); // Base Damage for Now
                    DamageDone = true;
                }
                else if((this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("atack1")  || 
                this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("Medium")) && DamageDone == false)
                {
                    // Debug.Log("Gladiator medium attack lands");
                    DamagePlayersHealth.TakeDamage(Boss.AttackDmg + 5f); // Base Damage for Now
                    DamageDone = true;
                }
                else if((this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("AttackEnd") ||
                 this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("Heavy")) && DamageDone == false)
                {
                    // Debug.Log("Gladiator heavy attack lands");
                    DamagePlayersHealth.TakeDamage(Boss.AttackDmg * 2f); // Base Damage for Now
                    DamageDone = true;
                }
                else if(this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("atack shield") && DamageDone == false)
                {
                    // Debug.Log("Gladiator shield bash lands");
                    DamagePlayersHealth.TakeDamage(0f); // Base Damage for Now
                    DamageDone = true;
                }
                if (this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("AttackEnd") && ComboDone == false)
                {
                    ComboDone = true;
                    Attacking = false;
                }
            }
        }
    }
    private void AttackAnimDone()
    {
        if(gameObject.CompareTag("Prisoner") && this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            Debug.Log("Correct name");
            Debug.Log("time is: " + this.animateEnemy.GetCurrentAnimatorStateInfo(0).normalizedTime);
            if(this.animateEnemy.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9)
            {
                Debug.Log("Anim is done");
                Attacking = false;
            }
        }
        else if (gameObject.CompareTag("Gladiator") && (this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("Attack") || 
                        this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("Heavy")||
                        this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("Medium") || 
                        this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("AttackEnd")))
        {
            if(this.animateEnemy.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                Attacking = false;
                ComboDone = true;
            }
        }
        Debug.Log("attacking: " + Attacking);
        Debug.Log("ComboDone: " + ComboDone);
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
    public void cutsceneMovement(Transform anchor, bool playerLook)
    {
        cutsceneControlled = true;
        currentAnchor = anchor;
        lookAtPlayer = playerLook;
    }
    public bool cutsceneMovement()
    {
        return this.cutsceneControlled;
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
        
        if(AggroEnemy == true && spawnDead == false)
        {
            // Debug.Log("ATTACKING: " + Attacking);
            CheckDead();
            float step = speed * Time.deltaTime; 
            // If the enemy is close to the player
            target.position = new Vector3(target.position.x,0f,target.position.z);
            this.transform.LookAt(target);
            if ((Vector3.Distance(transform.position, target.position) < TetherDistance || LastATK >= 1.0f) && Attacking == false)
            {
                // The enemy is stationaty
                step = 0;
                Attack();
                LastATK = 0;
                animateEnemy.SetFloat("Speed",step);
            }
            else if(Attacking == true)
            {
                AttackAnimDone();
                // LastATK += Time.deltaTime;
            }
            if(Attacking == false)
            {
                // The enemy moves toward the player
                transform.position = Vector3.MoveTowards(transform.position, target.position, step);
                LastATK += Time.deltaTime;
                animateEnemy.SetFloat("Speed",step);
            }
        }
        else if(spawnDead == false)
        {
            animateEnemy.SetFloat("Speed",0f);
        }

        if (this.cutsceneControlled)
        {
            float step = speed * Time.deltaTime;
            if (Vector3.Distance(transform.position, this.currentAnchor.position) < 3f)
            {
                cutsceneControlled = false;
                navMeshAgent.isStopped = true; // Stop the agent's movement
                navMeshAgent.enabled = false;
                animateEnemy.SetFloat("Speed",0f);
            }
            else
            {
                navMeshAgent.enabled = true;
                navMeshAgent.isStopped = false; // Enable the agent's movement
                navMeshAgent.SetDestination(this.currentAnchor.position);
                animateEnemy.SetFloat("Speed", step);
            }
        }
    }
}