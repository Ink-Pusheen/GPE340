using UnityEngine;

public class ExplosionBehavior : MonoBehaviour
{
    [Header("Attributes")]

    [SerializeField] float ExplosionForce; //How much force this object emits
    [SerializeField] float ExplosionDamage; //How much damage this deals

    [SerializeField] float ExplosionDistance; //How far does this explosion reach?

    [Header("Effects")]

    [SerializeField] GameObject ExplosionEffects; //Explosion particles
    [SerializeField] AudioClip ExplosionSound; //Sound of the explosion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Commit explosion behavior
    /// </summary>
    void Explode()
    {
        //Grab all objects around the colliders around the explosion
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, ExplosionDistance);

        //Deal damage to hit objects with health component, loop through each hit object
        foreach (Collider hit in hitObjects)
        {
            //Check for the health component
            Health hp = hit.GetComponent<Health>();

            if (hp is not null) //Null Check
            {
                //Apply the force towards the hit object based on their position if Rigidbody isn't null
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb is not null) //Null check
                {
                    Vector3 blastDirection = hit.transform.position - transform.position; //Get the direction the hit object would go

                    Vector3 normalizedBlast = blastDirection.normalized; //Normalize the blast direction

                    normalizedBlast *= ExplosionForce; //Multiply by the explosion force

                    rb.AddForce(normalizedBlast, ForceMode.Impulse); //Instant one burst of impulse
                }

                //Spawn the explosion particles and play the explosion sound
            }
        }

        
    }

}
