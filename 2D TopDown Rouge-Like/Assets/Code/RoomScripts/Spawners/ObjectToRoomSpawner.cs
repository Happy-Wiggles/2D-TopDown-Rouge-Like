using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToRoomSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct RandomSpawner
    {
        public string name;
        public SpawnerData spawnerData;
    }

    public GridController grid;

    public RandomSpawner[] thingsToSpawn;

    public void InitializeObjectSpawning()
    {
        foreach (RandomSpawner objectToSpawn in thingsToSpawn)
        {
            SpawnObjects(objectToSpawn);
        }
    }

    void SpawnObjects(RandomSpawner data)
    {
        int randomIteration = Random.Range(data.spawnerData.minSpawn, data.spawnerData.maxSpawn + 1);

        for (var i = 0; i < randomIteration; i++)
        {
            if (!(grid.availablePoints.Count == 0))
            {
                int randomPos = Random.Range(0, grid.availablePoints.Count - 1);
                Instantiate(data.spawnerData.itemToSpawn, grid.availablePoints[randomPos], Quaternion.identity, transform);
                grid.availablePoints.RemoveAt(randomPos);
            }
        }
    }
}
