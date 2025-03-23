using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamoBarScript : MonoBehaviour
{
    public Slider camoBar;
    public float playerCamo;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCamo = player.GetComponent<PlayerMovement>().getCamo();
        camoBar = GetComponent<Slider>();
        camoBar.maxValue = 100.0f;
        camoBar.value = playerCamo;
    }

    private void Update()
    {
        camoBar.value = player.GetComponent<PlayerMovement>().getCamo();
    }
}
