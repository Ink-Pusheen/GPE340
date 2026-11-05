using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager instance;

    [Header("Other Managers")]

    [Tooltip("Reference to the Spawn Manager")]
    public SpawnManager spawnManager;


    [Tooltip("Reference to the player")]
    public GameObject player;

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

    }

    /// <summary>
    /// Called when the player runs completely out of lives
    /// </summary>
    public void GameOver()
    {

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
