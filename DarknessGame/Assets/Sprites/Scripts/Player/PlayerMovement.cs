using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rigidbody;
    public float moveSpeed;
    [SerializeField] public KeyCode up;
    [SerializeField] public KeyCode down;
    [SerializeField] public KeyCode right;
    [SerializeField] public KeyCode left;
    bool move = true;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        if (move == false)
        {
            return;
        }

        if (Input.GetKey(up) == true)
        {
            transform.position += transform.up * Time.deltaTime * moveSpeed;
        }
        if (Input.GetKey(down) == true)
        {
            transform.position -= transform.up * Time.deltaTime * moveSpeed;
        }

        if (Input.GetKey(right) == true)
        {
            transform.position += transform.right * Time.deltaTime * moveSpeed;
        }

        if (Input.GetKey(left) == true)
        {
            transform.position -= transform.right * Time.deltaTime * moveSpeed;
        }
    }

    void OnTrigger2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            //move = false;
            Debug.Log("Collide");
        }
    }
}
