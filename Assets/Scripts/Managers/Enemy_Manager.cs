using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Manager : MonoBehaviour
{
    public static Enemy_Manager instance;

    [Header("Containters")]

    [Tooltip("Array of spawnable enemeis")]
    public GameObject[] enemies;


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

    // Spawning functionality

    public void SpawnEnemy()
    {
        //Select a random enemy
    }

    public void SpawnEnemy(GameObject enemyToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, EnemyData data)
    {
        //Spawn the prefab
        GameObject spawnedEnemy = Instantiate(enemyToSpawn, spawnPosition, spawnRotation);

        //Grab the pawn and weapon components
        Pawn enemyPawn = spawnedEnemy.GetComponent<Pawn>();

        //If data isn't null, assign the attributes
        if (data is not null)
        {
            Health enemyHP = spawnedEnemy.GetComponent<Health>();

            enemyHP.SetMaxHealth(data.maxHealth);
        }

        //Null check for the pawn
        if (enemyPawn is not null)
        {
            //Set the speed(s)
            enemyPawn.moveSpeed = data.moveSpeed;
            enemyPawn.rotateSpeed = data.rotationSpeed;
        }

        //Equip the weapon that is passed in
        if (data.equippedWeapon is not null)
        {
            GameObject spawnedWeapon = Instantiate(data.equippedWeapon.gameObject);

            Weapon newWeapon = spawnedWeapon.GetComponent<Weapon>();

            //Spawn the weapon into the enemies hands
            enemyPawn.SpawnWeapon(newWeapon);
        }

        //Create the controller for this AI
        AIController aiController = enemyPawn.AddComponent<AIController>();

        aiController.PossessPawn(enemyPawn);

        //Set the player target
        PlayerController player = FindFirstObjectByType<PlayerController>();

        if (player is not null && player.ownedPawn is not null) aiController.Target = player.ownedPawn.gameObject;

        else throw new System.ArgumentException("Player doesn't exist");
    }
}

public class EnemyData
{
    public float health;
    public float maxHealth;

    public Weapon equippedWeapon;

    public float moveSpeed;
    public float rotationSpeed;
}