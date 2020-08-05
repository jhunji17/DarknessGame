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
    public float attackForce;
    //public string enemyNameRed = "red";
    private int stunTime;
    //public float immunityTime;


    public Animator animator;

    //private OnHit playerHit;


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

    public enum actionState {idle, running, digging, attacking, stunned};
    public enum lightState  {lit, dark};

    public actionState aState = actionState.idle;
    public lightState lState =  lightState.lit;
    
    public static event Action<bool, Vector2> OnSuccessfulDig;
    public static event Action<bool> OnPlayerHit;
    public static event Action<bool> shovelBreaker;

    public Stack<float> gems = new Stack<float>();

    private void OnEnable()
    {
        GemScript.onGemDug += updateScoreStack;
        OnPlayerHit += Stun;
    }

    private void OnDisable()
    {
        GemScript.onGemDug -= updateScoreStack;
        OnPlayerHit -= Stun;
    }

    private void Stun(bool player){
        if(isRed == player){
            aState = actionState.stunned;
        }
    }

    IEnumerator StunTime()
    {
        yield return new WaitForSeconds(stunTime);
        aState = actionState.idle;
        rb.velocity = Vector2.zero;
        Debug.Log(rb.velocity);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //playerHit = GetComponentInParent<OnHit>();
        height = Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
        stunTime = 3;
    }

    private void FixedUpdate()
    {      
        if(aState != actionState.stunned){
            handleMovement();       
        }
    }


    private void Update(){   
        HandleAttackBoxMovement();
        if(aState != actionState.stunned){
            checkpassdig();
            handleOther();
            handleLighting();
        } else {
            StartCoroutine(StunTime());
        }
        animator.SetInteger("aState", (int)aState);
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
            if (Input.GetKey(down) || Input.GetKey(right) || Input.GetKey(left))
            {
                return;
            }
        }
        if (Input.GetKey(down))
        {
            aState = actionState.running;
            rb.position -= (Vector2)transform.up * Time.deltaTime * moveSpeed;
            called = true;
            if (Input.GetKey(up) || Input.GetKey(right) || Input.GetKey(left))
            {
                return;
            }
        }

        if (Input.GetKey(right))
        {
            aState = actionState.running;
            rb.position += (Vector2)transform.right * Time.deltaTime * moveSpeed;
            called = true;
            if (Input.GetKey(down) || Input.GetKey(up) || Input.GetKey(left))
            {
                return;
            }
        }

        if (Input.GetKey(left))
        {
            aState = actionState.running;
            rb.position -= (Vector2)transform.right * Time.deltaTime * moveSpeed;
            called = true;
            if (Input.GetKey(down) || Input.GetKey(right) || Input.GetKey(up))
            {
                return;
            }
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

        if (Input.GetKey(dig) == true && aState == actionState.idle)
        {
            aState = actionState.digging;
            PassDig = true;
            StartCoroutine(CheckCompletedDig(rb.position));          
        } 
        

        if (Input.GetKeyDown(attackKey))
        {
            aState = actionState.attacking;
            animator.SetInteger("aState", (int)aState);
            Debug.Log("ATTACK PRESSED");
            Attack();
        }
        
        
    }

    void Attack()
    {
        ShovelAttack canAttack;

        canAttack = GetComponentInChildren<ShovelAttack>();

        Debug.Log(canAttack);
        Debug.Log(canAttack.noShovel);

        if (canAttack.noShovel == true && canAttack != null)
        {
            return;
        }

        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange, playerLayer);
        foreach (Collider2D enemy in hitPlayer)
        {
            PlayerMovement otherPlayer;


            if (shovelBreaker!= null)
            {
                shovelBreaker(isRed);
            }
            
            otherPlayer = enemy.GetComponent<PlayerMovement>();

            if (otherPlayer.aState == PlayerMovement.actionState.stunned)
            {
                return;
            }

            Debug.Log(enemy.name + "GOT HIT");
            if(OnPlayerHit != null)
            {
                OnPlayerHit(!isRed);
            }

            AddForce(attackForce, enemy);
        }
    }

    public void checkpassdig(){
        if(aState != actionState.digging || lState != lightState.lit){
            PassDig = false;
        }
    }

    IEnumerator CheckCompletedDig(Vector2 startpos){
        yield return new WaitForSeconds(digTime);
        if(PassDig && aState == actionState.digging && rb.position == startpos){
            aState = actionState.idle;
            if (OnSuccessfulDig != null){
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

    private void AddForce(float force, Collider2D enemy)
    {

        

        Rigidbody2D enemyRigidbody = enemy.attachedRigidbody;

        var forceDirection = transform.position - enemy.transform.position;

        forceDirection.Normalize();

        enemyRigidbody.AddForce(-forceDirection*force);

    }

    private void updateScoreStack(bool red, Vector2 pos, float value)
    {
        gems.Push(value);
        if (red && isRed)
        {
            GameObject.FindGameObjectWithTag("RedScore").GetComponent<Score>().updateMe(value);
        }
        else if (!(isRed || red))
        {
            GameObject.FindGameObjectWithTag("BlueScore").GetComponent<Score>().updateMe(value);
        }
    }
}