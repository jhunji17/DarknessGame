using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OnHit : MonoBehaviour
{

    public bool stunned = false;
    public Rigidbody2D rb;

    public float stunTime;
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
        Debug.Log(stunned);
        if (stunned == true)
        {
            StartCoroutine(StunTime());
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
        Debug.Log("This has been called ");
        stunned = true;     
    }

    IEnumerator StunTime()
    {
        Debug.Log("Got here1");
        yield return new WaitForSeconds(stunTime);
        stunned = false;
        rb.velocity = Vector2.zero;
        Debug.Log(rb.velocity);
    }
}
