using UnityEngine;

public class AIVision : MonoBehaviour
{
    [Header("Vision Settings")]
    public float viewRadius = 10f;
    [Range(0, 360)] public float viewAngle = 90f;

    [Header("Detection Layers")]
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [Header("Target")]
    public Transform target; // Assign the player transform

    void Update()
    {
        if (target != null && IsTargetInSight(target))
        {
            Debug.Log("Player in sight!");
        }
    }

    public bool IsTargetInSight(Transform target)
    {
        Vector3 dirToTarget = (target.position - transform.position).normalized;

        // Check if within angle
        if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
        {
            float distToTarget = Vector3.Distance(transform.position, target.position);

            // Raycast to see if something blocks line of sight
            if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
            {
                return true;
            }
        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        int stepCount = Mathf.RoundToInt(viewAngle);
        float stepAngleSize = viewAngle / stepCount;
        Vector3 oldPoint = Vector3.zero;

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = -viewAngle / 2 + stepAngleSize * i;
            Vector3 dir = DirFromAngle(angle);
            Vector3 point = transform.position + dir * viewRadius;

            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, viewRadius, obstacleMask))
            {
                point = hit.point;
            }

            if (i > 0)
            {
                Gizmos.color = new Color(0, 0.5f, 1f, 0.3f); // translucent blue
                Gizmos.DrawLine(transform.position, point);
                Gizmos.DrawLine(oldPoint, point);
            }

            oldPoint = point;
        }

        // Draw forward angle bounds
        Vector3 leftBoundary = DirFromAngle(-viewAngle / 2);
        Vector3 rightBoundary = DirFromAngle(viewAngle / 2);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewRadius);

        // Optional: draw line to player
        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }

    // Converts angle in degrees to a direction vector
    Vector3 DirFromAngle(float angle)
    {
        float rad = (angle + transform.eulerAngles.y) * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));
    }
}
