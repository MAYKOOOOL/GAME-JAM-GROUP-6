using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisionConeRenderer : MonoBehaviour
{
    public float viewRadius = 10f;
    [Range(0, 360)] public float viewAngle = 90f;
    public int segments = 100;
    public LayerMask obstacleMask;

    private Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void LateUpdate()
    {
        DrawVisionCone();
    }

    void DrawVisionCone()
    {
        mesh.Clear();

        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero; // Cone center (local)

        float angleStep = viewAngle / segments;
        float startAngle = -viewAngle / 2;

        for (int i = 0; i <= segments; i++)
        {
            float angle = startAngle + angleStep * i;
            float rad = Mathf.Deg2Rad * angle;
            Vector3 localDir = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));
            Vector3 worldDir = transform.TransformDirection(localDir);

            Vector3 rayOrigin = transform.position;
            Vector3 localVertex;

            if (Physics.Raycast(rayOrigin, worldDir, out RaycastHit hit, viewRadius, obstacleMask))
            {
                localVertex = transform.InverseTransformPoint(hit.point);
            }
            else
            {
                localVertex = localDir * viewRadius;
            }

            vertices[i + 1] = localVertex;
        }

        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

}
