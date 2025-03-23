using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CamoManager : MonoBehaviour
{
    public Image camoBar;
    public PlayerMovement script;
    private float currentCamo;
    // Start is called before the first frame update
    void Start()
    {
        script = GameObject.Find("Player").GetComponent<PlayerMovement>();
        currentCamo = script.currentCamo;
    }

    // Update is called once per frame
    void Update()
    {
        camoBar.fillAmount = currentCamo / 5.0f;
    }
}
