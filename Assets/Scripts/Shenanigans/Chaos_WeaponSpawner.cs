using UnityEngine;

public class Chaos_WeaponSpawner : MonoBehaviour
{
    [Header("Attributes")]

    [Tooltip("Should Spawn Items")]
    public bool bSpawnerActive;

    [Tooltip("Spawner active time")]
    public float activeTimeLength;

    [SerializeField] private bool bReset; //Reset functionality
    [SerializeField] private float lastActiveTime; //Last time this was activated

    [Tooltip("Array of items to randomly select from")]
    public GameObject[] spawnables;

    [Tooltip("Spawn Point")]
    public Transform itemSpawnPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > activeTimeLength + lastActiveTime)
        {
            bSpawnerActive = false;
        }

        if (bSpawnerActive)
        {
            if (spawnables.Length is 0) bSpawnerActive = false; //Force off

            //Get a random index
            int index = Random.Range(0, spawnables.Length);

            //Spawn the object at the given transform
            Instantiate(spawnables[index], itemSpawnPoint.position, Quaternion.identity);
        }

        if (bReset)
        {
            bReset = false;

            lastActiveTime = Time.time;

            bSpawnerActive = true;
        }
    }
}
