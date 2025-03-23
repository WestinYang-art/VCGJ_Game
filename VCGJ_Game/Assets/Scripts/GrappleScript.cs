using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GrappleScript : MonoBehaviour
{
    private GameObject player;
    public GameObject gameManager;
    private bool inRange;
    private Vector2 grapplePos;
    private Vector2 rotation;
    // move this to player movement when you can, actually don't it's better that each grapple point knows where it is relative to the player than the player to the points also this is old code oh god
    /*private float negDegrees;
    private float posDegrees;
    public float cone;
    private float rotZ;
    private float tNegDegrees;
    private float tPosDegrees;*/
    private float rotZ;
    private float degrees;

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
        if (player.Input.GetButtonDown("grapple") && inRange == true) // if the grapple point is available and the grapple key is pressed
        {
            player.getTentacles(DegCalc());
        }
        else if (player.Input.GetButtonUp("grapple")) // if the grapple point is let go while connected to the grapple point
        {
            
        }
        else

        }*/
    }

    // will be used by the game initializer to set player variable to the player game object in the scene
    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    // translates the degrees to a 360 deg circle around the player PS. this is old code for a previous game
    /*private void degCalc()
    {
        negDegrees = rotZ - cone;

        posDegrees = rotZ + cone;

        if (negDegrees > 180)
        {
            tNegDegrees = 0 - (180 + (180 - negDegrees));
        }
        else
        {
            tNegDegrees = negDegrees;
        }

        if (posDegrees > 180)
        {
            tPosDegrees = 0 - (180 + (180 - posDegrees));
        }
        else
        {
            tPosDegrees = posDegrees;
        }
    }*/

    // translates the degress around the player to a 360 deg circle
    private void DegCalc()
    {
        grapplePos = this.GetComponent<Rigidbody2D>().position;

        rotZ = Vector2.Angle(grapplePos, player.GetComponent<Rigidbody2D>().position);

        if (rotZ < 0)
        {
            degrees = 180 + (rotZ + 180);
        }
    }

    // checks the radial direction of the tentacles and then uses the TentacleCheck function to decide whether or not to shoot a tentacle there
    void GetTentacles(float deg)
    {
        if (deg >= 316 && deg <= 361)
        {
            player.GetComponent<PlayerMovement>().TentacleCheck(0);
        }
        else if (deg >= 0 && deg <= 45)
        {
            player.GetComponent<PlayerMovement>().TentacleCheck(0);
        }
        else if (deg >= 46 && deg <= 135)
        {
            player.GetComponent<PlayerMovement>().TentacleCheck(1);
        }
        else if (deg >= 136 && deg <= 225)
        {
            player.GetComponent<PlayerMovement>().TentacleCheck(2);
        }
        else if (deg >= 226 && deg <= 315)
        {
            player.GetComponent<PlayerMovement>().TentacleCheck(3);
        }
        else
        {
            Debug.Log("your deg is either too large or small");
        }
    }
}
