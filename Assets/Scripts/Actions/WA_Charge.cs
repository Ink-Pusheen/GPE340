using UnityEngine;

public class WA_Charge : WeaponAction
{
    [Header("Attributes")]

    private bool bCharging; //Is this weapon currently charging? [Default False]

    [Tooltip("Current charge from when the button was held")]
    public float currentCharge;

    [Tooltip("Maximum charge this weapon can endow")]
    public float maxCharge;

    [Tooltip("The upper bounds of force based on the current / maximum charge")]
    public float maxForceApplied;

    [Tooltip("The upper bounds of damage this weapon can deal")]
    public float maxDamage;

    [Tooltip("Bullet to Spawn")]
    public GameObject bullet;

    // Update is called once per frame
    void Update()
    {
        //If the weapon is currently being charged
        if (bCharging)
        {
            if (currentCharge != maxCharge) //Ensure this cannot charge more than the limit
            {
                currentCharge += Time.deltaTime; //Add by delta time

                if (currentCharge >= maxCharge) currentCharge = maxCharge; //Default the value to the cap
            }
        }
    }

    // Change the charge state

    public void StartCharging()
    {
        bCharging = true;
    }

    public void StopCharging()
    {
        bCharging = false;

        //Reset the charge
        currentCharge = 0;
    }


    // Firing

    public void Shoot()
    {
        //Instantiate the Bullet
        GameObject spawnedBullet = Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation);

        //Set the bullet attributes

        float totalCharge = currentCharge / maxCharge; //Calclate the % charge

        float totalDamage = maxDamage * totalCharge; //Calculate the damage

        //TODO: Assign the damage

        //Add force to the bullet

        float forceToApply = maxForceApplied * totalCharge; //Multiply the charge to get how far this will go

        Rigidbody rb = spawnedBullet.GetComponent<Rigidbody>(); //Grab the rigidbody

        if (rb is not null) rb.AddForce(spawnedBullet.transform.forward * forceToApply, ForceMode.Impulse); //If rigidbody is detected, launch the bullet

        //Indicate to Stop Charging
        StopCharging();
    }
}
