using System.Collections;
using System.Collections.Generic;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

public class TravelScript : MonoBehaviour
{
    private GameObject player;
    private Vector3 endPosition;
    private GameObject gapExit;

    void OnCollisionEnter2D(Collision2D collision)
    {
        player = GameObject.Find("Player");
        if (collision.collider.tag == "Gap")
        {
            gapExit = GameObject.FindWithTag("GapExit"); // assuming that there will be at most one gap/exit pair per level
            endPosition = gapExit.transform.position;

            if (player.GetComponent<PlayerMovement>().canSqueeze())
            {
                player.transform.position = endPosition;
                player.transform.rotation = player.transform.rotation;
            }

        }
    }
}
