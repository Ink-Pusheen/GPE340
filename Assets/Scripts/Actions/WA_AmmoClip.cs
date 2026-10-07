using UnityEngine;
using UnityEngine.Events;

public class WA_AmmoClip : WeaponAction
{

    [Header("Attributes")]

    [Tooltip("The current ammo in the clip")]
    [SerializeField] private int currentAmmo;

    [Tooltip("The amount of ammo used in one firing action")]
    [SerializeField] private int ammoPerFire;

    [Tooltip("Maximum allowed ammo at any given time in the clip")]
    [SerializeField] private int maxAmmo;

    [Header("Events")]

    public UnityEvent OnFireSuccess;
    public UnityEvent OnFireFailed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentAmmo = maxAmmo; //Set the cap of the initial clip
    }

    public void AttemptFire()
    {
        if (currentAmmo >= ammoPerFire)
        {
            UseAmmo(ammoPerFire); //Subtract used bullets

            OnFireSuccess.Invoke(); //Invoke the successful event
        }
        else
        {
            OnFireFailed.Invoke(); //Invoke the failed event
        }
    }

    //Subtract successful fired ammo
    private void UseAmmo(int usedAmmo)
    {
        currentAmmo -= usedAmmo;
    }
}
