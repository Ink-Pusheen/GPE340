using UnityEngine;

public class WA_LaserRifle : WeaponAction
{
    [Tooltip("Distance of fire")]
    public float fireDistance;

    [Tooltip("Damage this weapon will deal")]
    public float damageToDeal;

    [Tooltip("Prefab with a Line Renderer to Spawn in World Space")]
    public GameObject laserPrefab;

    public Lazer beeeeeam; //Laser attributes

    [Header("Accuracy")]

    [Tooltip("Current accuracy of the weapon"), Range(0f, 1f)]
    public float accuracy;

    [Tooltip("How far off the fire will be based on the accuracy angle wise"), Range(0f, 45f)]
    public float maxAccuracyDelta;


    public void Shoot()
    {
        //Raycast hit data
        RaycastHit hit;

        //Instantiate the laser beam and grab the component
        GameObject spawnedFire = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
        LaserBeam beam = spawnedFire.GetComponent<LaserBeam>();

        //Find the angle of fire based on the accuracy system
        Vector3 newFireDirection = firePoint.forward;

        float accuracyPercent = Random.Range(0f, 1 - accuracy);
        float accuracyRotationDegrees = Mathf.Lerp(-maxAccuracyDelta, maxAccuracyDelta, accuracyPercent);

        Quaternion accuracyDelta = Quaternion.Euler(0, accuracyRotationDegrees, 0);

        //Multiply for the inaccuracy, order matters on how it is multiplied
        newFireDirection = accuracyDelta * newFireDirection;

        //Set the laser attributes
        beam.color = beeeeeam.color;
        beam.width = beeeeeam.width;
        beam.startPoint = firePoint.position;
        beam.endPoint = firePoint.position + (newFireDirection * fireDistance);
        beam.lifespan = beeeeeam.lifespawn;

        

        //Check for raycase collision
        if (Physics.Raycast(firePoint.position, newFireDirection, out hit, fireDistance))
        {
            //Check for the hit Health component
            Health objectHealth = hit.collider.GetComponent<Health>();
            if (objectHealth is not null) //Null check
            {
                //Cause damage accordingly
                objectHealth.TakeDamage(damageToDeal); //Default 5, change later
            }

            //Set the new endpoint of the laser
            beam.endPoint = hit.point;
        }
    }

    [System.Serializable]
    public class Lazer
    {
        [Tooltip("Start Point of the Line")]
        public Vector3 startPoint;

        [Tooltip("End Point of the Line")]
        public Vector3 endPoint;

        [Tooltip("Color of the created Line")]
        public Color color = Color.rebeccaPurple;

        [Tooltip("How long this line will display")]
        public float lifespawn = 0.1f;

        [Tooltip("How wide the laser line is")]
        public float width = 0.5f;

        private LineRenderer render; //Line renderer component
    }
}
