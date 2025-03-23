using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneScript : MonoBehaviour
{
    GameObject canvas;
    VideoPlayer video_player;
    private double time;
    private double current_time;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        canvas.SetActive(false);

        video_player = GetComponent<VideoPlayer>();
        time = video_player.clip.length;
    }

    // Update is called once per frame
    void Update()
    {
        if ((video_player.frame) > 0 && (video_player.isPlaying == false))
        {
            Debug.Log("Deactivate");
            canvas.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
