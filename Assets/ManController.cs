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
    private Vector3 targetPosition;
    private GameObject[] allNPCs;
    
    void Start()
    {
        allNPCs = GameObject.FindGameObjectsWithTag("NPC");
        //npc = GameObject.FindGameObjectWithTag("NPC");
        targetPosition = transform.position; // Initialize targetPosition with the player's starting position
    }

    // Update is called once per frame
    void Update()
    {
        // Check for left mouse button click
        if (Input.GetMouseButton(0))
        {
            // Convert mouse position to world space
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = transform.position.z; // Ensure target position has the same z-coordinate as the player
        }

        // Move towards the target position
        MoveTowardsTargetPosition();
        Debug.DrawLine(transform.position, transform.position + transform.up * 2, Color.magenta);
        Debug.DrawLine(transform.position, transform.position + transform.right * 2, Color.cyan);
    }

    void MoveTowardsTargetPosition()
    {
        // Calculate the direction vector towards the target position
        Vector3 direction = (targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPosition);

        // Check if the player is close enough to the target position to stop moving
        if (distance > 0.1f) // Threshold distance, adjust as needed
        {
            // Move the player towards the target position
            transform.position += direction * speed * Time.deltaTime;

            // Optional: Adjust player orientation to face the direction of movement
            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90; // Adjusted for sprite facing up
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                
            }
        }


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

        //Gizmos.color = Color.magenta;
        //Gizmos.DrawLine(transform.up, transform.up + new Vector3(0, 1, 0));

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
            angleInDegrees += transform.eulerAngles.z + 90; 
        }
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    private void FixedUpdate()
    {
        foreach(GameObject npc in allNPCs) {
            Vector2 directionToNpc = npc.transform.position - transform.position;
            float angleToNpc = Vector2.Angle(transform.up, directionToNpc); // Assuming the player's up is the forward direction
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
}
