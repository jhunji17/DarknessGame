using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Timer : MonoBehaviour
{
    public Text text;
    // replace this with the actual thing
    private float currentTime = 0f;
    private float startingTime = 180f;
    public delegate void GameOver();
    public static event GameOver GameOverEvent;

    private void Start()
    {
        text = GetComponent<Text>();
        currentTime = startingTime;
    }

    private void Update()
    {
        // this can be removed, just done for testing.
        if (Input.GetMouseButtonDown(1))
        {
            currentTime = 10;
        }

        if (currentTime > 0)
        {
            currentTime -= 1 * Time.deltaTime;
            text.text = "Time: " + currentTime.ToString("0");
        }

        else
        {
            // game over event occurs here - I think we need to do a GameManager for this - idk
            GameOverEvent();
        }       
    }
}
