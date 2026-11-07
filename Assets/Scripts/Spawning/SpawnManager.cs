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

    [Tooltip("Basic Blaster for the player to spawn with")]
    public GameObject playerSpawnBlaster;

    [Header("Enemy Spawns")]

    [Tooltip("Controller for the AI")]
    public GameObject aiControllerPrefab;

    [Tooltip("Time between enemy spawns")]
    public float enemySpawnTimer;

    [Tooltip("Last enemy spawn time")]
    public float enemySpawnDelay;

    [Tooltip("Should enemies be spawning?")]
    [SerializeField] private bool bRunSpawner;


    [Header("Wave Data")]

    [Tooltip("Array of the waves of this game")]
    public WavaData[] waves;

    [Tooltip("Current wave the player is on")]
    public int currentWave;


    [Header("Current Wave Information")]

    [Tooltip("Remaining enemies to spawn")]
    public int roundEnemies;

    [Tooltip("List of active enemy pawn units")]
    public List<GameObject> activeEnemyPawns = new();

    [Tooltip("List of active enemy controller units")]
    public List<GameObject> activeEnemyControllers = new();


    /// <summary>
    /// Select random spawners to spawn enemies at for the round
    /// </summary>
    public void SpawnEnemy()
    {
        //Decrement wave enemies
        roundEnemies--;

        //Select a random spawnpoint
        int selectedSpawn = Random.Range(0, enemySpawnPoints.Count);

        //Select a random enemy off index
        int enemyToSpawn = Random.Range(0, waves[currentWave].spawnableEnemies.Count);

        //Spawn the enemy at the selected spawn point [TODO, write code to summon different enemies based on wave]
        GameObject SpawnedEnemy = Instantiate(waves[currentWave].spawnableEnemies[enemyToSpawn], enemySpawnPoints[selectedSpawn].transform.position, enemySpawnPoints[selectedSpawn].transform.rotation);
        GameObject SpawnedAIController = Instantiate(aiControllerPrefab, Vector3.zero, Quaternion.identity);

        //Set up the possession of the two spawned components
        HumanoidPawn aiPawn = SpawnedEnemy.GetComponent<HumanoidPawn>();
        AIController aiController = SpawnedAIController.GetComponent<AIController>();

        if (aiPawn is null || aiController is null) Debug.LogError("Pawn or Controller is null");

        aiController.PossessPawn(aiPawn);
        aiPawn.Possess(aiController);

        //Select a random weapon and give it to the ai
        int selectedWeapon = Random.Range(0, waves[currentWave].enemyWeapons.Count);

        GameObject SpawnedWeapon = Instantiate(waves[currentWave].enemyWeapons[selectedWeapon]);

        Weapon weaponSpawn = SpawnedWeapon.GetComponent<Weapon>();

        aiPawn.GiveWeapon(weaponSpawn); //Give the weapon to the ai

        //Add the enemy pieces to their respective lists
        activeEnemyControllers.Add(SpawnedAIController);
        activeEnemyPawns.Add(SpawnedEnemy);

        //Alter the lists, removing it from the spawn points and putting them in the used
        //usedSpawnpoints.Add(enemySpawnPoints[selectedSpawn]);

        //enemySpawnPoints.RemoveAt(selectedSpawn);
    }

    /// <summary>
    /// When an enemy is defeated, remove them from these lists
    /// </summary>
    /// <param name="aiPawn"> The pawn of the ai removed </param>
    /// <param name="aiController"> The controller of the ai removed </param>
    public void ClearEnemyFromList(GameObject aiPawn, GameObject aiController)
    {
        //Clear the identified items from the lists
        activeEnemyControllers.Remove(aiController);
        activeEnemyPawns.Remove(aiPawn);

        //Run a check for progression
        CheckWaveProgress();
    }

    /// <summary>
    /// Spawn the player at a given spawn point with the controller
    /// </summary>
    public void SpawnPlayer()
    {
        //Select a position for the player to spawn
        Transform spawnlocation = playerSpawnPoints[Random.Range(0, playerSpawnPoints.Count)];

        //Create the pawn and controller
        GameObject PlayerPawn = Instantiate(playerPawnPrefab, spawnlocation.position, spawnlocation.rotation);

        GameObject PlayerController = Instantiate(playerControllerPrefab);

        //Grab the components
        Pawn playerPawnScript = PlayerPawn.GetComponent<Pawn>();

        Controller playerControllerScript = PlayerController.GetComponent<Controller>();

        //Have the Controller possess the spawned pawn
        playerControllerScript.PossessPawn(playerPawnScript);

        playerPawnScript.Possess(playerControllerScript);

        //Create the weapon and give it to the player
        Weapon spawnedBlaster = Instantiate(playerSpawnBlaster).GetComponent<Weapon>();

        playerPawnScript.GiveWeapon(spawnedBlaster);

        //Assign the player controller prefab to the game manager
        Game_Manager.instance.player = PlayerController;
    }

    /// <summary>
    /// Spawn the player with only the pawn
    /// </summary>
    /// <param name="inPlayerPawn"> The controller to assign the spawned pawn to </param>
    public void SpawnPlayer(Controller inPlayerController)
    {
        //Select a position for the player to spawn
        Transform spawnlocation = playerSpawnPoints[Random.Range(0, playerSpawnPoints.Count)];

        //Spawn the pawn
        GameObject PlayerPawn = Instantiate(playerPawnPrefab, spawnlocation.transform.position, spawnlocation.transform.rotation);

        //Grab the component
        Pawn PlayerPawnScript = PlayerPawn.GetComponent<Pawn>();

        //Re-possess to the new pawn
        inPlayerController.PossessPawn(PlayerPawnScript);

        PlayerPawnScript.Possess(inPlayerController);

        //Create the weapon and give it to the player
        Weapon spawnedBlaster = Instantiate(playerSpawnBlaster).GetComponent<Weapon>();

        PlayerPawnScript.GiveWeapon(spawnedBlaster);
        //Debug.Log("Player spawned with controller input"); //Dupe bug moment
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


    public void CheckWaveProgress()
    {
        //Check if there is no more spawns and if the enemy list is clear
        if (roundEnemies is 0 && activeEnemyPawns.Count is 0)
        {
            //Progress the wave if so
            ProgressWave();
        }
    }

    /// <summary>
    /// Function to check if the current wave unlocks a new spawnable enemy
    /// </summary>
    public void ProgressWave()
    {
        //Increment the current wave
        currentWave++;

        if (currentWave >= waves.Count())
        {
            Debug.Log("Game won, swap to win scene");

            //Disable the player input
            Game_Manager.instance.player.GetComponent<PlayerController>().DisableInput();

            //Display the win scene
            SceneSwapper.instance.EnableScene("WinScreen");
        }

        else
        {
            Debug.Log("Progress to the next wave");

            //Start the next wave
            StartWave();
        }
    }


    public void StartWave()
    {
        Debug.Log($"Wave {currentWave + 1}");

        //Set the amount of enemies that will be spawning
        roundEnemies = waves[currentWave].spawnCount;

        //Set the timer to spawn an enemy near immediately
        enemySpawnTimer = enemySpawnDelay - 1f;

        //Enable the spawner
        bRunSpawner = true;
    }

    private void Update()
    {
        //Ensure the game isn't paused
        if (Game_Manager.instance.bPaused) return;

        if (bRunSpawner)
        {
            enemySpawnTimer += Time.deltaTime; //Over time increment, so when the game pauses, this does too

            if (enemySpawnTimer >= enemySpawnDelay)
            {
                //Reset the spawn timer
                enemySpawnTimer = 0;

                //Spawn an enemy
                SpawnEnemy();

                //Check if there are any enemies remaining
                if (roundEnemies is 0)
                {
                    //Stop the spawner
                    bRunSpawner = false;
                }
            }
        }
    }

    /// <summary>
    /// Completely stop the spawner via outside influence
    /// </summary>
    public void StopSpawner()
    {
        bRunSpawner = false;
    }

    /// <summary>
    /// Clean up any lingering controllers and AI in the scene
    /// </summary>
    public void SpawnCleanup()
    {
        //Loop through and clean up any remainders
        foreach (GameObject aiPawn in activeEnemyPawns)
        {
            Destroy(aiPawn);
        }

        foreach (GameObject aiController in activeEnemyControllers)
        {
            Destroy(aiController);
        }

        //Clear the lists
        activeEnemyPawns.Clear();
        activeEnemyControllers.Clear();

        //TODO: Do the same thing with just.. weapons.. that are lying around..

        //If the player is still active, delete
        if (Game_Manager.instance.player != null)
        {
            //Grab the controller to access the pawn
            PlayerController pController = Game_Manager.instance.player.GetComponent<PlayerController>();

            //Delete the two
            Destroy(pController.ownedPawn.gameObject);
            Destroy(pController.gameObject);

            Game_Manager.instance.player = null; //Set this back to null
        }
    }
}
