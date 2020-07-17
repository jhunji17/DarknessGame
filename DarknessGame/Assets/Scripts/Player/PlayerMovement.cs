﻿using System.Collections;
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
    [SerializeField] public KeyCode attackKey;

    public GameObject attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayer;

    private enum actionState {idle, running, digging, attacking};
    private enum lightState  {lit, dark};
    public enum actionState {idle, running, digging};
    public enum lightState  {lit, dark};

    private actionState aState = actionState.idle;
    private lightState lState =  lightState.lit;
    public actionState astate = actionState.idle;
    public lightState lstate =  lightState.lit;
    
    public static event Action<bool, Vector2> OnSuccessfulDig;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        height = Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
    }

    private void FixedUpdate()
    {      
        handleMovement();       
    }


    private void Update(){
        checkpassdig();
        HandleAttackBoxMovement();
        handleOther();
        handleLighting();
        HandleAnimation();
    }

    private void handleMovement()
    {
        bool called = false;

        if (Input.GetKey(up))
        {
            aState = actionState.running;
            //fix this it shoudl be a straight addition  
            rb.position += (Vector2)transform.up * Time.deltaTime * moveSpeed;
            called = true;
        }
        if (Input.GetKey(down))
        {
            aState = actionState.running;
            rb.position -= (Vector2)transform.up * Time.deltaTime * moveSpeed;
            called = true;
        }

        if (Input.GetKey(right))
        {
            aState = actionState.running;
            rb.position += (Vector2)transform.right * Time.deltaTime * moveSpeed;
            called = true;
        }

        if (Input.GetKey(left))
        {
            aState = actionState.running;
            rb.position -= (Vector2)transform.right * Time.deltaTime * moveSpeed;
            called = true;
        }

        if (called == false)
        {
            aState = actionState.idle;
        }
    }

    private void handleLighting(){
        if(lState == lightState.dark){
            light.intensity = 0;
        } else {
            light.intensity = 2;
        }
    }

    private void handleOther(){

        if (Input.GetKeyDown(lightOn))
        {
            if (lState == lightState.lit)
            {
                lState = lightState.dark;
            }
            else
            {
                lState = lightState.lit;
            }
        }

        if (Input.GetKey(dig) == true)
        {
            aState = actionState.digging;
            PassDig = true;
            StartCoroutine(CheckCompletedDig(rb.position));
        }

        if (Input.GetKeyDown(attackKey))
        {
            aState = actionState.attacking;
            Attack();
            if (Input.GetKeyUp(attackKey))
            {
                aState = actionState.idle;
            }
        }
    }

    void Attack()
    {    
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange, playerLayer);

        foreach (Collider2D enemy in hitPlayer)
        {
            Debug.Log(enemy.name + "GOT HIT");
        }
    }

    public void checkpassdig(){
        if(aState != actionState.digging || lState != lightState.lit){
        
        if(astate != actionState.digging || lstate != lightState.lit){
            PassDig = false;
        }
    }

    IEnumerator CheckCompletedDig(Vector2 startpos){
        yield return new WaitForSeconds(digTime);
        aState = actionState.idle;
        if(PassDig){
            if(OnSuccessfulDig != null){
                OnSuccessfulDig(isRed,rb.position);
            }
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

    public void HandleAnimation()
    {
        if (aState == actionState.running)
        {
            animator.SetBool("isMoving", true);
        }

        else if (aState == actionState.digging)
        {
            // need some sort of diggin animation here
            // animator.SetTrigger("digging");
        }

        else if (aState == actionState.attacking)
        {
            animator.SetTrigger("attack");
        }

        else
        {
            //should automatically return to idle
            animator.SetBool("isMoving", false);
        }      
    }

    private void HandleAttackBoxMovement()
    {
        Vector2 newPos = new Vector2();

        if (Input.GetKey(up))
        {
            newPos = new Vector2(0, 1);
            attackPoint.transform.localPosition = newPos;
        }
        if (Input.GetKey(down))
        {
            newPos = new Vector2(0, -1);
            attackPoint.transform.localPosition = newPos;
        }

        if (Input.GetKey(right))
        {
            newPos = new Vector2(1, 0);
            attackPoint.transform.localPosition = newPos;
        }

        if (Input.GetKey(left))
        {
            newPos = new Vector2(-1, 0);
            attackPoint.transform.localPosition = newPos;
        }
    }
}
