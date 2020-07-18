using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject pause;
    public GameObject gameOver;

    public bool paused = false;

    enum PageState
    {
        None,
        pause,
        gameOver,

    }
    void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        Timer.GameOverEvent += GameOverMaker;
    }

    private void OnDisable()
    {
        Timer.GameOverEvent -= GameOverMaker;
    }

    private void Start()
    {
        SetPageState(PageState.None);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                pause.SetActive(false);
                gameOver.SetActive(false);
                break;
            case PageState.pause:
                pause.SetActive(true);
                gameOver.SetActive(false);
                break;
            case PageState.gameOver:
                pause.SetActive(false);
                gameOver.SetActive(true);
                break;
        }
    }

    public void Paused()
    {
        paused = true;
        SetPageState(PageState.pause);
        Time.timeScale = 0;
    }

    public void StartAgainFromPause()
    {
        paused = false;
        SetPageState(PageState.None);
        Time.timeScale = 1;
    }

    public void GameOverMaker()
    {
        SetPageState(PageState.gameOver);
        Time.timeScale = 0;
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene(sceneName: "StartPage");
    }
}
