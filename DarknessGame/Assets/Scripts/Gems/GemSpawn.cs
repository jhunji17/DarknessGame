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
    private void Start()
    {
        walls = GameObject.FindGameObjectWithTag("Wall").GetComponent<PolygonCollider2D>();
        height = Camera.main.orthographicSize - 2f;
        width = (height * Camera.main.aspect) - 2f;
        
        for (int i = 0; i < initialNumberOfGems; i++)
        {    
            Instantiate(Gem, getSpawnLocation(walls,width,height,maxAttempts), Quaternion.identity);   
        }
    }

    private void spawnNewGem(bool isRed, Vector2 gemPos, int value){
        if(height >= 3f){
            height -= 0.5f;
        }
        if(width >= 3f){
            width -= 0.5f;
        }
        Instantiate(Gem, getSpawnLocation(walls,width,height,maxAttempts), Quaternion.identity);
    }

    private Vector2 getSpawnLocation(Collider2D walls, float width, float height, int maxAttempts){
        int attempts = 0;

        do{
            float x = UnityEngine.Random.Range(-width, width);
            float y = UnityEngine.Random.Range(-height, height);
            Vector2 result = new Vector2(x, y);
            attempts++;

            if (!walls.OverlapPoint(result)) return result;
        }
        while (attempts < maxAttempts);

        return Vector2.zero;
    }
        
}
