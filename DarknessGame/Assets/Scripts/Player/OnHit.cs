using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OnHit : MonoBehaviour
{

    private bool stunned = false;
    private Vector2 oldPos;
    private Vector2 newPos;
    public Rigidbody2D rb;

    public float maxMoveDistance;
    public float immunityTime;

    public static event Action<bool> onLosingGems;

    private void OnEnable()
    {
        PlayerMovement.youHaveBeenHit += LoseGems;
        PlayerMovement.youHaveBeenHit += Stun;
    }

    private void OnDisable()
    {
        PlayerMovement.youHaveBeenHit -= LoseGems;
        PlayerMovement.youHaveBeenHit -= Stun;

    }

    private void Update()
    {
        if (stunned == false)
        {
            oldPos = transform.position;
        }

        newPos = transform.position;
        
        Debug.Log("Old position : " + oldPos);

        if (stunned == true)
        {
            Debug.Log("New position : " + newPos);
            MovingWhileStunned();
        }
        
    }
    private void LoseGems(bool red)
    {
        // implement them spawning around the player in a circle

        onLosingGems(red);

        Debug.Log("LoseGems got called");
    }

    private void Stun(bool red)
    {
        Debug.Log("Stun got called");
        stunned = true;     
    }

    private void MovingWhileStunned()
    {
        if (Vector2.Distance(newPos, oldPos) >= maxMoveDistance)
        {
            rb.velocity = Vector2.zero;
            stunned = false;
            Debug.Log("got here somehow");
        }
    }
}
