using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStart : MonoBehaviour
{
    public GameObject[] grapplePoints;
    public GameObject player;    

    // Start is called before the first frame update
    void Start()
    {
        grappleInitialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // assigns each grapplePoint player variable to the player in the scene
    private void grappleInitialize()
    {
        foreach (GameObject grapplePoint in grapplePoints)
        {
            //grapplePoint.GetComponent<GrappleScript>().setPlayer() = player;
        }
    }
}
