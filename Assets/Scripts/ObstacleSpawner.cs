using UnityEngine;

// Creates the obstacles at random when the level starts.
// Every time the level (re)starts, you get a new random layout.
//
// How to use:
// 1. Create an empty object called "ObstacleSpawner" and add this script.
// 2. Drag your obstacle PREFABS (from the Prefabs folder) into "Obstacle Prefabs".
//    If you add several different prefabs, it picks one at random each time.
public class ObstacleSpawner : MonoBehaviour
{
    [Header("Which obstacles to use (drag prefabs here)")]
    public GameObject[] obstaclePrefabs;

    [Header("Where the obstacles go (X positions)")]
    public float startX = 10f;    // first obstacle
    public float endX = 95f;      // no obstacles after this (the ground ends ~105)

    [Header("Space between obstacles")]
    public float minSpacing = 7f;
    public float maxSpacing = 9f;

    [Header("How high or low the gap can be")]
    public float minY = -1.5f;
    public float maxY = 1.5f;

    [Tooltip("Max height difference between two gaps in a row, so it's never impossible.")]
    public float maxYChange = 2f;
    
    [Header("Collectibles")]
    [Tooltip("How many collectibles to keep in the level. The rest are removed at random. -1 = keep all.")]
    public int maxCollectibles = 6;

    void Start()
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0)
        {
            Debug.LogWarning("ObstacleSpawner: no prefabs assigned!");
            return;
        }

        float x = startX;
        float lastY = 0f;

        while (x <= endX)
        {
            // Pick a random obstacle type
            GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

            // Pick a random height, but not too far from the previous one
            float y = lastY + Random.Range(-maxYChange, maxYChange);
            y = Mathf.Clamp(y, minY, maxY);

            Instantiate(prefab, new Vector3(x, y, 0f), Quaternion.identity, transform);

            lastY = y;
            x += Random.Range(minSpacing, maxSpacing);
        }

        LimitCollectibles();
    }

    // Keeps only "maxCollectibles" collectibles, chosen at random, and removes the others.
    void LimitCollectibles()
    {
        if (maxCollectibles < 0) return;

        Collectible[] all = GetComponentsInChildren<Collectible>();
        if (all.Length <= maxCollectibles) return;

        // Shuffle the list
        for (int i = all.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Collectible temp = all[i];
            all[i] = all[j];
            all[j] = temp;
        }

        // Remove everything after the first "maxCollectibles"
        for (int i = maxCollectibles; i < all.Length; i++)
            Destroy(all[i].gameObject);
    }
}