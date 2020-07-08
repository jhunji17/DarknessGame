using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float height;
    private float width;
    public float digTime;
    public int score;
    public float moveSpeed;
    public bool isRed;
    

    [SerializeField] public KeyCode up;
    [SerializeField] public KeyCode down;
    [SerializeField] public KeyCode right;
    [SerializeField] public KeyCode left;
    [SerializeField] public KeyCode dig;

    private enum State {idle, running, digging}
    private State state = State.idle;

    
    public static event Action<bool, Vector2> OnSuccessfulDig;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        height = Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
    }

    private void FixedUpdate()
    {
        
        handleInputs();
        
        
    }

    private void handleInputs()
    {
            
            if (Input.GetKey(up) == true)
            {
                state = State.running;
                rb.position += (Vector2)transform.up * Time.deltaTime * moveSpeed;
            }
            if (Input.GetKey(down) == true)
            {
                state = State.running;
                rb.position -= (Vector2)transform.up * Time.deltaTime * moveSpeed;
            }

            if (Input.GetKey(right) == true)
            {
                state = State.running;
                rb.position += (Vector2)transform.right * Time.deltaTime * moveSpeed;
            }

            if (Input.GetKey(left) == true)
            {
                state = State.running;
                rb.position -= (Vector2)transform.right * Time.deltaTime * moveSpeed;
            }

            if(Input.GetKey(dig) == true){
                state = State.digging;
                
                StartCoroutine(CheckCompletedDig(rb.position));
                
            }           
    }

    IEnumerator CheckCompletedDig(Vector2 startpos){
        Debug.Log("here1");
        yield return new WaitForSeconds(digTime);
        Debug.Log("here2");
        if(startpos == rb.position){
            Debug.Log("here3");
            if(OnSuccessfulDig != null){
                OnSuccessfulDig(isRed,rb.position);
                Debug.Log("successfuldig");
            }
        }
    }

    
   

    
    
}
