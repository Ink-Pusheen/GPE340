using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public enum SpawnType { Player, Enemy, Both };

    [Tooltip("Type of spawning")]
    public SpawnType spawnerType;


    [Header("Gizmos")]

    public float gizmoBoxHeight = 2f;
    public float gizmoBoxWidth = 1f;

    [SerializeField] private Color gizmoColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        switch (spawnerType)
        {
            case SpawnType.Player:

                gizmoColor = Color.blue;

                //Add this to the possible spawn points for the player
                Game_Manager.instance.spawnManager.playerSpawnPoints.Add(transform);

                break;

            case SpawnType.Enemy:

                gizmoColor = Color.red;

                //Add this to the possible spawn points for enemies
                Game_Manager.instance.spawnManager.enemySpawnPoints.Add(transform);

                break;

            case SpawnType.Both:

                gizmoColor = Color.green;

                //Add this spawner to both lists in the spawn manager
                Game_Manager.instance.spawnManager.playerSpawnPoints.Add(transform);
                Game_Manager.instance.spawnManager.enemySpawnPoints.Add(transform);

                break;
        }

        gizmoColor.a = 0.7f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        

        //Set the color of the gizmo
        Gizmos.color = gizmoColor;

        //Grab the vector of the box as it's 1/2 the width/height
        Vector3 boxPosition = transform.position;
        boxPosition += Vector3.up * (gizmoBoxHeight / 2);
        
        Vector3 boxSize = new Vector3(gizmoBoxWidth, gizmoBoxHeight, gizmoBoxWidth);

        Gizmos.DrawCube(boxPosition, boxSize);

        Gizmos.color = Color.pink;

        Gizmos.DrawRay(boxPosition, transform.forward * 2f);
    }
}
