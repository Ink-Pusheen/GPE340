using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawning Attributes")]

    [Tooltip("List of active points enemies can spawn at")]
    public List<Transform> enemySpawnPoints = new();

    [Tooltip("Private list for spawn points that have been used / recycled till next iteration")]
    private List<Transform> usedSpawnpoints = new();

    [Tooltip("List of positions the player can spawn at")]
    public List<Transform> playerSpawnPoints = new();

    [Header("Player Spawnables")]

    [Tooltip("Basic Player Pawn Prefab")]
    public GameObject playerPawnPrefab;

    [Tooltip("Player Controller Prefab")]
    public GameObject playerControllerPrefab;


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
        enemiesToSpawn = Mathf.Clamp(enemiesToSpawn, 0, enemySpawnPoints.Count);

        //Loop through the input iterations
        for (int spawn = 0; spawn < enemiesToSpawn; spawn++)
        {
            //Select a random spawnpoint
            int selectedSpawn = Random.Range(0, enemySpawnPoints.Count);

            //Spawn the enemy at the selected spawn point [TODO, write code to summon different enemies based on wave]


            //Alter the lists, removing it from the spawn points and putting them in the used
            usedSpawnpoints.Add(enemySpawnPoints[selectedSpawn]);

            enemySpawnPoints.RemoveAt(selectedSpawn);
        }
    }

    /// <summary>
    /// Spawn the player at a given spawn point with the controller
    /// </summary>
    public void SpawnPlayer()
    {
        //Create the pawn and controller
        GameObject PlayerPawn = Instantiate(playerPawnPrefab);

        GameObject PlayerController = Instantiate(playerControllerPrefab);

        //Grab the components
        Pawn playerPawnScript = PlayerPawn.GetComponent<Pawn>();

        Controller playerControllerScript = PlayerController.GetComponent<Controller>();

        //Have the Controller possess the spawned pawn
        playerControllerScript.PossessPawn(playerPawnScript);

        playerPawnScript.Possess(playerControllerScript);

        //Assign the player controller prefab to the game manager
        Game_Manager.instance.player = PlayerController;
    }

    /// <summary>
    /// Spawn the player with only the pawn
    /// </summary>
    /// <param name="inPlayerPawn"> The controller to assign the spawned pawn to </param>
    public void SpawnPlayer(Controller inPlayerController)
    {
        //Spawn the pawn
        GameObject PlayerPawn = Instantiate(playerPawnPrefab);

        //Grab the component
        Pawn PlayerPawnScript = PlayerPawn.GetComponent<Pawn>();

        //Re-possess to the new pawn
        inPlayerController.PossessPawn(PlayerPawnScript);

        PlayerPawnScript.Possess(inPlayerController);
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
            enemySpawnPoints.Add(usedSpawnpoints[0]);

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
