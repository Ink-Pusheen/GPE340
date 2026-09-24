using UnityEngine;
using UnityEngine.AI;

public class AIController : Controller
{
    [Header("Navmesh")]

    [SerializeField] NavMeshAgent navAgent; //Agent for this AI for navigation

    [SerializeField] GameObject Target; //Target in which the AI will head towards

    public override void Decisions()
    {
        //throw new System.NotImplementedException();
    }

    public override void PossessPawn(Pawn inPawnPossession)
    {
        //Set code to unpossess previous pawn
        if (ownedPawn != null) ownedPawn.UnPossess();

        //Possess new pawn
        ownedPawn = inPawnPossession;

        //Tell the new pawn this is the new controller
        ownedPawn.Possess(this);
    }

    public override void UnPossessPawn()
    {
        ownedPawn.UnPossess(); //Tell the pawn to unpossess itself
        ownedPawn = null; //Null the controllers pawn
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navAgent.SetDestination(Target.transform.position); //Set the target position to be the next area the AI heads
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
