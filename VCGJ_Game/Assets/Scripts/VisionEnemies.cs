using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionEnemies : MonoBehaviour
{
    // List to store the direct children
    public List<Transform> directChildren = new List<Transform>();
    public float TotalVisionValue;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("call back ping");
        GetChildren();
    }

    // Update is called once per frame
    void Update()
    {
        sumVision();
        //Debug.Log(TotalVisionValue);
    }
    void GetChildren()
    {
        // Clear the list first
        directChildren.Clear();

        // Iterate over all child transforms
        foreach (Transform child in transform)
        {
            directChildren.Add(child);
        }

        //DEBUG CODE
        // Optional: Print the names of the direct children
        foreach (Transform child in directChildren)
        {
            Debug.Log("Direct child: " + child.name);
            SecurityCamera fovCamera = child.GetComponent<SecurityCamera>();
            Debug.Log(child.name + ": " + fovCamera.getVisionValue());
        }
    }
    void sumVision()
    {
        TotalVisionValue = 0;
        foreach (Transform child in directChildren)
        {
            SecurityCamera fovCamera = child.GetComponent<SecurityCamera>();
            //Debug.Log(child.name + ": " + fovCamera.getVisionValue());
            TotalVisionValue += fovCamera.getVisionValue();
        }
    }
}
