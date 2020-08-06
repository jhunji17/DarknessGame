using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartPageManager : MonoBehaviour
{
    public GameObject NoPageState;
    public GameObject HowToPlay;
    public GameObject OnlineVersion;
    enum PageState
    {
        NoPageState,
        HowToPlay,
        // Online version will be changed to scene when we can actually figure the online shit out
        OnlineVersion,
    }

    private void Start()
    {
        SetPageState(PageState.NoPageState);
    }
    void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.NoPageState:
                NoPageState.SetActive(true);
                HowToPlay.SetActive(false);
                OnlineVersion.SetActive(false);
                break;
            case PageState.HowToPlay:
                NoPageState.SetActive(false);
                HowToPlay.SetActive(true);
                OnlineVersion.SetActive(false);
                break;
            case PageState.OnlineVersion:
                NoPageState.SetActive(false);
                HowToPlay.SetActive(false);
                OnlineVersion.SetActive(true);
                break;
        }
    }

    public void LocalGame()
    {
        SceneManager.LoadScene(sceneName: "Game");
    }

    public void HowToPlayScreen()
    {
        SetPageState(PageState.HowToPlay);
    }

    public void OnlineGame()
    {
        SetPageState(PageState.OnlineVersion);
    }

    public void ReturnToStartPage()
    {
        SetPageState(PageState.NoPageState);
    }
}
