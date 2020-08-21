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
    [Range(1,360)] public int numberOfRays;

    //for the light box manipulation
    public MeshFilter viewMeshFilter;
    Mesh viewMesh;
    public float cutaway;


    private void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

    }

    private void Update()
    {
        DrawRays(lightRadius, numberOfRays);
        //DrawOneLine2D(lightRadius);
        
    }


    // this is the 2d version, preferred but hey, coding is f u n
    public void DrawRays(float radius, int rayCount)
    {
        // this is for the mesh creation

        List<Vector2> lightPoints = new List<Vector2>();


        float degreesBetweenRays = 360 / rayCount;
        for (int i = 0; i < rayCount; i++)
        {
            rayDirection2 = DirectionFromAngle2D(degreesBetweenRays);

            degreesBetweenRays += 360 / rayCount;

            Ray2D lightRay = new Ray2D(transform.position, rayDirection2);

            RaycastHit2D wallHit = Physics2D.Raycast(transform.position, rayDirection2, radius, walls);

            if (wallHit.collider != null && wallHit.collider.gameObject == wall)
            {
                //Debug.DrawLine(lightRay.origin, wallHit.point, Color.green);
                lightPoints.Add(wallHit.point + rayDirection2*cutaway);
            }
            else
            {
                //Debug.DrawLine(lightRay.origin, lightRay.origin + rayDirection2*radius, Color.green);
                lightPoints.Add(lightRay.origin + rayDirection2 * radius);
            }
        }

        // S E B A S T I A N  L A G U E  I S  B A E 

        // I lie I can't do this

        int vertexCount = lightPoints.Count + 1;

        Vector3[] verticies = new Vector3[vertexCount];

        int[] triangles = new int[(vertexCount -  1) * 3];

        verticies[0] = Vector2.zero;

        for(int i =0; i<vertexCount - 1; i++)
        {

            verticies[i + 1] = transform.InverseTransformPoint(lightPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
            if (i == vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = 1;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = verticies;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();

    }

    // one ray tester

    public void DrawOneLine2D(float radius)
    {
        Ray2D lightRay = new Ray2D(transform.position, Vector2.right);
        RaycastHit2D hitSomething = Physics2D.Raycast(transform.position, Vector2.right, radius, walls);
        if (hitSomething.collider != null)
        {
            if (hitSomething.collider!= null && hitSomething.collider.gameObject == wall)
            {
                Debug.DrawLine(lightRay.origin, hitSomething.point, Color.red);
            }
            else
            {
                Debug.DrawLine(lightRay.origin, lightRay.origin + Vector2.right * radius, Color.green);
            }
        }
    }


    public Vector2 DirectionFromAngle2D(float angleInDegrees)
    {
        return new Vector2(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
    }

    // need this to show some wall

    public Vector2 LineExtender(Vector2 wallhitPoint)
    {
        var direction = wallhitPoint - (Vector2)transform.position;
        direction = direction.normalized;
        direction = direction*cutaway;
        
        return direction + wallhitPoint;
    }
}
