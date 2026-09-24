using UnityEditor;
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

    [Header("Camera")]

    private Camera mainCam;

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
        mainCam = Camera.main;
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

        //T'do: Find point on foot plane that the mouse overlaps, rotate towards
        Plane footPlane;

        footPlane = new(Vector3.up, ownedPawn.transform.position);

        Vector2 mousePos = Mouse.current.position.value;

        Vector3 raySPTR = new Vector3(mousePos.x, mousePos.y, ownedPawn.transform.position.z);

        Ray mouseRay = new();
        mouseRay = mainCam.ScreenPointToRay(raySPTR);

        float hitDistance;
        if (footPlane.Raycast(mouseRay, out hitDistance))
        {
            //If we hit the plane
            Vector3 hitPosition = mouseRay.GetPoint(hitDistance); //Grab the point of contact
            //Debug.Log(hitPosition);

            //Rotate towards said position
            ownedPawn.RotateTowards(hitPosition);


            //Debug.Log($"Hit plane at {hitPosition}"); //Testing
        }
        else
        {
            //Hit no plane
            //Perhaps doing nothing will be best
        }

        //Debug.Log(footPlane);
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
}
