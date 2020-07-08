using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScript : MonoBehaviour
{

    private Transform pos;
    private SpriteRenderer sprite;

    public delegate void RedScored();
    public static event RedScored OnRedScored;

    public delegate void BlueScored();
    public static event BlueScored OnBlueScored;


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
            if (redPlayer == true)
            {
                // redplayer scored event
                if(OnRedScored != null)
                {
                    OnRedScored();
                }
            }
            else
            {
                // blueplayer scored event
                if(OnBlueScored != null)
                {
                    OnBlueScored();
                }
            }        
            Destroy(this);
        }
    }
}
