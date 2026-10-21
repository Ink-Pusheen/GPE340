using System.Collections.Generic;
using UnityEngine;

public class HumanoidPawn : Pawn
{
    [Tooltip("Animator Component to enact Root Motion")]
    public Animator anim;

    [Tooltip("Detection Radius for Interactables")]
    public float detectionRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        selfPawn = this as HumanoidPawn; //Set the self pawn variable to this script

        //Grab components
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 500, Color.red); //Debug Testing Rotation
    }

    // Movement

    /// <summary>
    /// Functionality to move the humanoid pawn within world space via pure up down left and right
    /// </summary>
    /// <param name="direction"></param>
    public override void Move(Vector3 direction)
    {
        Vector3 normalizedDirection = Vector3.ClampMagnitude(direction.normalized, 1); //Normalize the direction

        normalizedDirection *= moveSpeed; //Multiply the movespeed

        normalizedDirection = transform.InverseTransformDirection(normalizedDirection);

        //Set movement parameters based on vector input for animation purposes

        anim.SetFloat("XInput", normalizedDirection.x);
        anim.SetFloat("ZInput", normalizedDirection.z);
    }

    /// <summary>
    /// Given a direction, make this pawn rotate towards said direction over time
    /// </summary>
    /// <param name="rotateDirection"></param>
    public override void RotateTowards(Vector3 rotateDirection)
    {
        Vector3 vectorToPoint = rotateDirection - transform.position;
        //Debug.Log(vectorToPoint); //Testing
        Quaternion lookRotation = Quaternion.LookRotation(vectorToPoint, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
    }

    // Possession

    /// <summary>
    /// Set the new Controller which is possessing this Pawn
    /// </summary>
    /// <param name="newController"></param>
    public override void Possess(Controller newController)
    {
        owningController = newController;
    }

    /// <summary>
    /// Remove the Current Controller which was possessing this Pawn
    /// </summary>
    public override void UnPossess()
    {
        owningController = null;
    }

    // Animator Functionality


    /// <summary>
    /// Whenever the animator is utilized for root motion, conduct the following code
    /// To set the new position / rotation
    /// As well as navigation for AI agents
    /// </summary>
    //This is run after the animation runs
    public void OnAnimatorMove()
    {
        //Set the root position and rotation
        transform.position = anim.rootPosition;
        transform.rotation = anim.rootRotation;

        //Grab the navmesh agent
        AIController AIcontroller = owningController as AIController; //This creats an explicit cast looking for a sub type of child script

        if (AIcontroller is not null) //Null check for the AI
        {
            //Set the nav mesh agent updated position respective to the animator
            AIcontroller.navAgent.nextPosition = anim.rootPosition;
        }

        if (interactbleInRange != null) InteractionDetection(); //Check if the closest interactable changed; Remove if this lags the game
    }

    /// <summary>
    /// IK Functionality after the animation frame plays
    /// </summary>
    /// <param name="layerIndex"></param>
    public void OnAnimatorIK(int layerIndex)
    {
        //Nope, no weapon is held, follow the animation
        if (heldWeapon is null)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);

            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);
        }
        else //A weapon exists
        {
            //If a right hand point exists
            if (heldWeapon.rightHand != null)
            {
                //Set the IK weights to 1
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);

                //Set the position and rotation of the weapon data
                anim.SetIKPosition(AvatarIKGoal.RightHand, heldWeapon.rightHand.position);
                anim.SetIKRotation(AvatarIKGoal.RightHand, heldWeapon.rightHand.rotation);
            }
            else
            {
                //Follow what the animator says
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);
            }

            //If a left hand point exists
            if (heldWeapon.leftHand != null)
            {
                //Set the IK weights to 1
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);

                //Set the position and rotation of the weapon data
                anim.SetIKPosition(AvatarIKGoal.LeftHand, heldWeapon.leftHand.position);
                anim.SetIKRotation(AvatarIKGoal.LeftHand, heldWeapon.leftHand.rotation);
            }
            else
            {
                //Follow what the animator says
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);
            }
        }

        
    }

    /// <summary>
    /// Set an animator boolean via the parameter name
    /// </summary>
    /// <param name="Parameter"></param>
    /// <param name="Value"></param>
    //Set animator booleans from the controller
    public void SetAnimatorBoolean(string Parameter, bool Value)
    {
        anim.SetBool(Parameter, Value);
    }

    /// <summary>
    /// Initiate an animator trigger via the parameter name
    /// </summary>
    /// <param name="Parameter"></param>
    //Set animator triggers from the controller
    public void SetTrigger(string Parameter)
    {
        anim.SetTrigger(Parameter);
    }

    // Weapon detection and alteration functionality

    public void OnTriggerEnter(Collider other)
    {
        //Run the Interaction Detection
        InteractionDetection();
    }

    public void OnTriggerExit(Collider other)
    {
        //Run the Interaction Detection
        InteractionDetection();
    }

    /// <summary>
    /// Detect weapons nearby based on discovery distance and select the nearest one
    /// </summary>
    public void InteractionDetection()
    {
        //Grab colliders in range
        Collider[] hitInteractables = Physics.OverlapSphere(transform.position, detectionRange);

        //Create a new list
        List<GameObject> trueInteractables = new();

        //Filter out non interactable items
        for (int item = 0; item < hitInteractables.Length; item++)
        {
            //Try to grab the component and add it to the list if it exists
            Weapon_Interaction weaponFound = hitInteractables[item].GetComponent<Weapon_Interaction>();

            if (weaponFound is not null && weaponFound.bCanBeGrabbed) trueInteractables.Add(hitInteractables[item].gameObject);
        }

        //Check that there is at  least one detected weapon, stopping if none
        if (trueInteractables.Count is 0)
        {
            interactbleInRange = null; //Null the interactable in range

            return; //Go no further
        }

        float smallestDistance = 0; //A temporary float for distance calculation

        //Loop through the weapons and detect which one is the closes
        for (int weapon = 0; weapon < trueInteractables.Count; weapon++)
        {
            //Set the default and loop to the next
            if (weapon is 0)
            {
                interactbleInRange = trueInteractables[weapon];

                smallestDistance = Vector3.Distance(transform.position, trueInteractables[weapon].transform.position);

                continue;
            }

            //Grab the distance of the detected weapon
            float distance = Vector3.Distance(transform.position, trueInteractables[weapon].transform.position);

            //Compare if it is smaller to the prior
            if (distance <= smallestDistance)
            {
                //Set the new default and update the interactable in range
                smallestDistance = distance;

                interactbleInRange = trueInteractables[weapon];
            }
            else continue;
        }
    }

    public override void GrabWeapon()
    {
        //Check if the player currently has a weapon, dropping it first if so
        if (heldWeapon is not null)
        {
            //Call the on Unequip Unity Event When dropping
            heldWeapon.OnUnequip.Invoke();

            //Clear the parent
            heldWeapon.gameObject.transform.parent = null;
        }

        //Invoke the weapons on grab function
        interactbleInRange.GetComponent<Weapon>().OnEquip.Invoke();

        //Set the position, rotation, and parent the weapon
        interactbleInRange.transform.position = weaponHoldPosition.position;
        interactbleInRange.transform.rotation = weaponHoldPosition.rotation;
        interactbleInRange.transform.parent = weaponHoldPosition.gameObject.transform;

        //Set the new weapon reference
        heldWeapon = interactbleInRange.GetComponent<Weapon>();

        //Call for the Interaction Detection to compute the next closest weapon
        InteractionDetection();
    }

    public override void DropWeapon()
    {
        //Ensure the player has a weapon to drop
        if (heldWeapon is not null)
        {
            //Call the on Unequip Unity Event When dropping
            heldWeapon.OnUnequip.Invoke();

            //Clear the parent
            heldWeapon.gameObject.transform.parent = null;

            //Remove the weapon reference
            heldWeapon = null;

            //Call for the Interaction Detection to compute the next closest weapon
            InteractionDetection();
        }
    }

    public override void SpawnWeapon(Weapon weaponSpawned)
    {
        //Set the new weapon reference
        heldWeapon = weaponSpawned;

        //Set the weapon position, rotation, and parent
        heldWeapon.gameObject.transform.position = weaponHoldPosition.transform.position;
        heldWeapon.gameObject.transform.rotation = weaponHoldPosition.transform.rotation;
        heldWeapon.gameObject.transform.parent = weaponHoldPosition.gameObject.transform;

        //Invoke the on equipt for the weapon
        heldWeapon.OnEquip.Invoke();

        //Call for an interaction detection
        InteractionDetection();
    }
}
