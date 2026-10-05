using UnityEngine;
using UnityEngine.Events;
public class Weapon : MonoBehaviour
{


    // Unity Events

    public UnityEvent OnEquip;
    public UnityEvent OnUnequip;
    public UnityEvent OnPrimaryFireStart;
    public UnityEvent OnPrimaryFirstEnd;
    public UnityEvent OnSecondaryFireStart;
    public UnityEvent OnSecondaryFireEnd;


    public void Equip() { OnEquip.Invoke(); }

    public void UnEquip() { OnUnequip.Invoke(); }

    public void PrimaryFireStart() { OnPrimaryFireStart.Invoke(); }

    public void PrimaryFireEnd() { OnPrimaryFirstEnd.Invoke(); }

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
