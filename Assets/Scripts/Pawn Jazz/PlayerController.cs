using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Controller
{
    [Header("Input System")]

    //Mapping
    private PlayerInput pInput;

    //Actions
    private InputAction horizontal;
    private InputAction vertical;

    private void Awake()
    {
        pInput = GetComponent<PlayerInput>(); //Attempt to grab the component

        if(pInput is not null) //Null check before grabbing the actions
        {
            horizontal = pInput.actions.FindAction("Horizontal");
            vertical = pInput.actions.FindAction("Vertical");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Decisions();
    }

    public override void Decisions()
    {
        float horizontalInput = horizontal.ReadValue<float>();
        float verticalInput = vertical.ReadValue<float>();

        Vector3 moveVector = new Vector3(horizontalInput, 0, verticalInput);

        ownedPawn.Move(moveVector);
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
        throw new System.NotImplementedException();
    }
}
