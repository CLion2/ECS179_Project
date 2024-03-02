//this is going to be the controller for spawning the enemies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    private GameObject Enemies;
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private GameObject EnemySpawn;
    void Awake()
    { // should work the second its created
        GameObject Enemy = Instantiate(EnemyPrefab,EnemySpawn.transform.position,Quaternion.identity);
    }
}