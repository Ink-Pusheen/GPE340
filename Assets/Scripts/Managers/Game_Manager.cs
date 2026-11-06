using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager instance;

    [Header("Other Managers")]

    [Tooltip("Reference to the Spawn Manager")]
    public SpawnManager spawnManager;


    [Header("Gameplay Elements")]

    [Tooltip("Reference to the player")]
    public GameObject player;

    [Tooltip("Is the game active?")]
    public bool bPlaying;

    private void Awake()
    {
        instance = this; //Set the instance
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Starts the game, spawning the player and starting the waves
    /// </summary>
    public void StartGame()
    {
        //Set the playing flag
        bPlaying = true;

        //Swap to the main game
        SceneSwapper.instance.SwapScene("MainGame");

        //Call to spawn the player and start the game in the Spawner Manager
        spawnManager.SpawnPlayer();

        spawnManager.StartWave();
    }

    /// <summary>
    /// Called when the player runs completely out of lives
    /// </summary>
    public void GameOver()
    {
        //Display the game over screen
        SceneSwapper.instance.EnableScene("LoseScreen");

        spawnManager.currentWave = 0; //Reset the game's wave
    }


    public void ResetToMainMenu()
    {
        //Set the playing flag
        bPlaying = false;

        //Swap the scene
        SceneSwapper.instance.SwapScene("MainMenu");

        //Clean up any excess in the spawn manager
        spawnManager.SpawnCleanup();
    }

    /// <summary>
    /// Pauses the game completely
    /// </summary>
    public void PauseGame()
    {
        //Set the timescale to none
        Time.timeScale = 0.0f;
    }

    /// <summary>
    /// Unpauses the game completely
    /// </summary>
    public void UnPauseGame()
    {
        //Set the timescale back to normal
        Time.timeScale = 1.0f;
    }
}
