using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleScript : MonoBehaviour
{
    private bool canGrapple = false;
    private GameObject player;
    private 
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

    // constantly checks if a grapple point is avaiable or not
    private void GrappleAvailable()
    {
        if (canGrapple)
        {

        }
        else if (canGrapple == false)
        {

        }
        else
        {

        }
    }

    // will be used by the game initializer to set player variable to the player game object in the scene
    public void setPlayer(GameObject player)
    {
        this.player = player;
    }
}
