using System;
using UnityEngine;

public class WA_SniperRifle : WeaponAction
{
    [Header("Attributes")]

    [Tooltip("Bullet this weapon will fire")]
    public GameObject bullet;

    [Tooltip("Force of bullet launch")]
    public float fireForce;

    [Tooltip("Damage this weapon's bullet will deal")]
    public float damageToDeal;


    [Header("Accuracy")]

    [Tooltip("Current accuracy of the weapon"), Range(0f, 1f)]
    public float accuracy;

    [Tooltip("How far off the fire will be based on the accuracy angle wise"), Range(0f, 45f)]
    public float maxAccuracyDelta;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Shoot functionality
    /// </summary>
    public void Shoot()
    {
        //Set up the firing accuracy
        Vector3 newFireDirection = firePoint.forward;

        float accuracyPercent = UnityEngine.Random.Range(0f, 1 - accuracy);
        float accuracyRotationDegree = Mathf.Lerp(-maxAccuracyDelta, maxAccuracyDelta, accuracyPercent);

        Quaternion accuracyDelta = Quaternion.Euler(0, accuracyRotationDegree, 0);

        newFireDirection = accuracyDelta * newFireDirection;

        //Instantiate the bullet corresponding the fire point
        GameObject spawnedBullet = Instantiate(bullet, firePoint.position + newFireDirection, Quaternion.identity);

        spawnedBullet.transform.forward = newFireDirection; //Set the rotation

        //Grab the bullet Component and assign the values
        Bullet bulletScript = spawnedBullet.GetComponent<Bullet>();

        //Null check
        if (bulletScript is not null)
        {
            //Assign values
            bulletScript.damageToDeal = this.damageToDeal;

            //Tell the bullet to apply force appropriately
            bulletScript.ApplyImpulseForce(fireForce);
        }
        else
        {
            //Throw exception
            throw new ArgumentException("Error, bullet script doesn't exist");
        }
    }
}
