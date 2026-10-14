using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Attributes")]

    [SerializeField] private float currentHealth; //Current health of the pawn
    [SerializeField] private float maxHealth; //Max health of the pawn

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
    }
}
