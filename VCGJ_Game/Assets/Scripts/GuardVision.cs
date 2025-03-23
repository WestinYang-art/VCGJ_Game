using UnityEngine;

public class GuardVision : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationAngle = 45f;
    public float rotationSpeed = 2f;

    [Header("Movement Settings")]
    public int test = 0;

    [Header("Detection Settings")]
    public Transform player;
    public float detectionRange = 10f;
    public float fieldOfView = 30f;
    public LayerMask obstructionMask;

    [Header("Field of View Settings")]
    public Transform fovObject;
    public int meshResolution = 30;
    public Material fovMaterial;

    [Header("Sorting Settings")]
    public string spriteSortingLayer = "Default";
    public int spriteSortingOrder = 0;
    public string fovSortingLayer = "Default";
    public int fovSortingOrder = -1;

    [Header("Patrol Settings")]
    public bool enablePatrol = false;
    public Vector2 pointA;
    public Vector2 pointB;
    public float moveSpeed = 2f;

    private Vector2 currentTarget;
    private bool facingRight = true;

    private Mesh fovMesh;
    private float startZRotation;
    private float zRotation;
    public float detectionValue;
    public float distanceToPlayer;
    private float timer = 0f;
    private bool timerRunning = true;
    private bool playerDetected;
    private bool wasPlayerDetected;

    private int state = 0;

    void Start()
    {
        startZRotation = transform.eulerAngles.z;

        if (enablePatrol)
        {
            currentTarget = pointB;
        }

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
        if (timerRunning)
        {
            timer += Time.deltaTime;
        }

        wasPlayerDetected = playerDetected;
        playerDetected = DetectPlayer();

        if (playerDetected)
        {
            state = 1;
        }
        else if (wasPlayerDetected && !playerDetected && state == 1)
        {
            state = 2;
        }
        else if (state == 2)
        {
            // Waiting for RotateBack()
        }
        else if (!playerDetected && state != 2)
        {
            state = 0;
        }

        if (enablePatrol)
        {
            Patrol();
        }

        switch (state)
        {
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

        fovObject.position = transform.position;
        DrawFOV();
    }

    void Patrol()
    {
        if (state != 0) return;

        Vector2 currentPosition = transform.position;
        transform.position = Vector2.MoveTowards(currentPosition, currentTarget, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(currentPosition, currentTarget) < 0.1f)
        {
            currentTarget = (currentTarget == pointA) ? pointB : pointA;
            FlipCamera();
        }
    }

    void FlipCamera()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void RotateCamera()
    {
        timerRunning = true;
        zRotation = startZRotation + Mathf.Sin(timer * rotationSpeed) * rotationAngle;
        transform.rotation = Quaternion.Euler(0, 0, zRotation);
    }

    void focusPlayer()
    {
        timerRunning = false;
        Vector2 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), 200f * Time.deltaTime);
    }

    bool DetectPlayer()
    {
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
                    timerRunning = false;
                    return true;
                }
            }
        }
        return false;
    }

    void RotateBack()
    {
        Quaternion targetRotation = Quaternion.Euler(0, 0, zRotation);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 50f * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.5f)
        {
            state = 0;
            timerRunning = true;
        }
        if (DetectPlayer())
        {
            state = 1;
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
            float angle = facingRight
                ? -fieldOfView + angleStep * i
                : fieldOfView - angleStep * i;

            float rad = Mathf.Deg2Rad * angle;
            Vector3 vertex = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * detectionRange;
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

        if (enablePatrol)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(pointA, 0.1f);
            Gizmos.DrawSphere(pointB, 0.1f);
        }
    }

    public float getVisionValue()
    {
        return Mathf.Max(0, detectionRange - distanceToPlayer);
    }
}
