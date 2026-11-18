using UnityEngine;

public class Game_Starter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Start the game as long as the game manager exists
        if (Game_Manager.instance != null)
        {
            Invoke("ProxyStart", 0.05f);
        }
        else
        {
            Debug.LogError("Game Manager does not exist");
        }
    }

    private void ProxyStart()
    {
        Game_Manager.instance.StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
