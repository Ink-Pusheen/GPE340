using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager instance;


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
}
