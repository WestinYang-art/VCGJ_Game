using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleScript : MonoBehaviour
{
    private bool canGrapple = false;
    private GameObject player;
    private bool inRange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FixedUpdate()
    {
        GrappleAvailable();
    }

    // constantly checks if a grapple point is avaiable or not and then decides whether you grapple or not based on that
    private void GrappleAvailable()
    {
        /*player.GetComponent<PlayerMovement>().getRange() = inRange;
        if (canGrapple && player.Input.GetButtonDown("grapple") && inRange == true) // if the grapple point is available and the grapple key is pressed
        {

        }
        else if (canGrapple == false && player.Input.GetButtonUp("grapple")) // if the grapple point is let go while connected to the grapple point
        {

        }
        else
        {
            canGrapple = false;
        }*/
    }

    // will be used by the game initializer to set player variable to the player game object in the scene
    public void setPlayer(GameObject player)
    {
        this.player = player;
    }
}
