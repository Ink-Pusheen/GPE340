using UnityEngine;
using UnityEngine.AI;

public class AIController : Controller
{
    [Header("Navmesh")]

    [Tooltip("Navigation Agent for the currently possesed AI Humanoid Pawn")]
    public NavMeshAgent navAgent;

    [Tooltip("The target in which this AI will set as it's destination")]
    public GameObject Target;

    //Reference to the target index [-1 targetting nothing]
    [SerializeField] private int targetIndex = -1;

    [Tooltip("Points in which this AI will roam between")]
    public GameObject[] waypoints; //TODO, Implement waypoints

    [Header("Attributes")]

    [Tooltip("Range at which the Nav Agent will stop from its Target"), Range(1f, 20f)]
    public float stopDistance;

    [Tooltip("Allowed firing angle"), Range(5f, 45f)]
    public float allowedFireAngle;

    [Tooltip("Range at which the agent will attempt to fire")]
    public float fireRange;

    private Vector3 desiredVelocity = Vector3.zero; //Real time velocity that the movement will consider when conducting root motion

    //Is the AI currently firing?
    [SerializeField] private bool bFiring;


    /// <summary>
    /// Update function for the AI to make decisions during real time
    /// </summary>
    public override void Decisions()
    {
        //Ensure that input can be taken first
        if (!bAllowedInput) return;

        //If there is a target, seek it, move and rotate towards it
        if(Target != null) SeekTarget();

        //Else find the next target respectfully
        else
        {
            FindTarget();
        }

        if (targetIndex is 0) TryFiring(); //Attempt to fire if the player is identified as the target
    }

    /// <summary>
    /// Possess the parameter Pawn, Unpossessing the prior if one is Possessed
    /// </summary>
    /// <param name="inPawnPossession"></param>
    public override void PossessPawn(Pawn inPawnPossession)
    {
        //Set code to unpossess previous pawn
        if (ownedPawn != null) ownedPawn.UnPossess();

        //Possess new pawn
        ownedPawn = inPawnPossession;

        //Tell the new pawn this is the new controller
        ownedPawn.Possess(this);

        //Set Navmesh Properties
        navAgent.speed = ownedPawn.moveSpeed;
        navAgent.angularSpeed = ownedPawn.rotateSpeed;

        //Disable movement and rotation of the agent
        navAgent.updatePosition = false;
        navAgent.updateRotation = false;
    }

    /// <summary>
    /// Unpossess the current pawn
    /// Null the current nav agent
    /// </summary>
    public override void UnPossessPawn()
    {
        ownedPawn.UnPossess(); //Tell the pawn to unpossess itself
        ownedPawn = null; //Null the controllers pawn

        //Remove the agent
        navAgent = null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Temp for testing
        if (ownedPawn is not null) PossessPawn(ownedPawn);
    }

    // Update is called once per frame
    void Update()
    {
        Decisions();
    }

    /// <summary>
    /// Constantly move towards the target location via navmesh
    /// </summary>
    public void SeekTarget()
    {
        //Null check for the pawn
        if (ownedPawn == null) return;
        //Debug.Log("Pawn Exists, DO SOMETHING"); //Forgot to make Decisions() run... whoops... Debug testing
        //Set to seek the target
        navAgent.SetDestination(Target.transform.position); //Set the target position to be the next area the AI heads

        //Find the velocity the agent seeks for movement
        desiredVelocity = navAgent.desiredVelocity;
        //Debug.Log(desiredVelocity); //Test
        //Send to the move function for movement
        ownedPawn.Move(desiredVelocity.normalized);

        //Rotate towards the player
        ownedPawn.RotateTowards(Target.transform.position);
    }

    /// <summary>
    /// Find and assign the player as the target
    /// </summary>
    public void FindTarget()
    {
        //Speak to the game manager to grab the player reference
        GameObject player = Game_Manager.instance.player;

        //Set the target
        Target = player;

        //Check if it is the player
        if (Target.GetComponent<HumanoidPawn>().identity == 0)
        {
            this.targetIndex = 0; //Set the reference
        }
    }

    /// <summary>
    /// Check the conditionals if this AI can start firing
    /// </summary>
    public virtual void TryFiring()
    {
        //Check if the distance is close enough to fire
        if (Vector3.Distance(ownedPawn.transform.position, Target.transform.position) <= fireRange)
        {
            //If they are within the allowed firing angle, fire, though stop if they are out of the angle range
            if (inRotationRange() && !bFiring)
            {
                bFiring = true; //Indicate that the weapon is firing

                ownedPawn.heldWeapon.PrimaryFireStart();
            }
            else if(!inRotationRange() && bFiring)
            {
                bFiring = false; //Indicate thtat the weapon is no longer firing

                //If too far, stop shooting
                ownedPawn.heldWeapon.PrimaryFireEnd();
            }
        }
        else
        {
            if (!bFiring) return;

            //Stop firing if they go out of range
            ownedPawn.heldWeapon.PrimaryFireEnd();

            bFiring = false;
        }
    }

    /// <summary>
    /// Boolean return if the target is within the allowed rotation angle
    /// </summary>
    /// <returns></returns>
    private bool inRotationRange()
    {
        //Find the vector and angle which the target is from the pawn
        Vector3 targetVector = Target.transform.position - ownedPawn.transform.position;

        float targetAngle = Vector3.Angle(ownedPawn.transform.forward, targetVector);

        if (targetAngle <= allowedFireAngle) return true;

        else return false;
    }
}
