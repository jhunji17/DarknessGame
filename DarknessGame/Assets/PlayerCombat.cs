using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    public Animator animator;
    public GameObject attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayer;

    [SerializeField] public KeyCode attackKey;
    void Update()
    {
        if (Input.GetKeyDown(attackKey))
        {
            Attack();
        }
    }

    void Attack()
    {
        animator.SetTrigger("attack");

        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange, playerLayer);

        foreach(Collider2D enemy in hitPlayer)
        {
            Debug.Log(enemy.name + "GOT HIT");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
    }
}
