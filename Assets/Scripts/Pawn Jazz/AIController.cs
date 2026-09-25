using UnityEngine;
using UnityEngine.AI;

public class AIController : Controller
{
    [Header("Navmesh")]

    public NavMeshAgent navAgent; //Agent for this AI for navigation

    [SerializeField] GameObject Target; //Target in which the AI will head towards


    [Header("Attributes")]

    [SerializeField] float stopDistance; //How far the agent stops from the target

    private Vector3 desiredVelocity = Vector3.zero; //??? I get it's what the ai is looking for... but what does this change? Oh, pawn movement

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
