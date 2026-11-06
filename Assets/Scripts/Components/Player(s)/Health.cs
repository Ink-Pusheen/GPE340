using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Attributes")]

    [Tooltip("Current health of the pawn")]
    public float currentHealth;

    [Tooltip("Max health of the pawn")]
    public float maxHealth;

    [Header("Events")]

    public UnityEvent OnTakeDamage;
    public UnityEvent OnDeath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth; //Set max health
    }

    // Health Functions

    public void Heal(float healthHealed)
    {
        //Add the health to the current health
        currentHealth += healthHealed;

        //Clamp the health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void TakeDamage(float damageTaken)
    {
        //Remove the health
        currentHealth -= damageTaken;

        //Clamp the health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        //Run the on damage taken Unity Event
        OnTakeDamage.Invoke();

        if (currentHealth <= 0) Death(); //Run the death function if health has hit th threshold
    }

    public void Death()
    {
        Debug.Log($"{gameObject.name} has died...");

        //Run the on death Unity Event
        OnDeath.Invoke();

        //Check if this is on an AI pawn via navmesh existence
        NavMeshAgent nav = GetComponent<NavMeshAgent>();

        //If not null, remove this pawn and the controller from the spawn manager
        if (nav is not null)
        {
            GameObject pawnController = GetComponent<HumanoidPawn>().owningController.gameObject;

            Game_Manager.instance.spawnManager.ClearEnemyFromList(gameObject, pawnController);
        }
    }

    // Set attributes

    public void SetMaxHealth(float newMaxHealth)
    {
        maxHealth = newMaxHealth;
    }
}
