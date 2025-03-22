using UnityEngine;

public class SecurityCamera : MonoBehaviour
{

    [Header("Rotation Settings")]
    public float rotationAngle = 45f; // Maximum angle from the initial position
    public float rotationSpeed = 2f;  // Speed of the rotation

    [Header("Detection Settings")]
    public Transform player;
    public float detectionRange = 10f;
    public float fieldOfView = 30f; // Half-angle
    public LayerMask obstructionMask;
    public bool playerDetected;


    [Header("Field of View Settings")]
    public Transform fovObject; // Drag your FOV child GameObject here
    public int meshResolution = 30;
    public Material fovMaterial;


    private Mesh fovMesh;
    private float startZRotation;

    void Start()
    {
        startZRotation = transform.eulerAngles.z;

        // Create mesh on the child object
        MeshFilter mf = fovObject.gameObject.AddComponent<MeshFilter>();
        MeshRenderer mr = fovObject.gameObject.AddComponent<MeshRenderer>();

        fovMesh = new Mesh();
        mf.mesh = fovMesh;
        mr.material = fovMaterial;
    }

    void Update()
    {
        RotateCamera();
        DetectPlayer();
        DrawFOV();
    }

    void RotateCamera()
    {
        // Back and forth rotation using sine wave
        float zRotation = startZRotation + Mathf.Sin(Time.time * rotationSpeed) * rotationAngle;
        transform.rotation = Quaternion.Euler(0, 0, zRotation);
    }

    void DetectPlayer()
    {
        playerDetected = false;

        Vector2 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer < detectionRange)
        {
            float angleToPlayer = Vector2.Angle(transform.right, directionToPlayer);

            if (angleToPlayer < fieldOfView)
            {
                // Raycast to detect if there's an obstruction
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

        vertices[0] = Vector3.zero; // origin is now the FOV child position

        float angleStep = (fieldOfView * 2f) / meshResolution;

        for (int i = 0; i <= meshResolution; i++)
        {
            float angle = -fieldOfView + angleStep * i;
            float rad = Mathf.Deg2Rad * angle;
            Vector3 vertex = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * detectionRange;
            vertices[i + 1] = vertex;
        }

        for (int i = 0; i < meshResolution; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        fovMesh.vertices = vertices;
        fovMesh.triangles = triangles;
    }
    // Optional: Visualize detection range and FOV in Scene view
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
}
