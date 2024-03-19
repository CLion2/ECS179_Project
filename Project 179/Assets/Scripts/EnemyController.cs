//this is going to be the controller for spawning the enemies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    private GameObject Enemies;
    // private bool BossSpawned;
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private GameObject GladiatorPrefab;
    [SerializeField] private GameObject EnemySpawnPrisoner;
    [SerializeField] private GameObject EnemySpawnGladiator;
    [SerializeField] private bool TutorialDone;
    void Awake() // creates the first enemy to be used
    { // should work the second its created
        if(TutorialDone == true) // spawns gladiator if true
        {
            Enemies = Instantiate(GladiatorPrefab,EnemySpawnGladiator.transform.position,Quaternion.identity);
        }
        else
        { // spawns prisoner on anything else
            Enemies = Instantiate(EnemyPrefab,EnemySpawnPrisoner.transform.position,Quaternion.identity);
            // BossSpawned = false;
        }
    }
    void SpawnPrisoner()
    {
        Enemies = Instantiate(EnemyPrefab,EnemySpawnPrisoner.transform.position,Quaternion.identity);
        EnemyAi checking = Enemies.GetComponent<EnemyAi>();
        checking.IsStageDone(false);
    }
    void SpawnBoss()
    {
        Enemies = Instantiate(GladiatorPrefab,EnemySpawnGladiator.transform.position,Quaternion.identity);
        EnemyAi checking = Enemies.GetComponent<EnemyAi>();
        checking.IsStageDone(true);
    }
    // void Build() // creates a new enemy when needed
    // {
    //     if(TutorialDone == true) // spawns gladiator if true
    //     {
    //         Enemies = Instantiate(GladiatorPrefab,EnemySpawnGladiator.transform.position,Quaternion.identity);
    //     }
    //     else
    //     { // spawns prisoner on anything else
    //         Enemies = Instantiate(EnemyPrefab,EnemySpawnPrisoner.transform.position,Quaternion.identity);
    //     } 
    // }

    // checks if the stage itself is done to switch to start spawn on the gladiator
    void CheckTutorialDone(GameObject Enemy) // checks if tutorial enemy is dead
    {
        EnemyAi checking = Enemy.GetComponent<EnemyAi>();
        if(checking.StageDone() == true)
        {
            TutorialDone = true;
            // DestroyEnemy();
        }
        else
        {
            TutorialDone = false;
        }
    }
    // void CheckBossDone(GameObject Enemy) //TODO: FINISH THIS!!!
    // {
    //     EnemyAi checkBoss = Enemy.GetComponent<EnemyAi>();
    //     if(checkBoss. == true)
    //     {
    //         DestroyEnemy();
    //     }
    // }
    //TODO: add a destroy when health becomes zero
    void DestroyEnemy()
    {
        Destroy(Enemies);
    }
    void Update()
    {
        if(TutorialDone == false)// checks if tutorial done
        {
            CheckTutorialDone(Enemies);
        }
        // if(TutorialDone == true && BossSpawned == false) // if tutorial done but boss not spawned
        // {
        //     BossSpawned = true;
        //     Build();
        // }
        // if(BossSpawned == true) // if boss spawned check when done
        // {
        //     CheckBossDone(Enemies);
        // }
    }
}