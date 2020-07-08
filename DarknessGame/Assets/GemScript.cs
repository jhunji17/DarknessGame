using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScript : MonoBehaviour
{

    private Transform pos;
    private SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        PlayerMovement.OnSuccessfulDig += iHaveBeenDug;
        pos = GetComponent<Transform>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void iHaveBeenDug(bool redPlayer, Vector2 playerPos){
            if(Vector2.Distance(pos.position, playerPos) <= 1){
                Destroy(sprite);
                
                Destroy(this);
            }
    }
}
