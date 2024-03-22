using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
// using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private PlayerMovement DamagePlayersHealth; // damages player health
    [SerializeField] public bool AggroEnemy; // when enemy needs to target player after cutscene
    private float Rage = 10f; // rage increase on hit
    private float TimeSinceLastATTK = 0f; // checks time since last attack
    private bool Blocking = false; // if enemy is blocking
    private bool ComboDone = true; //if boss combo is done
    private bool DamageDone = false; // if attack did damage
    private float BlockTime = 0f; //checks how long block has been up
    private bool Attacking; // if enemy is attacking
    private float LastATK = 0f; // how long it has been since last attack for both enemies
    private Prisoner Tutorial; // prisoner stats
    private Gladiator Boss; // boss stats
    private bool spawnDead = false; // is either spawn dead
    [SerializeField] private bool Stage1Done; // if prisoner fight is done
    [SerializeField] private float TetherDistance = 1.0f; // set it to nav mesh
    [SerializeField] private float speed = 5f; // set this to nav mesh
    // private GameObject Player;
    // following a vid here
    [SerializeField] private NavMeshAgent navMeshAgent; // for moving the enemy on the nav mesh
    public GameObject PlayerLocation; // checks player location (useless in the game)
    // public Transform playerTransform;
    public Animator animateEnemy; // idk following vid here
    BoxCollider EnemyAttack; //for checking the enemy attack collider
    // TODO: implement wakeup

    public Transform target; // player body location

    private bool cutsceneControlled = false; // if cutscene is being done
    private Transform currentAnchor; // cutscene move to this location
    private bool lookAtPlayer = false; // if need to look at player
    private bool gameEnd = false;
    private bool phase2 = false;
    private bool phase3 = false;
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
    public bool getGameEnd()
    {
        return gameEnd;
    }
    public float getEnemyCurrentHP() // gets enemy hp
    {
        if(Stage1Done == false)
        {
            return Tutorial.Health;
        }
        else if(Stage1Done == true)
        {
            return Boss.Health;
        }
        return 0f;
    }
    public void resetFight() // restarts the current fight
    {
        if(Stage1Done == false)
        {
            Tutorial.Health = 150f;
            transform.position = currentAnchor.transform.position;
        }
        else if(Stage1Done == true)
        {
            Boss.Health = 1000f;
            Boss.RageMeter = 0f;
            transform.position = GameObject.FindWithTag("BossRespawn").transform.position;
        }
    }
    public void Attack() // attacks that enemy does
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
                int attackLine = Random.Range(1, 5);
                string lineTag = attackLine.ToString() + "b";
                float unusedValue = FindObjectOfType<SoundManager>().PlaySoundEffect(lineTag);
                // Debug.Log("Prisoner Attack is made");
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
        else if(Stage1Done == true && TimeSinceLastATTK > this.Boss.AttackSpd && ComboDone == true)// then we are in boss fight
        {

            if(AttackRoll <= 6f && ComboDone == true) // then attack goes through
            {
                float WhatAttackRoll = Random.Range(0f,10f); // does a random roll for a random attack
                Debug.Log("Attack is made by gladiator");
                Attacking = true;
                int attackLine = Random.Range(1, 6); 
                string lineTag = attackLine.ToString() + "a";
                float unusedValue = FindObjectOfType<SoundManager>().PlaySoundEffect(lineTag);
                // GetComponentInChildren<Collider>().isTrigger = true;
                if(WhatAttackRoll >= 0f && WhatAttackRoll < 2f)
                {
                    DamageDone = false;
                    Debug.Log("combo");
                    animateEnemy.SetTrigger("Combo1");
                    ComboDone = false;
                }
                else if (WhatAttackRoll >= 2f && WhatAttackRoll < 6f)
                {
                    DamageDone = false;
                    // Debug.Log("light attk");
                    animateEnemy.SetTrigger("Attack");
                }
                else if (WhatAttackRoll >= 6f && WhatAttackRoll < 8f)
                {
                    DamageDone = false;
                    // Debug.Log("med attk");
                    animateEnemy.SetTrigger("MediumAttk");
                }
                else if (WhatAttackRoll >= 8f && WhatAttackRoll < 10f)
                {
                    DamageDone = false;
                    // Debug.Log("heavy attk");
                    animateEnemy.SetTrigger("HeavyAttk");
                }

                if(DamageDone == false) // if no attacks did damage
                {
                    Boss.RageMeter += 2f;
                    // Attacking = false;
                }
                //TODO: get information from soma and then finish up attack here
                TimeSinceLastATTK = 0; // reset timer
            }
            else if(ComboDone == true) // then block if no attack done
            {
                GetComponentInChildren<Collider>().isTrigger = false;
                // Debug.Log("Blocking");
                Block();
            }
            // TimeSinceLastATTK = 0;
            DamageDone = false; // reset damage 
        }
        TimeSinceLastATTK += Time.deltaTime; //update timer
    }
    public void initiateEnemy() // initiate enemy aggro to player
    {
        AggroEnemy = !AggroEnemy;
    }
    public bool checkStage() // check if tutorial is done
    {
        return Stage1Done;
    }
    public void CheckDead() // check if prisoner or boss is dead
    {
        if(Stage1Done == false && Tutorial.Health <= 0f)
        {
            Stage1Done = true;
            // need to add in a death animation or ragdoll here
            float unusedValue = FindObjectOfType<SoundManager>().PlaySoundEffect("07");
            Debug.Log(unusedValue);
            this.animateEnemy.SetTrigger("Death");
            spawnDead = true;
            
        }
        else if(Stage1Done == true && Boss.Health <= 0f)
        {
            // boss is dead then add in a death animation or ragdoll here
            this.animateEnemy.SetTrigger("Death");
            spawnDead = true;
            gameEnd = true;
        }
    }
    public void EnemyTakeDamage(float DamageToTake) // TakeDamage deals with the enemies direct health
    {
        Debug.Log(Tutorial.Health);
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
                // Debug.Log("Prisoner Health already 0");
                CheckDead();
                return;
            }
            Tutorial.Health = Tutorial.Health - DamageToTake;
            // animateEnemy.SetTrigger("TakeDamage");
        }
        else if(Stage1Done == true)
        {
            if(Boss.Health <= 0f)
            {
                // Debug.Log("Gladiator Health already 0");
                CheckDead();
                return;
            }
            Boss.Health = Boss.Health - DamageToTake; // decrease health
            Boss.RageMeter = Boss.RageMeter + Rage; // increase ragemeter
            if(Boss.RageMeter >= 100f && !phase3)
            {
                float unusedValue = FindObjectOfType<SoundManager>().PlaySoundEffect("14");
                Boss.AttackDmg = Boss.AttackDmg + Boss.AttackDmg;
                Boss.AttackSpd = Boss.AttackSpd - 0.5f;
                phase3 = true;
            }
            else if(Boss.RageMeter >= 50f && !phase2)
            {
                Boss.AttackDmg = Boss.AttackDmg + 5f;
                Boss.AttackSpd = Boss.AttackSpd - 0.5f;
                phase2 = true;
            }
            // animateEnemy.SetTrigger("TakeDamage");
        }
        // Debug.Log("Prisoner health: " + Tutorial.Health);
        // Debug.Log("Gladiator health: " + Boss.Health);
        // Debug.Log("Gladiator Rage Meter: "+ Boss.RageMeter);
    }
    private void OnTriggerEnter(Collider other) // checks if attack lands on player
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
                else if((this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("atack3") ||
                 this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("Heavy")) && DamageDone == false)
                {
                    // Debug.Log("Gladiator heavy attack lands");
                    DamagePlayersHealth.TakeDamage(Boss.AttackDmg * 2f); // Base Damage for Now
                    DamagePlayersHealth.UnBlock();
                    DamageDone = true;
                }
                else if(this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("atack shield") && DamageDone == false)
                {
                    // Debug.Log("Gladiator shield bash lands");
                    DamagePlayersHealth.TakeDamage(0f); // Base Damage for Now
                    DamagePlayersHealth.UnBlock();
                    DamageDone = true;
                }
                if (this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("atack3") && ComboDone == false)
                {
                    ComboDone = true;
                    Attacking = false;
                }
            }
        }
    }
    private void AttackAnimDone() // check if attack animation is done to start another attack
    {
        if(gameObject.CompareTag("Prisoner") && this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            // Debug.Log("Correct name");
            // Debug.Log("time is: " + this.animateEnemy.GetCurrentAnimatorStateInfo(0).normalizedTime);
            if(this.animateEnemy.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9)
            {
                // Debug.Log("Anim is done");
                Attacking = false;
            }
        }
        else if (gameObject.CompareTag("Gladiator") && (this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("Attack") || 
                        this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("Heavy")||
                        this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("Medium") || 
                        this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("atack3")))
        {
            if(this.animateEnemy.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                Attacking = false;
                ComboDone = true;
            }
            Debug.Log("attacking: " + Attacking);
            Debug.Log("ComboDone: " + ComboDone);
        }
        else if(gameObject.CompareTag("Gladiator") && (this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("atack2") || 
                        this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("atack shield")||
                        this.animateEnemy.GetCurrentAnimatorStateInfo(0).IsName("atack1") ))
        {
            DamageDone = false;
        }
    }
    void Block() // set block
    {
        this.Blocking = true;
        BlockTime = 0f;
        animateEnemy.SetTrigger("Block");
    }
    public void IsStageDone(bool stage) // set it from enemy controller
    {
        this.Stage1Done = stage;
    }
    public bool StageDone() // return it to enemy controller
    {
        return Stage1Done;
    }
    public void cutsceneMovement(Transform anchor, bool playerLook) // the cutscenes movement for enemies
    {
        cutsceneControlled = true;
        currentAnchor = anchor;
        lookAtPlayer = playerLook;
    }
    public bool cutsceneMovement() // return cutscene movement
    {
        return this.cutsceneControlled;
    }
    public void turnEnemy() // moves enemy to look at player
    {
        this.transform.LookAt(target);
    }
    void Update() // updates everything
    { 
        // TODO: add delay when distance is 5
        // TODO: add look at player
        // TODO: make sure it works with prisoner and gladiator
        // TODO: make sure that when in stage1 then gladiator doesnt move
    /*
        //old code for nav mesh doesnt work rn
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
        
        if(AggroEnemy == true && spawnDead == false) // if fight starts and enemy not dead
        {
            // Debug.Log("ATTACKING: " + Attacking);
            if(BlockTime > 0.5f) // checks if block is on for to long (block loop fix)
            {
                this.Blocking = false;
                BlockTime = 0f;
            }
            CheckDead(); // check if enemy dead
            float step = speed * Time.deltaTime; 
            // If the enemy is close to the player
            if(Stage1Done == false) // locations for looking at player
            {
                target.position = new Vector3(target.position.x,0f,target.position.z);
            }
            else
            {
                target.position = new Vector3(target.position.x,10f,target.position.z);
            }
            this.transform.LookAt(target);
            if ((Vector3.Distance(transform.position, target.position) < TetherDistance || LastATK >= 1.0f) && Attacking == false)
            { // basic attack with a checker afterwards
                // The enemy is stationaty
                step = 0;
                Attack();
                LastATK = 0;
                animateEnemy.SetFloat("Speed",step);
            }
            else if(Attacking == true) // if attack anim not done check here
            {
                AttackAnimDone();
                // LastATK += Time.deltaTime;
            }
            if(Attacking == false) // if done now move
            {
                // The enemy moves toward the player
                transform.position = Vector3.MoveTowards(transform.position, target.position, step);
                LastATK += Time.deltaTime;
                animateEnemy.SetFloat("Speed",step);
            }
            if(this.Blocking == true) //update blocking time here
            {
                BlockTime += Time.deltaTime;
            }
        }
        else if(spawnDead == false) // if dead no move
        {
            animateEnemy.SetFloat("Speed",0f);
        }

        if (this.cutsceneControlled) //if in cutscene do this
        {
            float step = speed * Time.deltaTime;
            if (Vector3.Distance(transform.position, this.currentAnchor.position) < 3f) //move to this anchor
            {
                cutsceneControlled = false;
                navMeshAgent.isStopped = true; // Stop the agent's movement
                if (StageDone()) // if prisoner fight done do this
                {
                    navMeshAgent.enabled = false;
                }
                target.position = new Vector3(target.position.x,0f,target.position.z);
                this.transform.LookAt(target);
                animateEnemy.SetFloat("Speed",0f);
            }
            else // if at anchor do this
            {
                navMeshAgent.enabled = true;
                navMeshAgent.isStopped = false; // Enable the agent's movement
                navMeshAgent.SetDestination(this.currentAnchor.position);
                animateEnemy.SetFloat("Speed", step);
            }
        }
    }
}