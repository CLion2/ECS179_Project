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
    [SerializeField] private int sceneState = 0;
    [SerializeField] private bool stateTransition = false;
    [SerializeField] private bool[] scenes = {false, false, false, false, false};
    private MouseLook mouseLook;
    private EnemyAi guardAi;
    private EnemyAi prisonerAi;
    private PlayerMovement playerScript;
    private SlideGate cell;
    private SlideGate stair;
    private SlideGate coliseum;
    [SerializeField] private float stateTime;
    [SerializeField] private float stateTimeEnd;
    private bool cutscene = true;
    private SoundManager soundManager;
    void Start()
    {
        prisoner = GameObject.FindGameObjectsWithTag("Prisoner")[0];
        // gladiator = GameObject.FindGameObjectsWithTag("Gladiator")[0];
        scenes[0] = true;

        mouseLook = cameraControls.GetComponent<MouseLook>();

        guardAi = guard.GetComponent<EnemyAi>();

        prisonerAi = prisoner.GetComponent<EnemyAi>();

        playerScript = player.GetComponent<PlayerMovement>();

        cell = cellDoor.GetComponent<SlideGate>();

        stair = stairDoor.GetComponent<SlideGate>();

        coliseum = arenaDoor.GetComponent<SlideGate>();

        soundManager = FindObjectOfType<SoundManager>();
        stateTimeEnd = soundManager.PlaySoundEffect("water");
        stateTimeEnd = 2f;
        stateTime = 0f;
        toggleControls();
    }

    // Update is called once per frame
    void Update()
    {
        if (cutscene)
        {
            stateTime += Time.deltaTime;
            if (stateTime >= stateTimeEnd)
            {
                stateTime = 0f;
                stateTransition = true;
            }
            if(scenes[0] && stateTransition)
            {
                Scene0prison();
            }
        }
    }
    void toggleControls()
    {
        playerScript.SetCutscene();
        mouseLook.SetCutscene();
    }
    // Uses sceneState to determine which part of the scene we are in then does an action based on the if statement
    // 1: First Line; targets prisoner, 2: Second Line, 3:3rd line, targets guard; guard walks in, 4: 4th line,
    // 5: 5th line; target prisoner, 6: 6th line; target guard, 7: door opens, 8: Player moves to anchor;
    // prisoner moves to anchor; guard moves to anchor, 9: Cell Door closes; HUD and Sword are set to active; Fight Starts.
    void Scene0prison()
    {
        // changeState
        if (stateTransition == true)
        {
            sceneState += 1;
            stateTransition = false;
        }
        // States
        if (sceneState == 1)
        {
            mouseLook.SetTargetLocking(1);
            stateTimeEnd = soundManager.PlaySoundEffect("00");
        }
        if (sceneState == 2)
        {
            mouseLook.SetTargetLocking(1);
            stateTimeEnd = soundManager.PlaySoundEffect("01");
        }
        if (sceneState == 3)
        {
            mouseLook.SetTargetLocking(3);
            guardAi.cutsceneMovement(anchors[0], true);
            stateTimeEnd = soundManager.PlaySoundEffect("03");
        }
        if (sceneState == 4)
        {
            mouseLook.SetTargetLocking(3);
            stateTimeEnd = soundManager.PlaySoundEffect("04");
        }
        if (sceneState == 5)
        {
            mouseLook.SetTargetLocking(1);
            prisonerAi.turnEnemy();
            stateTimeEnd = soundManager.PlaySoundEffect("02");
        }
        if (sceneState == 6)
        {
            mouseLook.SetTargetLocking(3);
            stateTimeEnd = soundManager.PlaySoundEffect("06");
        }
        if (sceneState == 7)
        {
            stateTimeEnd = soundManager.PlaySoundEffect("DoorSlide");
            mouseLook.SetTargetLocking(3);
            cell.SetOpening();
        }
        if (sceneState == 8)
        {
            stateTimeEnd = 10f;
            mouseLook.SetTargetLocking(4);
            guardAi.cutsceneMovement(anchors[3], true);
            prisonerAi.cutsceneMovement(anchors[2], true);
            playerScript.CutsceneMovement(anchors[1]);
        }
        if (sceneState == 9)
        {
            stateTimeEnd = soundManager.PlaySoundEffect("DoorSlide");
            cell.SetClosing();
            stateTimeEnd = 2f;
            mouseLook.SetTargetLocking(1);
            scenes[0] = false;
            toggleControls();
            prisonerAi.initiateEnemy();

        }
            
    }
    void Scene1Coliseum()
    {
        // changeState
        if (stateTransition == true)
        {
            sceneState += 1;
            stateTransition = false;
        }
        // States
        if (sceneState == 1)
        {
            mouseLook.SetTargetLocking(1);
        }
        if (sceneState == 2)
        {
            stateTimeEnd = 5f;
            mouseLook.SetTargetLocking(3);
            guardAi.cutsceneMovement(anchors[0], true);
        }
        if (sceneState == 3)
        {
            toggleControls();
            scenes[1] = false;
        }
    }
    void Respawn()
    {

    }
}
