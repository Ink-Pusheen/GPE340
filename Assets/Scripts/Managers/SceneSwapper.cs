using UnityEngine;

[System.Serializable]
public class GameScene
{
    [Tooltip("Name of the game object scene")]
    public string name;

    [Tooltip("Reference to the scene game object")]
    public GameObject sceneObject;
}

public class SceneSwapper : MonoBehaviour
{
    [Tooltip("Array of Scenes that are interchangeable")]
    public GameScene[] scenes;


    /// <summary>
    /// Enables the specified state via string
    /// </summary>
    /// <param name="sceneName"> Which scene to swap into </param>
    public void SwapScene(string sceneName)
    {
        //Loop through all the scenes, enabling the scene name that matches
        for (int scene = 0; scene < scenes.Length; scene++)
        {
            if (sceneName == scenes[scene].name) scenes[scene].sceneObject.SetActive(true);

            else continue;
        }
    }

    /// <summary>
    /// Enables the specified state via index
    /// </summary>
    /// <param name="sceneIndex"> Index of the scene that will display </param>
    public void SwapScene(int sceneIndex)
    {
        if (sceneIndex < 0 || sceneIndex >= scenes.Length)
        {
            Debug.LogError("Scene Index is out of range; Canceling operation.");
            return;
        }

        //Disable all scenes
        DisableAllScenes();

        //Enable the specified scene
        scenes[sceneIndex].sceneObject.SetActive(true);
    }

    /// <summary>
    /// Directly enables a specified scene via string
    /// </summary>
    /// <param name="sceneName"> String name of the scene being enabled </param>
    public void EnableScene(string sceneName)
    {
        //Loop through the scenes and enable the specifed match
        for (int scene = 0; scene < scenes.Length; scene++)
        {
            if (sceneName == scenes[scene].name) scenes[scene].sceneObject.SetActive(true);

            else continue;
        }
    }

    /// <summary>
    /// Directly enables a specified scene via integer
    /// </summary>
    /// <param name="sceneIndex"> Index of the scene being enabled </param>
    public void EnableScene(int sceneIndex)
    {
        //Enable the specified index
        scenes[sceneIndex].sceneObject.SetActive(true);
    }

    /// <summary>
    /// Directly disables a specified scene via string
    /// </summary>
    /// <param name="sceneName"> String name of the scene being disables </param>
    public void DisableScene(string sceneName)
    {
        //Loop through the scenes and enable the specifed match
        for (int scene = 0; scene < scenes.Length; scene++)
        {
            if (sceneName == scenes[scene].name) scenes[scene].sceneObject.SetActive(false);

            else continue;
        }
    }

    /// <summary>
    /// Directly disables a specified scene via integer
    /// </summary>
    /// <param name="sceneIndex"> Index of the scene being disables </param>
    public void DisableScene(int sceneIndex)
    {
        //Enable the specified index
        scenes[sceneIndex].sceneObject.SetActive(false);
    }


    /// <summary>
    /// Disables all the states within the array
    /// </summary>
    private void DisableAllScenes()
    {
        //Loop through and disable all scenes
        for (int scene = 0; scene < scenes.Length; scene++)
        {
            scenes[scene].sceneObject.SetActive(false);
        }
    }
}
