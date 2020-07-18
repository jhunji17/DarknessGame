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

    private void onEnable(){
        Debug.Log("here2");
        GemScript.onGemDug += spawnNewGem;
        
    }

    
    


    public static float height;
    public static float width;
    
    
    
    

    

    private void Start()
    {
        height = Camera.main.orthographicSize - 2;
        width = (height * Camera.main.aspect) - 2;
        
        for (int i = 0; i < initialNumberOfGems; i++)
        {    
            Instantiate(Gem, getSpawnLocation(), Quaternion.identity);   
        }
    }

    public void spawnNewGem(bool isRed, Vector2 pos, int value){
        Debug.Log("here1");
        if(height >= 3){
            height -= 1;
        }
        if(width >= 3){
            width -= 1;
        }
        Instantiate(Gem, getSpawnLocation(), Quaternion.identity);

    }

    private Vector2 getSpawnLocation(){
        float RandomX = UnityEngine.Random.Range(-width, width);
        float RandomY = UnityEngine.Random.Range(-height, height);
        return new Vector2(RandomX, RandomY);
            
    }

}
