using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Experimental.Rendering.Universal;


public class GemSpawn : MonoBehaviour
{
    public GameObject Gem;
    public int initialNumberOfGems = 10;
    Vector2 spawnLocation;
    public Collider2D walls;
    private int maxAttempts = 100;
    private float gemConcentration = 0.3f;


    private void OnEnable()
    {
        GemScript.onGemDug += spawnNewGem;
    }

    private void OnDisable()
    {
        GemScript.onGemDug -= spawnNewGem;
    }
    public static float height;
    public static float width;
    public float gemValue;
    private void Start()
    {
        walls = GameObject.FindGameObjectWithTag("Wall").GetComponent<PolygonCollider2D>();
        height = Camera.main.orthographicSize - 2f;
        width = (height * Camera.main.aspect) - 2f;

        for (int i = 0; i < initialNumberOfGems; i++)
        {
            GemScript myGem = UnityEngine.Object.Instantiate(Gem, getSpawnLocation(walls, width, height, maxAttempts), Quaternion.identity).GetComponent<GemScript>();
            myGem.Initialize(getValue());
        }

    }

    private float getValue()
    {   

        float time = GameObject.FindGameObjectWithTag("TimerText").GetComponent<Timer>().currentTime;
        float etime = 180f - time;
        float mean = ((etime/time) * 9f);
        return   (float) (Math.Round(NextGaussian(mean,1,0,10),0));
         
    }

    private void spawnNewGem(bool isRed, Vector2 gemPos, float value)
    {
        if (height >= 3f)
        {
            height -= gemConcentration;
        }
        if (width >= 3f)
        {
            width -= gemConcentration;
        }
        GemScript myGem = UnityEngine.Object.Instantiate(Gem, getSpawnLocation(walls, width, height, maxAttempts), Quaternion.identity).GetComponent<GemScript>();
        myGem.Initialize(getValue());
    }

    private Vector2 getSpawnLocation(Collider2D walls, float width, float height, int maxAttempts)
    {
        int attempts = 0;

        do
        {
            float x = UnityEngine.Random.Range(-width, width);
            float y = UnityEngine.Random.Range(-height, height);
            Vector2 result = new Vector2(x, y);
            attempts++;

            if (!walls.OverlapPoint(result)) return result;
        }
        while (attempts < maxAttempts);

        return Vector2.zero;
    }

    public static float NextGaussian() {
        float v1, v2, s;
        do {
            v1 = 2.0f * UnityEngine.Random.Range(0f,1f) - 1.0f;
            v2 = 2.0f * UnityEngine.Random.Range(0f,1f) - 1.0f;
            s = v1 * v1 + v2 * v2;
        } while (s >= 1.0f || s == 0f);

        s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);
    
        return v1 * s;
    }

    public static float NextGaussian(float mean, float standard_deviation)
    {
        return mean + NextGaussian() * standard_deviation;
    }

    
    public static float NextGaussian (float mean, float standard_deviation, float min, float max) {
    float x;
    do {
        x = NextGaussian(mean, standard_deviation);
    } while (x < min || x > max);
        
        return x;
    }
    

}
