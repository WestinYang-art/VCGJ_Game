using UnityEngine;

public class OctoRenderScript : MonoBehaviour
{
    SpriteRenderer sprite;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        sprite = player.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // in theory, we also want to check whether certain conditions are met
        // but the current focus is making it work 
        if (Input.GetKey(KeyCode.C) && !player.GetComponent<PlayerMovement>().isMoving())
        {
            sprite.color = new Color(1, 1, 1, 0.3f);
        }
        else 
        {
            sprite.color = new Color(1, 1, 1, 1);
        }
        
    }
}
