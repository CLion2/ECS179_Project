using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject prisoner;
    private GameObject gladiator;
    [SerializeField] private GameObject guard;
    [SerializeField] private GameObject cellDoor;
    [SerializeField] private GameObject stairDoor;
    [SerializeField] private GameObject arenaDoor;
    [SerializeField] private GameObject cameraControls;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform[] anchors;
    private int sceneState = 0;
    private bool sceneTransition = false;
    [SerializeField] private bool[] scenes = {false, false, false, false, false};
    private MouseLook mouseLook;
    private EnemyAi guardAi;
    private PlayerMovement playerScript;
    void Start()
    {
        prisoner = GameObject.FindGameObjectsWithTag("Prisoner")[0];
        // gladiator = GameObject.FindGameObjectsWithTag("Gladiator")[0];
        scenes[0] = true;
        mouseLook = cameraControls.GetComponent<MouseLook>();
        guardAi = guard.GetComponent<EnemyAi>();
        playerScript = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(scenes[0])
        {
            Scene0dark(Time.deltaTime);
        }
    }

    void Scene0dark(float timeNow)
    {
        if (sceneTransition == true)
        {
            sceneState += 1;
        }
        if (sceneState == 1)
        {
            mouseLook.SetTargetLocking(1);
        }
        
        guardAi.cutsceneMovement(anchors[0], true);
        scenes[0] = false;
    }
}
