using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed;
    public Vector2 moveDir;
    

    [SerializeField] public KeyCode up;
    [SerializeField] public KeyCode down;
    [SerializeField] public KeyCode right;
    [SerializeField] public KeyCode left;
    


    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    private void HandleMovement()
    {
        float moveX = 0f;
        float moveY = 0f;
        if (Input.GetKey(up) == true)
        {
            moveY = +1f;
        }
        if (Input.GetKey(down) == true)
        {
            moveY = -1f;        }

        if (Input.GetKey(right) == true)
        {
            moveX = +1f;
        }

        if (Input.GetKey(left) == true)
        {
            moveX = -1f;
        }
        moveDir = new Vector2(moveX,moveY).normalized;
    }
    private void FixedUpdate() {
        rb.velocity = moveDir;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Collide");
        col.isTrigger = false;
    }
}
