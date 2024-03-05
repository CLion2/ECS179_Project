//this is going to be the controller for spawning the enemies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyController : MonoBehaviour
{
    private GameObject Enemies;
    [SerializeField] private GameObject EnemyPrefab;
    // [SerializeField] private 
    [SerializeField] private GameObject EnemySpawnPrisoner;
    [SerializeField] private GameObject EnemySpawnGladiator;
    [SerializeField] private bool TutorialDone = false;
    void Awake()
    { // should work the second its created
        if(TutorialDone == true) // spawns gladiator if true
        {
            Enemies = Instantiate(EnemyPrefab,EnemySpawnGladiator.transform.position,Quaternion.identity);
        }
        else
        { // spawns prisoner on anything else
            Enemies = Instantiate(EnemyPrefab,EnemySpawnPrisoner.transform.position,Quaternion.identity);
        }
    }

    // checks if the stage itself is done to switch to start spawn on the gladiator
    void CheckTutorialDone(GameObject Enemy)
    {
        EnemyAi checking = Enemy.GetComponent<EnemyAi>();
        if(checking.StageDone() == true)
        {
            TutorialDone = true;
        }
        else
        {
            TutorialDone = false;
        }
    }
    //TODO: add a destroy when health becomes zero
    void DestroyEnemy()
    {
        Destroy(Enemies);
    }
    void Update()
    {
        CheckTutorialDone(Enemies);
    }
}