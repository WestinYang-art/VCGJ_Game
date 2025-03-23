using UnityEngine;

public class SecurityCamera : MonoBehaviour
{

    [Header("Rotation Settings")]
    public float rotationAngle = 45f; // Maximum angle from the initial position
    public float rotationSpeed = 2f;  // Speed of the rotation

    [Header("Movement Settings")]
    public int test = 0;

    [Header("Detection Settings")]
    public Transform player;
    public float detectionRange = 10f;
    public float fieldOfView = 30f; // Half-angle
    public LayerMask obstructionMask;
    


    [Header("Field of View Settings")]
    public Transform fovObject; // Drag your FOV child here
    public int meshResolution = 30;
    public Material fovMaterial;

    [Header("Sorting Settings")]
    public string spriteSortingLayer = "Default";
    public int spriteSortingOrder = 0;
    public string fovSortingLayer = "Default";
    public int fovSortingOrder = -1; // Render behind the sprite

    private Mesh fovMesh;

    private float startZRotation;
    //Current zRotation
    float zRotation;
    public float detectionValue;
    public float distanceToPlayer;
    private float timer = 0f;
    private bool timerRunning = true;
    private bool playerDetected;
    private bool wasPlayerDetected;

    
    //State 0 is the idle state
    //State 1 is the player detected, locked state
    //State 2 is returning to idle
    private int state = 0;

    void Start()
    {
        startZRotation = transform.eulerAngles.z;

        // Create mesh on the child object
        MeshFilter mf = fovObject.gameObject.AddComponent<MeshFilter>();
        MeshRenderer mr = fovObject.gameObject.AddComponent<MeshRenderer>();

        fovMesh = new Mesh();
        mf.mesh = fovMesh;
        mr.material = fovMaterial;

        mr.material.SetFloat("_Range", detectionRange);
        mr.material.SetFloat("_Falloff", 0.6f);
        mr.material.SetFloat("_MinAlpha", 0.3f);

        // Set FOV mesh to render behind the sprite
        mr.sortingLayerName = fovSortingLayer;
        mr.sortingOrder = fovSortingOrder;

        // Optional: also configure your sprite sorting in this script if needed
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingLayerName = spriteSortingLayer;
            sr.sortingOrder = spriteSortingOrder;
        }
    }

    void Update()
    {
        if (timerRunning)
        {
           timer += Time.deltaTime;
        }
        wasPlayerDetected = playerDetected;
        playerDetected = DetectPlayer();

        // Handle state transitions
        if (playerDetected)
        {
            state = 1;
        }
        else if (wasPlayerDetected && !playerDetected && state == 1)
        {
            state = 2; // Transition to returning to idle
        }
        else if (state == 2)
        {
            // Stay in state 2 until RotateBack() finishes the rotation
        }
        else if (!playerDetected && state != 2)
        {
            state = 0;
        }


        switch (state) {
            case 0:
                RotateCamera();
                break;
            case 1:
                focusPlayer();
                break;
            case 2:
                RotateBack();
                break;
        }
        Debug.Log(state);
        DrawFOV();
    }

    //Passive State
    void RotateCamera()
    {
        timerRunning = true;
        // Back and forth rotation using sine wave
        zRotation = startZRotation + Mathf.Sin(timer * rotationSpeed) * rotationAngle;
        transform.rotation = Quaternion.Euler(0, 0, zRotation);
    }

    //Locked on Player State
    void focusPlayer()
    {
        timerRunning = false;
        Vector2 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), 200f * Time.deltaTime);
    }

    //Can I see the player?
    bool DetectPlayer()
    {
        Vector2 directionToPlayer = player.position - transform.position;
        distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer < detectionRange)
        {
            float angleToPlayer = Vector2.Angle(transform.right, directionToPlayer);

            if (angleToPlayer < fieldOfView)
            {
                // Raycast to detect if there's an obstruction
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, detectionRange, ~obstructionMask);
                if (hit.collider != null && hit.collider.transform == player)
                {
                    state = 1;
                    //No longer sweeping
                    timerRunning = false;
                    //Debug.Log("Player detected!");
                    return true;
                }
            }
        }
        return false;
    }

    void RotateBack()
    {
        Quaternion targetRotation = Quaternion.Euler(0, 0, zRotation);

        // Smoothly rotate back towards the target rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 50f * Time.deltaTime);

        // Check if we have reached (or are very close to) the target rotation
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.5f)
        {
            // Reset back to idle state
            state = 0;
            timerRunning = true; // Resume the sweeping motion
        }
        if (DetectPlayer()) {
            state = 1;
        }
    }

    void DrawFOV()
    {
        fovMesh.Clear();

        Vector3[] vertices = new Vector3[meshResolution + 2];
        int[] triangles = new int[meshResolution * 3];

        vertices[0] = Vector3.zero; // origin is now the FOV child position

        float angleStep = (fieldOfView * 2f) / meshResolution;

        for (int i = 0; i <= meshResolution; i++)
        {
            float angle = -fieldOfView + angleStep * i;
            float rad = Mathf.Deg2Rad * angle;
            Vector3 vertex = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * (detectionRange);
            vertices[i + 1] = vertex;
        }

        for (int i = 0; i < meshResolution; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 2;
            triangles[i * 3 + 2] = i + 1;
        }
        fovMesh.vertices = vertices;
        fovMesh.triangles = triangles;

    }
    // Optional: Visualize detection range and FOV in Scene view
    void OnDrawGizmosSelected()
    {
        //Sphere of detection
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Vector3 leftBoundary = Quaternion.Euler(0, 0, fieldOfView) * transform.right;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, -fieldOfView) * transform.right;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * detectionRange);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * detectionRange);
    }
    public float getVisionValue()
    {
        return Mathf.Max(0, detectionRange - distanceToPlayer);
    }
}
