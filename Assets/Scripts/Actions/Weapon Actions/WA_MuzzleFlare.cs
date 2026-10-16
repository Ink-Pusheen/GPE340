using UnityEngine;

[RequireComponent(typeof(WA_AmmoClip))]
public class WA_MuzzleFlare : MonoBehaviour
{
    [Header("Attributes")]

    [Tooltip("Where the effect will spawn")]
    public Transform firePoint;

    [Tooltip("Effects to spawn")]
    public GameObject particles;

    /// <summary>
    /// Spawn the flare whenever firing
    /// </summary>
    public void SpawnParticles()
    {
        //Spawn said particles
        GameObject spawnedParticles = Instantiate(particles, firePoint.position, firePoint.rotation);

        Destroy(spawnedParticles, 3); //Destroy the particles after 3 seconds
    }
}
