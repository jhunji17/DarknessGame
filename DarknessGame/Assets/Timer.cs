using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Timer : MonoBehaviour
{
    public Text text;
    // replace this with the actual thing
    private bool gameOver = false;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        if (gameOver == false)
        {
            text.text = "Time: " + Time.time.ToString("F1");
        }

        else
        {
            
        }
    }
}
