using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public struct RandomSpawner
    {
        public string name;
        public GameObject itemToSpawn;
        public int minSpawn;
        public int maxSpawn;
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
        var currLevel = System.Int32.Parse(GameController.CurrentLevel);
        var changedMaxSpawn = data.maxSpawn + currLevel - 1;
        int randomIteration = Random.Range(data.minSpawn, changedMaxSpawn + 1);
        grid.room.amountOfEnemies = randomIteration;
        
        for (var i = 0; i < grid.room.amountOfEnemies; i++)
        {
            if (!(grid.availablePoints.Count == 0))
            {
                int randomPos = Random.Range(0, grid.availablePoints.Count - 1);
                Instantiate(data.itemToSpawn, grid.availablePoints[randomPos], Quaternion.identity, transform);
                grid.availablePoints.RemoveAt(randomPos);
            }
        }
        
    }
}
