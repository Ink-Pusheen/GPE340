using UnityEngine;

public class Button_Manager : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        if (Game_Manager.instance != null)
        {
            Game_Manager.instance.ResetToMainMenu();
        }
        else
        {
            Debug.LogError("Game_Manager does not exist");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
