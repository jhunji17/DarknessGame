using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Score : MonoBehaviour
{
    public Text scoreText;
    private float score;
    [SerializeField] private bool redPlayer;

    private void Start()
    {
        scoreText = GetComponent<Text>();
        score = 0;
    }



    public void updateMe(float value)
    {
        Debug.Log(value);
        score += value;
        if (redPlayer)
        {
            scoreText.text = "Redscore: " + score;
        }
        else
        {
            scoreText.text = "Bluescore: " + score;
        }
    }


}

