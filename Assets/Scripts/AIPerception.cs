using UnityEngine;

public class AIPerception : MonoBehaviour
{
    public AIVision vision;
    public AIHearing hearing;

    void Update()
    {
        if (vision != null && vision.target != null && vision.IsTargetInSight(vision.target))
        {
            Debug.Log("AI sees the player!");
        }
    }
}
