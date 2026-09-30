using UnityEngine;

public class HumanoidPawn : Pawn
{
    [Tooltip("Animator Component to enact Root Motion")]
    public Animator anim;

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
}
