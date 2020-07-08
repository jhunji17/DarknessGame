using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class RedScore : MonoBehaviour
{
    public Text scoreText;
    public static int scoreRed;

    private void OnEnable()
    {
        GemScript.OnRedScored += scored;
    }

    private void OnDisable()
    {
        GemScript.OnRedScored -= scored;

    }

    private void Start()
    {
        scoreText = GetComponent<Text>();
    }

    private void scored()
    {
        scoreRed++;
        scoreText.text = "Redscore: " + scoreRed;
    }
}
