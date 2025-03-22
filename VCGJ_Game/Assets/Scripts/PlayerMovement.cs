using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    private float horizontal;
    private float vertical;
    public float acceleration;
    public float maxSpeed;
    public GameObject[] tentacles;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        playerMovement();
        getInput();
    }

    private void getInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    void playerMovement() // calculations for acceleration and movement
    {
        if (rb.velocity.x == 0 && (horizontal == -1 || horizontal == 1))
        {
            rb.velocity = new Vector2(horizontal * acceleration, rb.velocity.y);
        }
        else if (horizontal == -1 && rb.velocity.x > (0 - maxSpeed))
        {
            rb.velocity = new Vector2(rb.velocity.x - acceleration, rb.velocity.y);
        }
        else if (horizontal == 1 && rb.velocity.x < maxSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x + acceleration, rb.velocity.y);
        }

        /*if (rb.velocity.y == 0 && (vertical == -1 || vertical == 1))
        {
            rb.velocity = new Vector2(rb.velocity.x, vertical * acceleration);
        }
        else if (vertical == -1 && rb.velocity.y > (0 - maxSpeed))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - acceleration);
        }
        else if (vertical == 1 && rb.velocity.y < maxSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + acceleration);
        }*/

        if (horizontal == 0)
        {
            if (rb.velocity.x > 0 && rb.velocity.x >= acceleration)
            {
                rb.velocity = new Vector2(rb.velocity.x - (acceleration / 2), rb.velocity.y);
            }
            else if (rb.velocity.x < 0 && rb.velocity.x <= 0 - acceleration)
            {
                rb.velocity = new Vector2(rb.velocity.x + (acceleration / 2), rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }

        /*if (vertical == 0)
        {
            if (rb.velocity.y > 0 && rb.velocity.y >= acceleration)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - (acceleration / 2));
            }
            else if (rb.velocity.y < 0 && rb.velocity.y <= 0 - acceleration)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + (acceleration / 2));
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
        }*/
    }

    // this will decide if the player is in range of the grapple point or not ps. for now set it to true just to make sure that it connects xd
    public bool inRange()
    {
        return true;
    }

    public void setTentacles(GameObject[] tentacles) // decide the number of tentacles and where to place them ??? not sure oh well
    {
        this.tentacles = tentacles;
    }
}
