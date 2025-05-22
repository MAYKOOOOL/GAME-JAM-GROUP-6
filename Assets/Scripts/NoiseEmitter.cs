using UnityEngine;

public class NoiseEmitter : MonoBehaviour
{
    public float soundRadius = 10f;
    public LayerMask aiLayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Trigger noise
        {
            Collider[] aiListeners = Physics.OverlapSphere(transform.position, soundRadius, aiLayer);
            foreach (var listener in aiListeners)
            {
                AIHearing hearing = listener.GetComponent<AIHearing>();
                if (hearing != null)
                {
                    hearing.HearNoise(transform.position);
                }
            }
        }
    }
}
