// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class EnemyController
// {
//     private float DamageTaken = 20;
//     private float Rage = 10;
//     private boolean Stage1;
//     private Prisoner Tutorial;
//     private Gladiator Boss;
//     private boolean Stage1Done;
//     private float TetherDistance = 10f; // for now just to get it working
//     private GameObject Player;
//     [SerializeField] private GameObject PlayerPrefab;


//     void Awake()
//     { // should work the second its created
//         Stage1 = true;
//         Stage1Done = false;
//         Tutorial = Prisoner();
//         Boss = Gladiator();
//         Player = PlayerPrefab; // may not be needed
//     }
//     void Movement()
//     {
//         if(Stage1 == true) // then we are in tutorial
//         {
//             // check player position then use that to create a tether distance
//             // we go based off enemymovement.cs
//             // TODO: make changes for stage 1 here & change it to work
//             // Move to the next location set by SetNextLocation
//             float step = speed * Time.deltaTime; 
//             transform.position = Vector3.MoveTowards(transform.position, nextPosition, step);

//             // If Enemy reaches the next location, call SetNextLocation again
//             // so that Enemy updates the next location
//             if (Vector3.Distance(transform.position, nextPosition) < 0.1f)
//             {
//                 SetNextLocation();
//             }
//         }
//         else if(Stage1 == false)// then we are in boss fight
//         {

//         }
//     }
//     void Attack()
//     {
//         if(Stage1 == true) // then we are in tutorial
//         {

//         }
//         else if(Stage1 == false)// then we are in boss fight
//         {

//         }
//     }
//     void CheckDead()
//     {
//         if(stage1 == true && Tutorial.Health == 0)
//         {
//             Stage1Done = true;
//             Stage1 = false;
//             // need to add in a death animation or ragdoll here
//         }
//         else if(stage1 == false && Boss.Health == 0)
//         {
//             // boss is dead then add in a death animation or ragdoll here
//         }
//     }

//     void TakeDamage() // TAkeDamage deals with the enemies direct health
//     {
//         if(stage1 == true && Stage1Done == false)
//         {
//             Tutorial.Health = Tutorial.Health - DamageTaken;
//         }
//         else if(Stage1Done == true && stage1 == false)
//         {
//             Boss.Health = Boss.Health - DamageTaken; // decrease health
//             Boss.RageMeter = Boss.RageMeter + Rage; // increase ragemeter
//             if(Boss.RageMeter == 100f)
//             {
//                 Boss.AttackDmg = Boss.AttackDmg + 5;
//                 Boss.AttackSpd = Boss.AttackSpd + 0.5f;
//             }
//             else if(Boss.RageMeter < 100f && Boss.RageMeter >= 50f)
//             {
//                 Boss.AttackDmg = Boss.AttackDmg + 5;
//                 Boss.AttackSpd = Boss.AttackSpd + 0.5f;
//             }
//         }
//     }
//     void Block()
//     {

//     }
//     void Update()
//     {
//         if(Stage1 == true)
//         {

//         }
//         else if(Stage1 == false)
//         {

//         }
//     }
// }