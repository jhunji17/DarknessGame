using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHit : MonoBehaviour
{

    private bool stunned = false;
    private Vector2 oldPos;
    private Vector2 newPos;
    public Rigidbody2D rb;

    public float maxMoveDistance;
    public float immunityTime;

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
        Debug.Log(newPos);
        Debug.Log(oldPos);
        
    }
    private void LoseGems()
    {
        // implement losing gems in score as well as them spawning around the player in a circle
    }

    private void Stun()
    {
        stunned = true;
        if( DistanceCalculator(oldPos, newPos) >= maxMoveDistance)
        {
            rb.velocity = Vector2.zero;
            stunned = false;
        }
    }

    private float DistanceCalculator(Vector2 startPos, Vector2 endPos)
    {
        float distance = Vector2.Distance(endPos, startPos);
        Debug.Log(distance);
        return distance;
    }
}
