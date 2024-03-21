using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
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
    [SerializeField] private bool[] scenes = {false, false, false};
    private MouseLook mouseLook;
    private EnemyAi guardAi;
    private EnemyAi prisonerAi;
    private EnemyAi gladiatorAi;
    private PlayerMovement playerScript;
    private SlideGate cell;
    private SlideGate stair;
    private SlideGate coliseum;
    [SerializeField] private float stateTime;
    [SerializeField] private float stateTimeEnd;
    [SerializeField] private GameObject enemyGroup;
    private bool cutscene = true;
    private SoundManager soundManager;
    private EnemyController enemyController;
    [SerializeField] private Bar enemyHealth;
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject playerHealth;
    [SerializeField] private GameObject playerStamina;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private GameObject subtitle;
    [SerializeField] private CanvasGroup gameOverScreen;
    private bool HUDactive = true;
    private Subtitles subtitleScript;
    [SerializeField] private bool hideGameOver = true;
    private float timedDelay = 0f;
    // private Subtitles subtitles;
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
        subtitleScript = subtitle.GetComponent<Subtitles>();
        coliseum = arenaDoor.GetComponent<SlideGate>();
        enemyController = enemyGroup.GetComponent<EnemyController>();

        soundManager = FindObjectOfType<SoundManager>();
        stateTimeEnd = soundManager.PlaySoundEffect("water");
        stateTimeEnd = 2f;
        stateTime = 0f;
        toggleControls();
        HideHud();
        enemyHealth.SetMaxHealth(prisonerAi.getEnemyCurrentHP());
    }
    void HideHud()
    {
        HUDactive = !HUDactive;
        sword.gameObject.SetActive(HUDactive);
        playerHealth.gameObject.SetActive(HUDactive);
        playerStamina.gameObject.SetActive(HUDactive);
        enemyHealth.gameObject.SetActive(HUDactive);
        title.gameObject.SetActive(HUDactive);
    }
    void ShowSubtitles()
    {
        subtitleScript.UpdateText(20);
    }
    void gameOver()
    {
        hideGameOver = false;
        gameOverScreen.blocksRaycasts = true;
        mouseLook.unlockMouse();
        toggleControls();
        gameOverScreen.alpha = 0.8f;
        if (title.text == "Gladiator")
        {
            float unusedValue = FindObjectOfType<SoundManager>().PlaySoundEffect("15");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (hideGameOver)
        {
            if (gameOverScreen.alpha >= 0)
            {
                gameOverScreen.alpha -= Time.deltaTime*2;
            }
        }
        else
        {
            if (gameOverScreen.alpha < 0.5)
            {
                gameOverScreen.alpha += Time.deltaTime*2;
            }
        }
        if (playerScript.getGameOver())
        {
            toggleControls();
            gameOver();
        }
        if (enemyController.TutorialDone && scenes[1] == false && timedDelay >= 3f)
        {
            toggleControls();
            HideHud();
            scenes[1] = true;
            cutscene = true;
            stateTransition = true;
            stateTime = 0f;
            timedDelay = 0f;
            sceneState = 0;
            playerScript.resetHP();
            gladiator = GameObject.FindGameObjectsWithTag("Gladiator")[0];
        } 
        else if (enemyController.TutorialDone && scenes[1] == false)
        {
            timedDelay += Time.deltaTime;
        }
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
            if(scenes[1] && stateTransition)
            {
                Scene1Coliseum();
            }
        }
    }
    void LateUpdate()
    {
        if (title.text == "Gladiator")
        {
            if (gladiator != null)
            {
                gladiatorAi = gladiator.GetComponent<EnemyAi>();
                enemyHealth.SetMaxHealth(gladiatorAi.getEnemyCurrentHP());
            }
            enemyHealth.SetHealth(gladiatorAi.getEnemyCurrentHP());
        }
        else
        {
            enemyHealth.SetHealth(prisonerAi.getEnemyCurrentHP());
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
            mouseLook.isLockedOnTarget = true;
        }
        // States
        if (sceneState == 1)
        {
            mouseLook.SetTargetLocking(1);
            stateTimeEnd = soundManager.PlaySoundEffect("00");
            subtitleScript.UpdateText(0);
        }
        if (sceneState == 2)
        {
            mouseLook.SetTargetLocking(1);
            stateTimeEnd = soundManager.PlaySoundEffect("01");
            subtitleScript.UpdateText(2);
        }
        if (sceneState == 3)
        {
            mouseLook.SetTargetLocking(3);
            guardAi.cutsceneMovement(anchors[0], true);
            guardAi.turnEnemy();
            stateTimeEnd = soundManager.PlaySoundEffect("03");
            subtitleScript.UpdateText(3);
        }
        if (sceneState == 4)
        {
            mouseLook.SetTargetLocking(3);
            stateTimeEnd = soundManager.PlaySoundEffect("04");
            subtitleScript.UpdateText(4);
            guardAi.turnEnemy();
        }
        if (sceneState == 5)
        {
            mouseLook.SetTargetLocking(1);
            prisonerAi.turnEnemy();
            stateTimeEnd = soundManager.PlaySoundEffect("02");
            subtitleScript.UpdateText(8);
        }
        if (sceneState == 6)
        {
            mouseLook.SetTargetLocking(3);
            stateTimeEnd = soundManager.PlaySoundEffect("06");
            subtitleScript.UpdateText(10);
        }
        if (sceneState == 7)
        {
            ShowSubtitles();
            stateTimeEnd = soundManager.PlaySoundEffect("DoorSlide") - 3f;
            mouseLook.SetTargetLocking(3);
            cell.SetOpening();
        }
        if (sceneState == 8)
        {
            soundManager.PauseSoundEffect("water");
            stateTimeEnd = 4f;
            mouseLook.SetTargetLocking(4);
            guardAi.cutsceneMovement(anchors[3], true);
            prisonerAi.cutsceneMovement(anchors[2], true);
            playerScript.CutsceneMovement(anchors[1]);
            guardAi.turnEnemy();
        }
        if (sceneState == 9)
        {
            stateTimeEnd = soundManager.PlaySoundEffect("DoorSlide") - 3f;
            cell.SetClosing();
            guardAi.turnEnemy();
        }
        if (sceneState == 10)
        {
            stateTimeEnd = 2f;
            mouseLook.SetTargetLocking(1);
            scenes[0] = false;
            toggleControls();
            HideHud();
            prisonerAi.initiateEnemy();
            cutscene = false;
        }
            
    }
    void Scene1Coliseum()
    {
        // changeState
        if (stateTransition == true)
        {
            sceneState += 1;
            stateTransition = false;
            mouseLook.isLockedOnTarget = true;
        }
        // States
        if (sceneState == 1)
        {
            mouseLook.SetTargetLocking(3);
            stateTimeEnd = soundManager.PlaySoundEffect("08") + 2;
            subtitleScript.UpdateText(11);
        }
        if (sceneState == 2)
        {
            mouseLook.SetTargetLocking(3);
            stateTimeEnd = soundManager.PlaySoundEffect("09") + 4;
            guardAi.cutsceneMovement(anchors[7], true);
            subtitleScript.UpdateText(12);
        }
        if (sceneState == 3)
        {
            ShowSubtitles();
            stateTimeEnd = soundManager.PlaySoundEffect("DoorSlide") - 3f;
            mouseLook.SetTargetLocking(4);
            stair.SetOpening();
            playerScript.CutsceneMovement(anchors[4]);
        }
        if (sceneState == 4)
        {
            soundManager.StopSoundEffect("water");
            mouseLook.SetTargetLocking(4);
            stair.SetOpening();
            playerScript.CutsceneMovement(anchors[4]);
        }
        if (sceneState == 5)
        {
            stateTimeEnd = 8f;
            mouseLook.SetTargetLocking(5);
            guardAi.cutsceneMovement(anchors[6], false);
            playerScript.CutsceneMovement(anchors[5]);
        }
        if (sceneState == 6)
        {
            title.text = "Gladiator";
            stateTimeEnd = soundManager.PlaySoundEffect("DoorSlide") - 3f;
            mouseLook.SetTargetLocking(2);
            coliseum.SetOpening();
        }
        if (sceneState == 7)
        {
            stateTimeEnd = soundManager.PlaySoundEffect("10");
            mouseLook.SetTargetLocking(3);
            subtitleScript.UpdateText(13); 
        }
        if (sceneState == 8)
        {
            stateTimeEnd = soundManager.PlaySoundEffect("11");
            mouseLook.SetTargetLocking(3);
        }
        if (sceneState == 9)
        {
            stateTimeEnd = soundManager.PlaySoundEffect("12");
            mouseLook.SetTargetLocking(3);
            subtitleScript.UpdateText(14);
        }
        if (sceneState == 10)
        {
            stateTimeEnd = soundManager.PlaySoundEffect("13");
            mouseLook.SetTargetLocking(2);
            subtitleScript.UpdateText(16);
        }
        if (sceneState == 11)
        {
            stateTimeEnd = 2f;
            toggleControls();
            HideHud();
            gladiatorAi.initiateEnemy();
            playerScript.resetHP();
            ShowSubtitles();
            scenes[0] = false;
            cutscene = false;
        }
    }
    public void Respawn()
    {
        playerScript.resetHP();
        playerScript.setGameOver();
        if (title.text == "Gladiator")
        {
            gladiatorAi.resetFight();
        }
        else 
        {
            prisonerAi.resetFight();
        }
        gameOverScreen.alpha = 0;
        hideGameOver = true;
        gameOverScreen.blocksRaycasts = false;
        mouseLook.LockMouse();
        toggleControls();
    }
}
