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
    public int stunTime;
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

    public IEnumerator StunTime()
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
        Debug.Log(gameObject.name + " " + gems.Peek());
        
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
            //  for the new raycast lights

            MeshRenderer MR;
            MR = GetComponentInChildren<MeshRenderer>();

            

            if (lState == lightState.lit)
            {
                MR.enabled = false;
                lState = lightState.dark;
            }
            else
            {
                MR.enabled = true;
                lState = lightState.lit;
            }
        }

        if (Input.GetKey(dig) == true && aState == actionState.idle && lState == lightState.lit)
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

            otherPlayer.lState = PlayerMovement.lightState.dark;

            if (otherPlayer.aState == PlayerMovement.actionState.stunned)
            {
                return;
            }
            
            Debug.Log(enemy.name + "GOT HIT");

            

            //otherPlayer.loseScoreStack();
            //gainScoreStack();

            gainGems(otherPlayer);

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
        
        if (red && isRed)
        {
            gems.Push(value);
            UpdateScoreboard(isRed, value);
            
        }
        else if (!isRed && !red)
        {
            gems.Push(value);
            UpdateScoreboard(isRed, value);
        }
    }


    private void UpdateScoreboard(bool whichPLayer, float value){
        if(whichPLayer){
            GameObject.FindGameObjectWithTag("RedScore").GetComponent<Score>().updateMe(value);
        } else {
            GameObject.FindGameObjectWithTag("BlueScore").GetComponent<Score>().updateMe(value);
        }
    }


    // this is for the stack when you get hit

    public void gainGems(PlayerMovement enemy){
        if(enemy.gems.Count <= 2){
            return;
        } else {

            float x;
            float y;
            x = enemy.gems.Pop();
            UpdateScoreboard(enemy.isRed, -x);
            y = enemy.gems.Pop();
            UpdateScoreboard(enemy.isRed, -y);

            gems.Push(x);
            UpdateScoreboard(isRed, x);
            gems.Push(y);
            UpdateScoreboard(isRed, y);
        }
    }




//     public static float gem1;
//     public static float gem2;
//     public void loseScoreStack()
//     {
//         if (gems.Count >= 2)
//         {
//             Debug.Log(gameObject.name + "LOSE SCORE STACK GOT CALLED");
//             gem1 = gems.Pop();
//             updateScoreStack(isRed, transform.position, -gem1);
//             gem2 = gems.Pop();
//             updateScoreStack(isRed, transform.position, -gem2);
//             Debug.Log("Gem2" + gem2);
//             Debug.Log("Gem1" + gem1);
//         }

//         else
//         {
//             gem1 = 0;
//             gem2 = 0;
//         }
//     }

//     public void gainScoreStack()
//     {
//         Debug.Log(gameObject.name + "GAIN SCORE STACK GOT CALLED");
//         updateScoreStack(isRed, transform.position, gem1);
//         updateScoreStack(isRed, transform.position, gem2);
//     }
}