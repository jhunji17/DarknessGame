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
    public LayerMask walls;

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
        height = Camera.main.orthographicSize - 2f;
        width = (height * Camera.main.aspect) - 2f;
        
        for (int i = 0; i < initialNumberOfGems; i++)
        {    
            Instantiate(Gem, getSpawnLocation(), Quaternion.identity);   
        }
    }

    private void spawnNewGem(bool isRed, Vector2 gemPos, int value){
        if(height >= 3f){
            height -= 0.5f;
        }
        if(width >= 3f){
            width -= 0.5f;
        }
        Instantiate(Gem, getSpawnLocation(), Quaternion.identity);
    }

    private Vector2 getSpawnLocation(){
        Debug.Log("here 2");
        float rX = UnityEngine.Random.Range(-width, width);
        float rY = UnityEngine.Random.Range(-height, height);
        Vector2 point = new Vector2(rX,rY);
        while((Physics2D.OverlapCircle(point,1.5f,walls))){
            rX = UnityEngine.Random.Range(-width, width);
            rY = UnityEngine.Random.Range(-height, height);
            point = new Vector2(rX,rY);
            Debug.Log("here 7");
        }
            return point;
    }
}
