using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManController : MonoBehaviour

{
    public float speed = 5.0f;
    private GameObject npc;

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
    }

    private void FixedUpdate()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, npc.transform.position - transform.position);  
        if (ray.collider != null)
        {
            hasLineofSight = ray.collider.CompareTag("NPC");
            if(hasLineofSight)
            {
                Debug.DrawRay(transform.position, npc.transform.position - transform.position, Color.green);
                
            }
            else
            {
                Debug.DrawRay(transform.position, npc.transform.position - transform.position, Color.red);
            }
        }
    }
}
