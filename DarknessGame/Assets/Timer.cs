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

    private void Start()
    {
        text = GetComponent<Text>();
        currentTime = startingTime;
    }

    private void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        text.text = "Time: " + currentTime.ToString ("0");
        
    }
}
