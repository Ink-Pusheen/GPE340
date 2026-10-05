using UnityEngine;

[RequireComponent(typeof (Weapon))]
public class WeaponAction : GameAction
{
    [Tooltip("Origin of Line of Fire")]
    public Transform firePoint;

    [Tooltip("Distance of fire")]
    public float fireDistance;

    private LineRenderer fireLine; //Draw a line when fired
    public Lazer beeeeeam;
    public GameObject laserPrefab;


    public void Shoot()
    {
        //Raycast hit data
        RaycastHit hit;

        //Instantiate the laser beam and grab the component
        GameObject spawnedFire = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
        LaserBeam beam = spawnedFire.GetComponent<LaserBeam>();

        //Set the laser attributes
        beam.color = beeeeeam.color;
        beam.width = beeeeeam.width;
        beam.startPoint = firePoint.position;
        beam.endPoint = firePoint.position + (firePoint.forward * fireDistance);
        beam.lifespan = beeeeeam.lifespawn;

        //Check for raycase collision
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, fireDistance))
        {
            //Check for the hit Health component
            Health objectHealth = hit.collider.GetComponent<Health>();
            if (objectHealth is not null) //Null check
            {
                //Cause damage accordingly
                objectHealth.TakeDamage(5); //Default 5, change later
            }

            //Set the new endpoint of the laser
            beam.endPoint = hit.point;
        }
    }

}

[System.Serializable]
public class Lazer
{
    public Vector3 startPoint;
    public Vector3 endPoint;
    public Color color = Color.rebeccaPurple;
    public float lifespawn = 0.1f;
    public float width = 0.5f;
    private LineRenderer render;
}