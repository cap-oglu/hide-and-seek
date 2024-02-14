using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManController : MonoBehaviour

{
    public float speed = 5.0f;
    private GameObject npc;
    private float maxVisibleDistance = 10.0f;
    private float viewAngle = 45f;

    private bool hasLineofSight = false;
    // Start is called before the first frame update
    void Start()
    {
        npc = GameObject.FindGameObjectWithTag("NPC");
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        transform.position += new Vector3(moveX, moveY, 0);

        //DrawFOVCircle();
        //DrawFOVLines();
    }

    /*private void DrawFOVCircle()
    {
        float theta = 0;
        float x = maxVisibleDistance * Mathf.Cos(theta);
        float y = maxVisibleDistance * Mathf.Sin(theta);
        Vector3 pos = transform.position + new Vector3(x, y, 0);
        Vector3 newPos = pos;
        Vector3 lastPos = pos;

        for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
        {
            x = maxVisibleDistance * Mathf.Cos(theta);
            y = maxVisibleDistance * Mathf.Sin(theta);

            newPos = transform.position + new Vector3(x, y, 0);
            Debug.DrawLine(lastPos, newPos, Color.blue);
            lastPos = newPos;
        }

        // Connect the last and the first points
        Debug.DrawLine(lastPos, pos, Color.blue);
    }

    private void DrawFOVLines()
    {
        Vector3 rightBoundary = Quaternion.Euler(0, 0, viewAngle / 2) * transform.right * maxVisibleDistance;
        Vector3 leftBoundary = Quaternion.Euler(0, 0, -viewAngle / 2) * transform.right * maxVisibleDistance;

        Debug.DrawLine(transform.position, transform.position + rightBoundary, Color.yellow);
        Debug.DrawLine(transform.position, transform.position + leftBoundary, Color.yellow);
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxVisibleDistance);

        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * maxVisibleDistance);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * maxVisibleDistance);
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    private void FixedUpdate()
    {
        Vector2 directionToNpc = npc.transform.position - transform.position;
        float angleToNpc = Vector2.Angle(transform.right, directionToNpc); // Assuming the player's right is the forward direction
        if (angleToNpc <= viewAngle / 2 && directionToNpc.magnitude <= maxVisibleDistance)
        {
            RaycastHit2D ray = Physics2D.Raycast(transform.position, npc.transform.position - transform.position, maxVisibleDistance);
            if (ray.collider != null)
            {
                hasLineofSight = ray.collider.CompareTag("NPC");
                if (hasLineofSight)
                {
                    Debug.DrawRay(transform.position, npc.transform.position - transform.position, Color.green);

                }
                else
                {
                    Debug.DrawRay(transform.position, npc.transform.position - transform.position, Color.red);
                }
            }
        }
        else
        {
            //Debug.DrawRay(transform.position, npc.transform.position - transform.position, Color.red);
        }
    }
}
