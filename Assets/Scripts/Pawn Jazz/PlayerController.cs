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

    private InputAction leftJoystickMovement;
    private InputAction rightJoystickRotation;

    private InputAction crouch;
    private bool bCrouching; //Is the player currently crouching?

    private InputAction dance;
    private bool bDancing; //Is the player currently dancing?

    [Header("Camera")]

    private Camera mainCam;

    [SerializeField] bool bUsingController; //Is input being read by a controller for aim commands?

    private void Awake()
    {
        pInput = GetComponent<PlayerInput>(); //Attempt to grab the component

        if(pInput is not null) //Null check before grabbing the actions
        {
            horizontal = pInput.actions.FindAction("Horizontal");
            vertical = pInput.actions.FindAction("Vertical");

            leftJoystickMovement = pInput.actions.FindAction("LeftJoystick");
            rightJoystickRotation = pInput.actions.FindAction("RightJoystick");

            crouch = pInput.actions.FindAction("Crouch");

            dance = pInput.actions.FindAction("Dance");
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
        if (!bUsingController) //Not using Controller
        {
            float horizontalInput = horizontal.ReadValue<float>();
            float verticalInput = vertical.ReadValue<float>();

            Vector3 moveVector = new Vector3(horizontalInput, 0, verticalInput);

            if (!bDancing) ownedPawn.Move(moveVector);

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
        }
        else //Using controller
        {
            Vector2 Input = leftJoystickMovement.ReadValue<Vector2>();
            float verticalInput = vertical.ReadValue<float>();

            Vector3 moveVector = new Vector3(Input.x, 0, Input.y);

            if (!bDancing) ownedPawn.Move(moveVector);

            Vector2 newRotation = rightJoystickRotation.ReadValue<Vector2>(); //Read the controller input

            if (newRotation.x is not 0 || newRotation.y is not 0) //Check that this is actual input before proceeding
            {
                Vector3 rotateTowards = new Vector3(ownedPawn.transform.position.x + newRotation.x, ownedPawn.transform.position.y, ownedPawn.transform.position.z + newRotation.y); //Set to a vector3

                ownedPawn.RotateTowards(rotateTowards); //Rotate towards the input
            }
        }

        //Toggle the crouched state
        if (crouch.WasPressedThisFrame())
        {
            bCrouching = !bCrouching; //Flip the value

            HumanoidPawn pawn = ownedPawn as HumanoidPawn; //Cast to player

            if (pawn is not null) //Null check
            {
                pawn.SetAnimatorBoolean("Crouching", bCrouching); //Set the new value
            }
            else Debug.Log("Missing Pawn"); //Testing
        }

        //Toggle the dance state
        if (dance.WasPressedThisFrame())
        {
            bDancing = !bDancing; //Flip the value

            HumanoidPawn pawn = ownedPawn as HumanoidPawn; //Cast to player

            if (pawn is not null) //Null check
            {
                if (bDancing) pawn.SetTrigger("StartDance"); //Set the trigger only when entering the dance state

                pawn.SetAnimatorBoolean("Dance", bDancing); //Set the new value
            }
            else Debug.Log("Missing Pawn"); //Testing
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
