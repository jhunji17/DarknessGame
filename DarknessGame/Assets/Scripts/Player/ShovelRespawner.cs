using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelRespawner : MonoBehaviour
{
    public LayerMask playerLayer;
    public Vector3 tester;
    public GameObject shovel;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GivePlayerShovel(collision);
    }

    private void GivePlayerShovel(Collider2D playerinBox)
    {
        ShovelAttack playerTEST;
        playerTEST = playerinBox.GetComponentInChildren<ShovelAttack>();

        if (playerTEST != null)
        {
            Debug.Log("HAS A SHOVEL ALREADY");
        }

        else
        {
            //playerTEST.noShovel = false;
            Instantiate(shovel, playerinBox.transform);
        }
    }
}
