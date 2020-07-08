using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class BlueScore : MonoBehaviour
{
    public Text scoreText;
    public static int scoreBlue;

    private void OnEnable()
    {
        GemScript.OnBlueScored += scored;
    }

    private void OnDisable()
    {
        GemScript.OnBlueScored -= scored;
    }

    private void Start()
    {
        scoreText = GetComponent<Text>();
    }

    private void scored()
    {
        scoreBlue++;
        scoreText.text = "Bluescore: " + scoreBlue;
    }
}
