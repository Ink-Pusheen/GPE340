using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Health))]
public class P_DeathDrop : MonoBehaviour
{
    [Header("Attributes")]

    [Tooltip("Owning Pawn of this component")]
    public Pawn ownerPawn;

    [Tooltip("Odds of 100% the weapon will drop")]
    [Range(0f, 1f)] public float weaponDropChance = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Health pawnHP = GetComponent<Health>();

        //Null check before subscribing
        if (pawnHP is not null) pawnHP.OnDeath.AddListener(DropGoods);

        ownerPawn = GetComponent<Pawn>();
    }

    /// <summary>
    /// Drop any weapon the player/enemy may be holding
    /// </summary>
    public void DropGoods()
    {
        if (ownerPawn is null) return; //Null check insurance

        //Create a temporary odds for comparison
        float odds = Random.Range(0f, 1f);

        //Drop the weapon if odds are hit
        if(odds <= weaponDropChance) ownerPawn.DropWeapon();

        //Else delete the weapon from existance and null the field
        else
        {
            Destroy(ownerPawn.heldWeapon.gameObject);
            ownerPawn.heldWeapon = null;
        }
    }
}
