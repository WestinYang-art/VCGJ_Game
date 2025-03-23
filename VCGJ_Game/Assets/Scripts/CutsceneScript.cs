using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneScript : MonoBehaviour
{
    GameObject world;
    GameObject actors;
    VideoPlayer video_player;
    private double time;
    private double current_time;
    // Start is called before the first frame update
    void Start()
    {
        world = GameObject.Find("Game World");
        world.SetActive(false);
        actors = GameObject.Find("Game Actors");
        actors.SetActive(false);

        video_player = GetComponent<VideoPlayer>();
        time = video_player.clip.length;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) || ((video_player.frame) > 0 && (video_player.isPlaying == false)))
        {
            Debug.Log("Deactivate");
            world.SetActive(true);
            actors.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
