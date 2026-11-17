
using UnityEngine;
using System.Collections.Generic;

public class TrackSpawner : MonoBehaviour
{
    public GameObject groundPrefab;
    public Transform player;

    public float tileLength = 20f;
    public int tilesOnScreen = 6;

    private float nextSpawnZ = 0f;

    private Queue<GameObject> activeTiles =
        new Queue<GameObject>();

    void Start()
    {
        // Create the first six ground tiles
        for (int i = 0; i < tilesOnScreen; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        if (player == null || groundPrefab == null)
            return;

        // When the player approaches the end, add a tile
        if (player.position.z + 40f >= nextSpawnZ)
        {
            SpawnTile();

            // Keep only six tiles active
            if (activeTiles.Count > tilesOnScreen)
            {
                Destroy(activeTiles.Dequeue());
            }
        }
    }

    void SpawnTile()
    {
        // Place tiles end-to-end
        Vector3 spawnPosition = new Vector3(
            0f,
            -0.5f,
            nextSpawnZ + tileLength / 2f
        );

        GameObject newTile = Instantiate(
            groundPrefab,
            spawnPosition,
            Quaternion.identity
        );

        activeTiles.Enqueue(newTile);

        nextSpawnZ += tileLength;
    }
}
