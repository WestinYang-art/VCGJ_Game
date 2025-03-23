using UnityEngine;

public class SimpleMeshRenderTest : MonoBehaviour
{
    public Material testMaterial;
    public float radius = 5f;
    public int segments = 30;

    void Start()
    {
        // Create the GameObject
        GameObject fovTestObject = new GameObject("FOV_Test");

        // Add MeshFilter & MeshRenderer
        MeshFilter mf = fovTestObject.AddComponent<MeshFilter>();
        MeshRenderer mr = fovTestObject.AddComponent<MeshRenderer>();

        // Apply your red-tinted material
        mr.material = testMaterial;
        mr.sortingLayerName = "Default";
        mr.sortingOrder = 0;
        mr.material.renderQueue = 3000; // Transparent queue

        // Set position to origin
        fovTestObject.transform.position = Vector3.zero;

        // Generate a simple "pie slice" mesh
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero;

        float angleStep = 360f / segments;

        for (int i = 0; i <= segments; i++)
        {
            float angle = Mathf.Deg2Rad * i * angleStep;
            vertices[i + 1] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
        }

        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 2;
            triangles[i * 3 + 2] = i + 1;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mf.mesh = mesh;

        Debug.Log("Simple FOV test mesh created.");
    }
}
