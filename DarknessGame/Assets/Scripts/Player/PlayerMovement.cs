using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed;
    public Vector2 moveDir;

    private bool dug = false;
    private bool destroy = false;
    private float height;
    private float width;

    [SerializeField] public KeyCode up;
    [SerializeField] public KeyCode down;
    [SerializeField] public KeyCode right;
    [SerializeField] public KeyCode left;
    [SerializeField] public KeyCode dig;

    Collider2D collisionDetection;

    //this is a test

    private void OnEnable()
    {
        collisionDetection = gameObject.GetComponent<Collider2D>();
    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        height = Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
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
            moveY = -1f;    
        }

        if (Input.GetKey(right) == true)
        {
            moveX = +1f;
        }

        if (Input.GetKey(left) == true)
        {
            moveX = -1f;
        }

        if (rb.position.x < -width && rb.position.x > width)
        {
            if (rb.position.y < -height && rb.position.y > height)
            {
                transform.position = rb.position;
            }
        }
        moveDir = new Vector2(moveX, moveY).normalized;
        moveDir *= moveSpeed;
    }
    private void FixedUpdate() {
        HandleMovement();
        rb.velocity = moveDir;       
    }
    void OnTriggerStay2D(Collider2D col)
    {

        if (col.gameObject.CompareTag("Gem") == true)
        {
            if (Input.GetKey(dig) == true)
            {
                Destroy(col.gameObject);
            }
            return; 
        }
        col.isTrigger = false;
    }
    void OnTriggerExit2D(Collider2D col)
    {
        dug = false;      
    }
    

}
