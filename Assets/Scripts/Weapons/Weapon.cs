using UnityEngine;
using UnityEngine.Events;
public class Weapon : MonoBehaviour
{
    [Header("Inverse Kinematics")]

    public Transform leftHand; //Position for the left hand
    public Transform rightHand; //Position for the right hand

    [Header("Unity Events")]

    public UnityEvent OnEquip;
    public UnityEvent OnUnequip;
    public UnityEvent OnPrimaryFireStart;
    public UnityEvent OnPrimaryFireEnd;
    public UnityEvent OnSecondaryFireStart;
    public UnityEvent OnSecondaryFireEnd;


    public void Equip() { OnEquip.Invoke(); }

    public void UnEquip() { OnUnequip.Invoke(); }

    public void PrimaryFireStart() { OnPrimaryFireStart.Invoke(); }

    public void PrimaryFireEnd() { OnPrimaryFireEnd.Invoke(); }

    public void SecondaryFireStart() { OnSecondaryFireStart.Invoke(); }

    public void SecondaryFireEnd() { OnSecondaryFireEnd.Invoke(); }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
