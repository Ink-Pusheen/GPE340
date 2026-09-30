using UnityEngine;
using UnityEngine.AI;

public class AIController : Controller
{
    [Header("Navmesh")]

    [Tooltip("Navigation Agent for the currently possesed AI Humanoid Pawn")]
    public NavMeshAgent navAgent; //Agent for this AI for navigation

    [Tooltip("The target in which this AI will set as it's destination")]
    [SerializeField] GameObject Target; //Target in which the AI will head towards


    [Header("Attributes")]

    [Tooltip("Range at which the Nav Agent will stop from its Target"), Range(1f, 20f)]
    [SerializeField] float stopDistance; //How far the agent stops from the target

    private Vector3 desiredVelocity = Vector3.zero; //Real time velocity that the movement will consider when conducting root motion

    /// <summary>
    /// Update function for the AI to make decisions during real time
    /// </summary>
    public override void Decisions()
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
}
