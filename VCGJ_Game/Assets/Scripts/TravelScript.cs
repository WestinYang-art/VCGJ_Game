using System.Collections;
using System.Collections.Generic;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TravelScript : MonoBehaviour
{
    private GameObject player;
    private Vector3 endPosition;
    private GameObject exit;

    void OnCollisionEnter2D(Collision2D collision)
    {
        player = GameObject.Find("Player");
        if (collision.collider.tag == "Gap")
        {
            exit = GameObject.FindWithTag("GapExit"); // assuming that there will be at most one gap/exit pair per level
            endPosition = exit.transform.position;

            if (player.GetComponent<PlayerMovement>().canSqueeze())
            {
                player.transform.position = endPosition;
                player.transform.rotation = player.transform.rotation;
            }

        }
        else if (collision.collider.tag == "Goal")
        {
            exit = GameObject.FindWithTag("Goal");
            Debug.Log("Goal!");
            SceneManager.LoadScene("PlayerTest");
        }
        else if (collision.collider.tag == "Acid")
        {
            Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAGHSAIGhasdbfpoajbskfuabwj,hfbwaself");

        }
        Debug.Log(collision.collider.tag);
    }
}
