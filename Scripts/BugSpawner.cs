using UnityEngine;

public class BugSpawner : MonoBehaviour
{
    public GameObject bugPrefab;      // Prefab-ul gândacului
    public Transform spawnArea;       // Punctul central de spawn
    public int numberOfBugs = 20;     // Câți gândaci să apară
    public float spawnRangeX = 5f;    // Distanța maximă pe axa X
    public float spawnRangeY = 3f;    // Distanța maximă pe axa Y

    private bool hasSpawned = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasSpawned)
        {
            SpawnBugs();
            hasSpawned = true;
        }
    }

    void SpawnBugs()
    {
        for (int i = 0; i < numberOfBugs; i++)
        {
            // Calculează o poziție aleatoare pe X și Y
            float randomX = spawnArea.position.x + Random.Range(-spawnRangeX, spawnRangeX);
            float randomY = spawnArea.position.y + Random.Range(-spawnRangeY, spawnRangeY);
            
            // Păstrează Z-ul fix (sau poți adăuga variație dacă e necesar)
            float fixedZ = spawnArea.position.z;
            
            Vector3 spawnPosition = new Vector3(randomX, randomY, fixedZ);
            
            // Instantiază gândacul
            Instantiate(bugPrefab, spawnPosition, Quaternion.identity);
        }
        Debug.Log($"Au apărut {numberOfBugs} gândaci în jurul lui {spawnArea.name}!");
    }
}