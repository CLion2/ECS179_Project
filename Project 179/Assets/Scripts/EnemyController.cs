//this is going to be the controller for spawning the enemies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    private GameObject Enemies;
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private GameObject EnemySpawnPrisoner;
    [SerializeField] private GameObject EnemySpawnGladiator;
    private bool TutorialDone = false;
    void Build()// change this to build
    { // should work the second its created
        GameObject Enemy = Instantiate(EnemyPrefab,EnemySpawnPrisoner.transform.position,Quaternion.identity);

        Destroy(Enemy, 1f);// in case an issue occurs it auto deletes the enemy
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
}