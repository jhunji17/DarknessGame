using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float height;
    private float width;
    public float digTime;
    public int score;
    public float moveSpeed;
    public bool isRed;
    public bool PassDig = false;
    public Light2D light;

    public Animator animator;


    [SerializeField] public KeyCode up;
    [SerializeField] public KeyCode down;
    [SerializeField] public KeyCode right;
    [SerializeField] public KeyCode left;
    [SerializeField] public KeyCode dig;
    [SerializeField] public KeyCode lightOn;

    private enum actionState {idle, running, digging};
    private enum lightState  {lit, dark};

    private actionState astate = actionState.idle;
    private lightState lstate =  lightState.lit;
    
    public static event Action<bool, Vector2> OnSuccessfulDig;

    private Vector2 currentPos;
    private Vector2 prevPos;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        height = Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
    }

    private void FixedUpdate()
    {      
        prevPos = rb.position;
        handleMovement();
        currentPos = rb.position;
        HandleRunningAnimation();
        
    }


    private void Update(){
        checkpassdig();
        handleOther();
        handleLighting();      
    }

    private void handleMovement()
    {
        if (Input.GetKey(up) == true)
        {
            astate = actionState.running;
            //fix this it shoudl be a straight addition
            rb.position += (Vector2)transform.up * Time.deltaTime * moveSpeed;
        }
        if (Input.GetKey(down) == true)
        {
            astate = actionState.running;
            rb.position -= (Vector2)transform.up * Time.deltaTime * moveSpeed;
        }

        if (Input.GetKey(right) == true)
        {
            astate = actionState.running;
            rb.position += (Vector2)transform.right * Time.deltaTime * moveSpeed;
        }

        if (Input.GetKey(left) == true)
        {
            astate = actionState.running;
            rb.position -= (Vector2)transform.right * Time.deltaTime * moveSpeed;
            
        }

    }

    public void HandleRunningAnimation()
    {
        if(prevPos != currentPos)
        {
            animator.SetBool("isMoving", true);
            return;
        }

        else
        {
            animator.SetBool("isMoving", false);
        }
        
    }

    private void handleLighting(){
        if(lstate == lightState.dark){
            light.intensity = 0;
        } else {
            light.intensity = 2;
        }
    }

    private void handleOther(){

            if(Input.GetKeyDown(lightOn)){
                if(lstate == lightState.lit){
                    lstate = lightState.dark;
                } else {
                    lstate = lightState.lit;
                }
            }

            if(Input.GetKey(dig) == true){
                astate = actionState.digging;
                PassDig = true;
                StartCoroutine(CheckCompletedDig(rb.position));
            }
    }

    public void checkpassdig(){
        Debug.Log("here1");
        if(astate != actionState.digging || lstate != lightState.lit){
            PassDig = false;
        }
    }

    IEnumerator CheckCompletedDig(Vector2 startpos){
        yield return new WaitForSeconds(digTime);
        if(PassDig){
            if(OnSuccessfulDig != null){
                OnSuccessfulDig(isRed,rb.position);
            }
        }
    }

    
   

    
    
}
