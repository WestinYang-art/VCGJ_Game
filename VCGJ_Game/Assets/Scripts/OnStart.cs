using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStart : MonoBehaviour
{
    public GameObject[] grapplePoints;
    public int[][] tentacles = new int[4][];
    public GameObject player;    

    // Start is called before the first frame update
    void Start()
    {
        GrappleInitialize();
        for (int i = 0; i < 4; i++)
        {
            tentacles[i] = new int[2];
            for (int k = 0; k < 2; k++)
            {
                tentacles[i][k] = 1;
            }
        }    
        player.GetComponent<PlayerMovement>().setTentacles(tentacles);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // assigns each grapplePoint player variable to the player in the scene
    private void GrappleInitialize()
    {
        foreach (GameObject grapplePoint in grapplePoints)
        {
            //grapplePoint.GetComponent<GrappleScript>().setPlayer() = player;
        }
    }

    // creates a tentacle between the player and the grapplePoint
    private void TentacleInitialize(GameObject grapplePoint, GameObject Player)
    {
        
    }
}
