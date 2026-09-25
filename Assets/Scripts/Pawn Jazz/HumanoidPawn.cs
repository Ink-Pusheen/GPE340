using UnityEngine;

public class HumanoidPawn : Pawn
{
    public Animator anim;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        selfPawn = this as HumanoidPawn;

        //Grab components
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 500, Color.red);
    }

    public override void Move(Vector3 direction)
    {
        Vector3 normalizedDirection = Vector3.ClampMagnitude(direction.normalized, 1); //Normalize the direction

        normalizedDirection *= moveSpeed; //Multiply the movespeed

        normalizedDirection = transform.InverseTransformDirection(normalizedDirection);

        //Set movement parameters based on vector input for animation purposes

        anim.SetFloat("XInput", normalizedDirection.x);
        anim.SetFloat("ZInput", normalizedDirection.z);
    }

    public override void Possess(Controller newController)
    {
        owningController = newController;
    }

    public override void UnPossess()
    {
        owningController = null;
    }

    public override void RotateTowards(Vector3 rotateDirection)
    {
        Vector3 vectorToPoint = rotateDirection - transform.position;
        //Debug.Log(vectorToPoint); //Testing
        Quaternion lookRotation = Quaternion.LookRotation(vectorToPoint, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
    }


    //This is run after the animation runs
    public void OnAnimatorMove()
    {
        //Set the root position and rotation
        transform.position = anim.rootPosition;
        transform.rotation = anim.rootRotation;

        //Grab the navmesh agent
        AIController AIcontroller = owningController as AIController; //This creats an explicit cast looking for a sub type of child script

        if (AIcontroller is not null)
        {
            //Set the nav mesh agent updated position respective to the animator
            AIcontroller.navAgent.nextPosition = anim.rootPosition;
        }
    }

    //Set animator booleans from the controller
    public void SetAnimatorBoolean(string Parameter, bool Value)
    {
        anim.SetBool(Parameter, Value);
    }

    //Set animator triggers from the controller
    public void SetTrigger(string Parameter)
    {
        anim.SetTrigger(Parameter);
    }
}
