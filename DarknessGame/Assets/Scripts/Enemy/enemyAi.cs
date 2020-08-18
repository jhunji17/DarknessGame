using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Experimental.Rendering.Universal;
using CodeMonkey.Utils;

public class enemyAi : MonoBehaviour
{
    [SerializeField] public PlayerMovement redTarget;
    [SerializeField] public PlayerMovement blueTarget;

    

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    private Vector3 startPos;
    private Vector3 roamPos;
    private Vector2 target;
    private bool targetIsPlayer;

    public float attackRange = 2f;

    Seeker seeker;
    Rigidbody2D rb;

    // stuff for attack
    public float hitboxRange;
    public LayerMask playerLayer;
    private Vector2 previousZombiePosition;
    public float jumpSpeed;

    public enum State {seeking,patrolling,attacking};
    private State state;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        startPos = rb.position;
        roamPos = getRandomRoamingPos();
        InvokeRepeating("UpdatePath",0f, 0.5f);
 
    }

    void UpdatePath()
    {
        if(seeker.IsDone()){
            seeker.StartPath(rb.position, target, OnPathComplete);
        }
    }

    void OnPathComplete(Path p){
        if(!p.error)
        {
                path = p;
                currentWaypoint = 0;

        }
    }

    //fix the attack function
    void FixedUpdate()
    {
        target = getTargetAndSetStates(redTarget,blueTarget); 
        
        if(Vector2.Distance(rb.position, target) <= attackRange && targetIsPlayer){
            state = State.attacking;
            Debug.Log("ATTACK");
            attack();
        } else {
            if(state == State.attacking)
            {
                return;
            }
            moveAlongPath();
            previousZombiePosition = gameObject.transform.position;
        }
    }

    private void attack()
    {
        // get zombie to jump, probably need to cancle pathing or somehting, idk

        gameObject.transform.position = Vector2.Lerp(previousZombiePosition, target, jumpSpeed);
        Debug.Log(gameObject.transform.position);
        Debug.Log(target);
        Debug.Log(previousZombiePosition);
        // get zombie to stun
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(gameObject.transform.position, hitboxRange, playerLayer);
        foreach (Collider2D player in hitPlayer)
        {
            PlayerMovement playerHit;

            playerHit = player.GetComponent<PlayerMovement>();

            // to stop the zombie from continuing to follow them once they are hit

            playerHit.lState = PlayerMovement.lightState.dark;

            if (playerHit.aState == PlayerMovement.actionState.stunned)
            {
                return;
            }

            // doing this cause calling Stun event is disgusting, you can try if you like idk how to do it

            playerHit.aState = PlayerMovement.actionState.stunned;

            playerHit.StartCoroutine(playerHit.StunTime());

            state = State.seeking;
        }

        return;
    }

    // to show hit box

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, hitboxRange);
    }

    private Vector2 getRandomRoamingPos() {
        return startPos + UtilsClass.GetRandomDir() * Random.Range(0f,15f);
    }

    private Vector2 getTargetAndSetStates(PlayerMovement red, PlayerMovement blue){
        if(red.lState == PlayerMovement.lightState.lit && blue.lState == PlayerMovement.lightState.dark){
                state = State.seeking;
                targetIsPlayer = true;
                return red.transform.position;
                
        } 
        else if (red.lState == PlayerMovement.lightState.dark && blue.lState == PlayerMovement.lightState.lit){
            state = State.seeking;
            targetIsPlayer = true;
            return blue.transform.position;
            
        }
        else if(red.lState == PlayerMovement.lightState.lit && blue.lState == PlayerMovement.lightState.lit){
            if(Vector2.Distance(rb.position,red.transform.position) <= Vector2.Distance(rb.position,blue.transform.position)){
                state = State.seeking;
                targetIsPlayer = true;
                return red.transform.position;
                
            } else {
                state = State.seeking;
                targetIsPlayer = true;
                return blue.transform.position;
                
            }
        } else {
            state = State.patrolling;
            targetIsPlayer = false;
            return getRandomRoamingPos();
            
        }
    }


    private void moveAlongPath()
    {
        Debug.Log("moveAlongPathCalled");
        if (path == null){
            return;
        }
        if(currentWaypoint >= path.vectorPath.Count){
            reachedEndOfPath = true;
            return;
        }
        else {
            reachedEndOfPath = false;
        }
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector3 moveDirection = new Vector3(direction.x, direction.y);
        rb.transform.position += moveDirection * Time.deltaTime * speed;
        Debug.Log("force added");
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if(distance < nextWaypointDistance)
        {
            currentWaypoint ++;
        }
    }
  
}   
