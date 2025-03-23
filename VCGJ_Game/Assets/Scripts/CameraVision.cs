using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationAngle = 45f;
    public float rotationSpeed = 2f;

    [Header("Detection Settings")]
    public Transform player;
    public float detectionRange = 10f;
    public float fieldOfView = 30f;
    public LayerMask obstructionMask;
    public bool playerDetected;

    [Header("Field of View Settings")]
    public Transform fovObject;
    public int meshResolution = 30;
    public Material fovMaterial;

    [Header("Sorting Settings")]
    public string spriteSortingLayer = "Default";
    public int spriteSortingOrder = 0;
    public string fovSortingLayer = "Default";
    public int fovSortingOrder = -1;

    private Mesh fovMesh;
    private float startZRotation;
    float distanceToPlayer;

    // New fields
    private bool returningToSweep = false;
    private float returnSpeed = 100f; // Speed to return to sweep rotation
    private float detectionCooldown = 2f; // Seconds to stay in focus mode after losing sight
    private float cooldownTimer = 0f;

    void Start()
    {
        startZRotation = transform.eulerAngles.z;

        MeshFilter mf = fovObject.gameObject.AddComponent<MeshFilter>();
        MeshRenderer mr = fovObject.gameObject.AddComponent<MeshRenderer>();

        fovMesh = new Mesh();
        mf.mesh = fovMesh;
        mr.material = fovMaterial;

        mr.material.SetFloat("_Range", detectionRange);
        mr.material.SetFloat("_Falloff", 0.6f);
        mr.material.SetFloat("_MinAlpha", 0.3f);

        mr.sortingLayerName = fovSortingLayer;
        mr.sortingOrder = fovSortingOrder;

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingLayerName = spriteSortingLayer;
            sr.sortingOrder = spriteSortingOrder;
        }
    }

    void Update()
    {
        if (playerDetected)
        {
            focusPlayer();
            returningToSweep = false;
            cooldownTimer = detectionCooldown;
        }
        else if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            focusPlayer(); // Still focus during cooldown
        }
        else if (!returningToSweep)
        {
            // Start smooth return
            returningToSweep = true;
        }

        if (returningToSweep)
        {
            returnToSweep();
        }
        else if (!playerDetected && cooldownTimer <= 0)
        {
            RotateCamera();
        }

        DetectPlayer();
        DrawFOV();
    }

    void RotateCamera()
    {
        float zRotation = startZRotation + Mathf.Sin(Time.time * rotationSpeed) * rotationAngle;
        transform.rotation = Quaternion.Euler(0, 0, zRotation);
    }

    void focusPlayer()
    {
        Vector2 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), 200f * Time.deltaTime);
    }

    void returnToSweep()
    {
        // Smoothly rotate back to the sweep's "midpoint" angle
        Quaternion targetRotation = Quaternion.Euler(0, 0, startZRotation);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, returnSpeed * Time.deltaTime);

        // If we're close enough, stop returning
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.5f)
        {
            returningToSweep = false;
        }
    }

    void DetectPlayer()
    {
        playerDetected = false;

        Vector2 directionToPlayer = player.position - transform.position;
        distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer < detectionRange)
        {
            float angleToPlayer = Vector2.Angle(transform.right, directionToPlayer);

            if (angleToPlayer < fieldOfView)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, detectionRange, ~obstructionMask);
                if (hit.collider != null && hit.collider.transform == player)
                {
                    playerDetected = true;
                    Debug.Log("Player detected!");
                }
            }
        }
    }

    void DrawFOV()
    {
        fovMesh.Clear();

        Vector3[] vertices = new Vector3[meshResolution + 2];
        int[] triangles = new int[meshResolution * 3];

        vertices[0] = Vector3.zero;

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

    void OnDrawGizmosSelected()
    {
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
        return Mathf.Max(detectionRange - distanceToPlayer,0);
    }
}



