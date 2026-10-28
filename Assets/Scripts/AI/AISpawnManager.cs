using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AISpawnManager : MonoBehaviour
{
    [Header("Spawning Attributes")]

    [Tooltip("List of active points enemies can spawn at")]
    public List<Transform> spawnPoints = new();

    [Tooltip("Private list for spawn points that have been used / recycled till next iteration")]
    private List<Transform> usedSpawnpoints = new();


    [Header("Enemy Spawns")]

    [Tooltip("Active list of possible enemies to spawn")]
    public List<GameObject> spawnableEnemies = new();

    [Tooltip("List of enemies that can be added at round intervals")]
    public List<GameObject> stanbyEnemies = new();

    [Tooltip("Array in which what waves new enemies will be added")]
    public int[] enemyAppearanceWaves;

    /// <summary>
    /// Select random spawners to spawn enemies at for the round
    /// </summary>
    /// <param name="enemiesToSpawn"> How many enemies to spawn </param>
    public void SpawnEnemies(int enemiesToSpawn)
    {
        //Clamp the value to ensure it doesn't over
        enemiesToSpawn = Mathf.Clamp(enemiesToSpawn, 0, spawnPoints.Count);

        //Loop through the input iterations
        for (int spawn = 0; spawn < enemiesToSpawn; spawn++)
        {
            //Select a random spawnpoint
            int selectedSpawn = Random.Range(0, spawnPoints.Count);

            //Spawn the enemy at the selected spawn point [TODO, write code to summon different enemies based on wave]


            //Alter the lists, removing it from the spawn points and putting them in the used
            usedSpawnpoints.Add(spawnPoints[selectedSpawn]);

            spawnPoints.RemoveAt(selectedSpawn);
        }
    }

    /// <summary>
    /// Reset the spawners for next round
    /// </summary>
    public void ResetSpawners()
    {
        //Get the amount the iteration will loop
        int resets = usedSpawnpoints.Count;

        //Loop through all the used spawn points and place them back in the pool
        for (int spawnReset = 0; spawnReset < resets; spawnReset++)
        {
            //Add it back to the pool
            spawnPoints.Add(usedSpawnpoints[0]);

            //Remove it from the recycled
            usedSpawnpoints.RemoveAt(0);
        }
    }

    /// <summary>
    /// Function to check if the current wave unlocks a new spawnable enemy
    /// </summary>
    /// <param name="currentWave"> Input of the current wave </param>
    public void ProgressWave(int currentWave)
    {
        //Check if the wave value corresponds to an unlock wave
        if (enemyAppearanceWaves.Contains(currentWave))
        {
            //Add the new enemy to the pool
            spawnableEnemies.Add(stanbyEnemies[0]);

            //Remove it from the standby pool
            stanbyEnemies.RemoveAt(0);
        }
    }
}
