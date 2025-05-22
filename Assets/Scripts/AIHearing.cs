using UnityEngine;

public class AIHearing : MonoBehaviour
{
    public float hearingRange = 15f;

    public void HearNoise(Vector3 soundPos)
    {
        float dist = Vector3.Distance(transform.position, soundPos);
        if (dist <= hearingRange)
        {
            Debug.Log("Sound heard! Investigating...");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, hearingRange);
    }
}
