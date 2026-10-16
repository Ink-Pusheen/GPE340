using UnityEngine;

public class Weapon_Interaction : MonoBehaviour
{
    [Tooltip("Interaction Availability")]
    public bool bCanBeGrabbed;


    /// <summary>
    /// Set the Grab state of this object
    /// </summary>
    /// <param name="bCanBeGrabbed"></param>
    public void SetGrabState(bool bCanBeGrabbed)
    {
        this.bCanBeGrabbed = bCanBeGrabbed;
    }
}
