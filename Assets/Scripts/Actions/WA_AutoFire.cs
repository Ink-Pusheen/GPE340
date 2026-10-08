using UnityEngine;
using UnityEngine.Events;

public class WA_AutoFire : MonoBehaviour
{
    [Header("Attributes")]

    [Tooltip("Weapon Firing Interval")]
    [SerializeField] private float fireDelay;

    [Tooltip("Current Firing state of Weapon")]
    public bool bFiring;

    private float lastFireTime; //Time which this weapon was last fired?

    [Header("Events")]

    public UnityEvent OnFire;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastFireTime = Time.time - fireDelay; //Initial setup to allow immediate fire
    }

    // Update is called once per frame
    void Update()
    {
        //Check for firing the weapon
        if (bFiring && Time.time > lastFireTime + fireDelay)
        {
            //Reset the last fire time
            lastFireTime = Time.time;

            //Fire the event to the ammo check
            OnFire.Invoke();
        }
    }

    // Toggle the firing states
    
    public void EnableFiring()
    {
        bFiring = true;
    }

    public void DisableFiring()
    {
        bFiring = false;
    }

}
