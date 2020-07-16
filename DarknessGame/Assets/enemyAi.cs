using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Experimental.Rendering.Universal;

public class enemyAi : MonoBehaviour
{
    [SerializeField] public PlayerMovement redTarget;
    [SerializeField] public PlayerMovement blueTarget;

    
    

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);

        
        
    }

    void UpdatePath()
    {
        if(seeker.IsDone()){
            seeker.StartPath(rb.position, getTarget(redTarget,blueTarget), OnPathComplete);
        }
        

    }

    void OnPathComplete(Path p){
        if(!p.error)
        {
                path = p;
                currentWaypoint = 0;

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint ++;
        }



    }

    private Vector2 getTarget(PlayerMovement red, PlayerMovement blue){
        if(red.lstate == PlayerMovement.lightState.lit && blue.lstate == PlayerMovement.lightState.dark){
                return red.transform.position;
        } 
        else if (red.lstate == PlayerMovement.lightState.dark && blue.lstate == PlayerMovement.lightState.lit){
            return blue.transform.position;
        }
        else if(red.lstate == PlayerMovement.lightState.lit && blue.lstate == PlayerMovement.lightState.lit){
            if(Vector2.Distance(rb.position,red.transform.position) <= Vector2.Distance(rb.position,blue.transform.position)){
                return red.transform.position;
            } else {
                return blue.transform.position;
            }
        } else {
            return new Vector2(-2,-4);
        }
    }
}
