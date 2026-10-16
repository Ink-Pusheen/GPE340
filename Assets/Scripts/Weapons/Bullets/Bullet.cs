using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [Header("Events")]

    public UnityEvent OnHit; //Hit something
    public UnityEvent OnDiminish; //Did not hit anything, lifetime expired


    [Header("Attributes")]

    private Rigidbody rb;

    [Tooltip("Damage this bullet will deal")]
    public float damageToDeal;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    
    // Force Application

    public void ApplyImpulseForce(float forceAmt)
    {
        rb.AddForce(transform.forward * forceAmt, ForceMode.Impulse);
    }


    private void OnCollisionEnter(Collision collision)
    {
        //Attempt to grab the health component
        Health hitHP = collision.gameObject.GetComponent<Health>();

        //Null check
        if (hitHP is not null)
        {
            hitHP.TakeDamage(damageToDeal);
        }

        //Invoke the On Hit
        OnHit.Invoke();

        //Destroy the bullet
        Destroy(gameObject);
    }
}
