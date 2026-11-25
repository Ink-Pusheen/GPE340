using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Pawn_Ragdoll : MonoBehaviour
{
    private Collider pawnCollider;
    private List<Collider> ragdollColliders = new();

    private Rigidbody pawnRigidbody;
    private List<Rigidbody> ragdollRigidbodies = new();

    private Animator pawnAnimator;

    private bool bRagdolling;

    private void Awake()
    {
        //Grab all components
        pawnCollider = GetComponent<Collider>();
        pawnRigidbody = GetComponent<Rigidbody>();
        pawnAnimator = GetComponent<Animator>();

        //Grab child colliders and rigidbodies
        ragdollColliders = GetComponentsInChildren<Collider>().ToList<Collider>();
        ragdollColliders.Remove(pawnCollider);

        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>().ToList<Rigidbody>();
        ragdollRigidbodies.Remove(pawnRigidbody);

        //Default disabled
        DisableRagdoll();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }


    public void ToggleRagdoll()
    {
        //Flip the bRagdolling bool
        bRagdolling = !bRagdolling;

        if (bRagdolling) //Enable ragdoll
        {
            EnableRagdoll();
        }
        else //Disable ragdoll
        {
            DisableRagdoll();
        }
    }

    private void EnableRagdoll()
    {
        //Disable pawn components
        pawnCollider.enabled = false;
        pawnRigidbody.isKinematic = true;
        pawnAnimator.enabled = false;

        //Enable the listed colliders and rigidbodies of the ragdoll
        foreach (Collider col in ragdollColliders)
        {
            col.enabled = true;
        }

        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = false;
        }
    }

    private void DisableRagdoll()
    {
        //Enable pawn attributes
        pawnCollider.enabled = true;
        pawnRigidbody.isKinematic = false;
        pawnAnimator.enabled = true;

        //Disable listed colliders and rigidbodies of the ragdoll
        foreach (Collider col in ragdollColliders)
        {
            col.enabled = false;
        }

        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = true;
        }
    }
}
