﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBlocker : MonoBehaviour
{
   
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Collide");
        col.isTrigger = false;
    }
}