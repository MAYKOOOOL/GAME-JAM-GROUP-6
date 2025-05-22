using UnityEngine;
using System.Collections.Generic;

public class MoneyBagSpawner : MonoBehaviour
{
    public GameObject moneyBagPrefab;
    public int maxBags = 7;
    public float spawnRadius = 50f;
    public LayerMask groundMask;
    public List<GameObject> currentBags = new List<GameObject>();

    void Start()
    {
        SpawnBags();
    }

    void SpawnBags()
    {
        for (int i = 0; i < maxBags; i++)
        {
            Vector3 spawnPos = GetRandomSpawnPoint();
            GameObject bag = Instantiate(moneyBagPrefab, spawnPos, Quaternion.identity);
            currentBags.Add(bag);
        }
    }

    Vector3 GetRandomSpawnPoint()
    {
        Vector3 point;
        int attempts = 0;
        do
        {
            Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
            point = new Vector3(randomPoint.x, 50f, randomPoint.y); // Start from height
            Ray ray = new Ray(point, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
            {
                return hit.point + Vector3.up * 1.5f; // Raise a bit above ground
            }
            attempts++;
        } while (attempts < 10);

        return transform.position;
    }

    public void RemoveBag(GameObject bag)
    {
        currentBags.Remove(bag);
        Destroy(bag);
    }
}
