using UnityEngine;

[RequireComponent(typeof (Weapon))]
public abstract class WeaponAction : GameAction
{
    [Tooltip("Origin of Line of Fire")]
    public Transform firePoint;
}

