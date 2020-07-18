using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Score : MonoBehaviour
{
    public Text scoreText;
    private int score;
    [SerializeField] private bool redPlayer;


    private void OnEnable()
    {
        GemScript.onGemDug += scored;
    }

    private void OnDisable()
    {
        GemScript.onGemDug -= scored;
    }

    // Start is called before the first frame update
    
    private void Start()
    {
        scoreText = GetComponent<Text>();
    }
    

    // Update is called once per frame
    private void scored(bool isRed, Vector2 gemPos, int value){
        if(redPlayer && isRed){
            score += value;
            scoreText.text = "Redscore: " + score;
        }

        if(!redPlayer && !isRed){
            score += value;
            scoreText.text = "Bluescore: " + score;
        }


    }
}
