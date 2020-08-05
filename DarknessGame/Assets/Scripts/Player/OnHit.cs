// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System;

// public class OnHit : MonoBehaviour
// {

//     public bool stunned = false;
//     public Rigidbody2D rb;

//     public float stunTime;
//     public float immunityTime;


//     private void OnEnable()
//     {
//         PlayerMovement.youHaveBeenHit += Stun;
//     }

//     private void OnDisable()
//     {
//         PlayerMovement.youHaveBeenHit -= Stun;
//     }

//     private void Update()
//     {
//         Debug.Log(stunned);
//         if (stunned == true)
//         {
//             StartCoroutine(StunTime());
//         }       
//     }

//     private void Stun(bool red)
//     {
//         stunned = true;     
//     }

//     IEnumerator StunTime()
//     {
//         yield return new WaitForSeconds(stunTime);
//         stunned = false;
//         rb.velocity = Vector2.zero;
//         Debug.Log(rb.velocity);
//     }
// }
