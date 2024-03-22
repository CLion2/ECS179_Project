using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] public CanvasGroup ControlScreen;
    private bool hideUI = true;

    public void PlayButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        FindObjectOfType<SoundManager>().PauseSoundEffect("Title");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void ControlButton()
    {
        hideUI = false;
        ControlScreen.blocksRaycasts = true;
    }
    public void BackButton()
    {
        hideUI = true;
        ControlScreen.blocksRaycasts = false;
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            FindObjectOfType<SoundManager>().PlaySoundEffect("Title");
        }
        
    }
    void Update()
    {
        if (hideUI)
        {
            if (ControlScreen.alpha >= 0)
            {
                ControlScreen.alpha -= Time.deltaTime*2;
            }
        }
        else
        {
            if (ControlScreen.alpha < 1)
            {
                ControlScreen.alpha += Time.deltaTime*2;
            }
        }
    }
}
