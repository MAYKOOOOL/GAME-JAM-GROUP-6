using UnityEngine;

public class MoneyBagCollector : MonoBehaviour
{
    public int collectedCount = 0;
    public int goal = 5;

    private MoneyUIManager uiManager;

    void Start()
    {
        uiManager = FindObjectOfType<MoneyUIManager>();
        uiManager.UpdateUI(collectedCount);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MoneyBag"))
        {
            collectedCount++;
            FindObjectOfType<MoneyBagSpawner>().RemoveBag(other.gameObject);

            uiManager.UpdateUI(collectedCount);

            if (collectedCount >= goal)
            {
                Debug.Log("Success! Goal Reached.");
                // Add win screen or transition here
            }
        }
    }
}
