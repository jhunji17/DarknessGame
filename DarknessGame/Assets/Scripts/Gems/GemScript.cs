using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GemScript : MonoBehaviour
{



    private int value ;
    private Transform pos;
    private SpriteRenderer sprite;


    public static event Action<bool, Vector2, int> onGemDug;

     public void Initialize(int x){
        value = x;
    }

    void Start()
    {       
        pos = GetComponent<Transform>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        PlayerMovement.OnSuccessfulDig += iHaveBeenDug;
    }

    private void OnDisable()
    {
        PlayerMovement.OnSuccessfulDig -= iHaveBeenDug;
    }

    public void iHaveBeenDug(bool redPlayer, Vector2 playerPos)
    {
        if (Vector2.Distance(pos.position, playerPos) <= 1)
        {
            Destroy(sprite);
            if(onGemDug != null)
            {
                onGemDug(redPlayer,pos.position, value);
                
            }
            Destroy(this);
        }
    }
}
