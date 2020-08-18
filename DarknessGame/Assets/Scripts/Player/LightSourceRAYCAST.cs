using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSourceRAYCAST : MonoBehaviour
{

    // for detecting what are walls
    public LayerMask walls;
    public GameObject wall;
    private Vector2 rayDirection1;
    private Vector2 rayDirection2;
    // for the actual 'light rays'
    public float lightRadius;
    [Range(60, 360)] public int numberOfRays;


    private void Update()
    {
        //DrawLight(lightRadius, walls, numberOfRays);
        DrawTorch(lightRadius, numberOfRays);
        //DrawOneLine2D(lightRadius);
    }

    // this should draw the light rays going out from the point, sorta works, but issue is that this game is 2D not 3D
    //public void DrawLight(float radius, LayerMask collision, int rayCount)
    //{
    //    float degreesBetweenRays = 360 / rayCount;
    //    for (int i = 0; i < rayCount; i++)
    //    {
    //        rayDirection1 = DirectionFromAngle(degreesBetweenRays);
    //        degreesBetweenRays += 360 / rayCount;

    //        // this function should probaby return a ray, need to figure that out

    //        Ray lightRay = new Ray(gameObject.transform.position, rayDirection1);

    //        RaycastHit wallHit;

    //        if (Physics.Raycast(lightRay, out wallHit, lightRadius))
    //        {
    //            Debug.DrawLine(lightRay.origin, wallHit.point, Color.red);
    //        }
    //        else
    //        {
    //            Debug.DrawLine(lightRay.origin, (Vector2)lightRay.origin + rayDirection1 * lightRadius, Color.green);
    //        }

    //    }

    //}

    // this is the 2d version, preferred but hey, coding is f u n
    public void DrawTorch(float radius, int rayCount)
    {
        float degreesBetweenRays = 360 / rayCount;
        for (int i = 0; i < rayCount; i++)
        {
            rayDirection2 = DirectionFromAngle2D(degreesBetweenRays);

            degreesBetweenRays += 360 / rayCount;

            Ray2D lightRay = new Ray2D(transform.position, rayDirection2);

            RaycastHit2D wallHit = Physics2D.Raycast(transform.position, rayDirection2, radius, walls);

            if (wallHit.collider != null && wallHit.collider.gameObject == wall)
            {
                if (wallHit.collider.gameObject == wall)
                {
                    Debug.DrawLine(lightRay.origin, wallHit.point, Color.red);
                }
                
            }
            else
            {
                Debug.DrawLine(lightRay.origin, lightRay.origin + rayDirection2*radius, Color.green);
            }

        }     
    }

    //// trying to get one ray to work

    //    public void DrawOneLine2D(float radius)
    //    {
    //        Ray2D lightRay = new Ray2D(transform.position, Vector2.right);
    //        RaycastHit2D hitSomething = Physics2D.Raycast(transform.position, Vector2.right, radius);
    //        if (hitSomething.collider != null)
    //        {
    //            if (hitSomething.collider.gameObject == wall)
    //            {
    //                Debug.DrawLine(lightRay.origin, hitSomething.point, Color.red);
    //            }
    //            else
    //            {
    //                Debug.DrawLine(lightRay.origin, lightRay.origin + Vector2.up * radius, Color.green);
    //            }
    //        }
    //    }

    //// thanks to my BOI SEBASTIAN LAGUE for this

    //public Vector2 DirectionFromAngle(float angleInDegrees)
    //{
    //    return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    //}

    // thanks to mrs thakar for teaching me trig for this <3 <3 <3

    public Vector2 DirectionFromAngle2D(float angleInDegrees)
    {
        return new Vector2(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
    }
}
