using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Attributes")]

    [SerializeField] float currentHealth; //Current health of the pawn
    [SerializeField] float maxHealth; //Max health of the pawn

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
    }

    public void Death()
    {
        Debug.Log($"{gameObject.name} has died...");

        //Run the on death Unity Event
        OnDeath.Invoke();
    }
}
