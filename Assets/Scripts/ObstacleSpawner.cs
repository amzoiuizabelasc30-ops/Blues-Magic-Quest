using UnityEngine;

// Creates the obstacles at random when the level starts.
// Every time the level (re)starts, you get a new random layout.
//
// How to use:
// 1. Create an empty object called "ObstacleSpawner" and add this script.
// 2. Drag your obstacle PREFABS (from the Prefabs folder) into "Obstacle Prefabs".
//    If you add several different prefabs, it picks one at random each time.
// 3. (Optional) Drag prefabs into "Must Appear Once" to make sure each of them
//    shows up at least once per level.
public class ObstacleSpawner : MonoBehaviour
{
    [Header("Which obstacles to use (drag prefabs here)")]
    public GameObject[] obstaclePrefabs;

    [Tooltip("Each prefab in this list appears AT LEAST once in the level (at a random spot).")]
    public GameObject[] mustAppearOnce;

    [Header("Where the obstacles go (X positions)")]
    public float startX = 10f;    // first obstacle
    public float endX = 95f;      // no obstacles after this

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
    public int maxCollectibles = 12;

    void Start()
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0)
        {
            Debug.LogWarning("ObstacleSpawner: no prefabs assigned!");
            return;
        }

        // 1. Work out all the positions first
        System.Collections.Generic.List<Vector3> positions = new System.Collections.Generic.List<Vector3>();
        float x = startX;
        float lastY = 0f;
        while (x <= endX)
        {
            float y = lastY + Random.Range(-maxYChange, maxYChange);
            y = Mathf.Clamp(y, minY, maxY);
            positions.Add(new Vector3(x, y, 0f));
            lastY = y;
            x += Random.Range(minSpacing, maxSpacing);
        }

        // 2. Pick a random prefab for every position
        GameObject[] chosen = new GameObject[positions.Count];
        for (int i = 0; i < chosen.Length; i++)
            chosen[i] = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        // 3. Make sure the "must appear once" prefabs are in there (each at a different spot)
        if (mustAppearOnce != null)
        {
            bool[] reserved = new bool[chosen.Length];
            foreach (GameObject must in mustAppearOnce)
            {
                if (must == null) continue;
                for (int tries = 0; tries < 50; tries++)
                {
                    int i = Random.Range(0, chosen.Length);
                    if (reserved[i]) continue;
                    chosen[i] = must;
                    reserved[i] = true;
                    break;
                }
            }
        }

        // 4. Create them
        for (int i = 0; i < chosen.Length; i++)
        {
            if (chosen[i] == null) continue; // skip empty slots instead of crashing
            Instantiate(chosen[i], positions[i], Quaternion.identity, transform);
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
