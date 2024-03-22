//this is going to be the controller for spawning the enemies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    private GameObject Enemies; // holds the spawned enemies
    // private bool BossSpawned;
    [SerializeField] private GameObject EnemyPrefab; // prisoner prefab
    [SerializeField] private GameObject GladiatorPrefab; // boss / gladiator prefab
    [SerializeField] private GameObject EnemySpawnPrisoner; // prisoner spawn location (changed in cutscene)
    [SerializeField] private GameObject EnemySpawnGladiator; // gladiator spawn location (changed in cutscene)
    [SerializeField] public bool TutorialDone; // checker for prisoner fight done
    void Awake() // creates the first enemy to be used
    { // should work the second its created
        // GameObject parent = GameObject.Find("Enemies");
        if(TutorialDone == true) // spawns gladiator if true
        {
            Enemies = Instantiate(GladiatorPrefab,EnemySpawnGladiator.transform.position,Quaternion.identity);
        }
        else
        { // spawns prisoner on anything else
            SpawnPrisoner();
        }
    }
    void SpawnPrisoner() // spawns the prisoner
    {
        Enemies = Instantiate(EnemyPrefab,EnemySpawnPrisoner.transform.position,Quaternion.identity);
        EnemyAi checking = Enemies.GetComponent<EnemyAi>();
        checking.IsStageDone(false);
    }
    void SpawnBoss() // spawns the boss
    {
        Enemies = Instantiate(GladiatorPrefab,EnemySpawnGladiator.transform.position,Quaternion.identity);
        EnemyAi checking = Enemies.GetComponent<EnemyAi>();
        checking.IsStageDone(true);
        checking.initiateEnemy();
    }

    // checks if the stage itself is done to switch to start spawn on the gladiator
    void CheckTutorialDone(GameObject Enemy) // checks if tutorial enemy is dead
    {
        EnemyAi checking = Enemy.GetComponent<EnemyAi>();
        if(checking.StageDone() == true)
        {
            TutorialDone = true;
            SpawnBoss();
            // DestroyEnemy();
        }
        else
        {
            TutorialDone = false;
        }
    }

    //TODO: add a destroy when health becomes zero
    void DestroyEnemy() // in case we need it (never used)
    {
        Destroy(Enemies);
    }
    void Update() // update immediatly when prisoner is dead
    {
        if(TutorialDone == false)// checks if tutorial done
        {
            CheckTutorialDone(Enemies); // consistant checks to see if tutorial done
        }
    }
}