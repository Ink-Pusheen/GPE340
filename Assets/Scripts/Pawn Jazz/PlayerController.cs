using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Controller
{
    [Header("Input System")]

    //Mapping PlayerInput Component

    private PlayerInput pInput;

    //Actions

    private InputAction horizontal; //Input for horizontal movement
    private InputAction vertical; //Input for vertical movement

    private InputAction leftJoystickMovement; //Input for joystick player movement
    private InputAction rightJoystickRotation; //Input for joystick player rotation

    private InputAction crouch; //Input to crouch and uncrouch
    private bool bCrouching; //Is the player currently crouching?

    private InputAction dance; //Input to dance and stop dancing
    private bool bDancing; //Is the player currently dancing?

    [Header("Camera")]

    private Camera mainCam; //Assignment of the main camera within the scene

    [SerializeField] bool bUsingController; //Is input being read by a controller for aim commands?

    private void Awake()
    {
        pInput = GetComponent<PlayerInput>(); //Attempt to grab the component

        if (pInput is not null) //Null check before grabbing the actions
        {
            //Assign the input actions via the names within the player input mapping

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
        mainCam = Camera.main; //Assign the main camera
    }

    // Update is called once per frame
    void Update()
    {
        Decisions(); //Conduct all input of the player during runtime
    }

    /// <summary>
    /// Conduction of all player input during runtime
    /// </summary>
    public override void Decisions()
    {
        if (!bUsingController) //Not using Controller
        {
            //Read the horizontal and vertical input from the keyboard
            float horizontalInput = horizontal.ReadValue<float>();
            float verticalInput = vertical.ReadValue<float>();

            //Create a new movement vector from said inputs
            Vector3 moveVector = new Vector3(horizontalInput, 0, verticalInput);

            if (!bDancing) ownedPawn.Move(moveVector); //If not in the dance state, move the player pawn

            Plane footPlane; //Create a temporary Plane

            footPlane = new(Vector3.up, ownedPawn.transform.position); //Set the plane position at the players feet

            Vector2 mousePos = Mouse.current.position.value; //Grab the Mouses current (x,y) position

            //Create a Vector3 from the mouse position and the player pawn z position
            Vector3 raySPTR = new Vector3(mousePos.x, mousePos.y, ownedPawn.transform.position.z);

            //Create a new ray and set its screen point to ray using the prior Vector3
            Ray mouseRay = new();
            mouseRay = mainCam.ScreenPointToRay(raySPTR);

            //Create an indefinite cast and cast the ray to see if where it hits the created plane
            float hitDistance;
            if (footPlane.Raycast(mouseRay, out hitDistance))
            {
                //If we hit the plane
                Vector3 hitPosition = mouseRay.GetPoint(hitDistance); //Grab the point of contact

                //Rotate towards said position
                ownedPawn.RotateTowards(hitPosition);
            }
            else
            {
                //Hit no plane
                //Perhaps doing nothing will be best
            }
        }
        else //Using controller
        {
            //Grab the Vector2 input from the left stick of the joystick
            Vector2 Input = leftJoystickMovement.ReadValue<Vector2>();

            //Create a Vector3 from the left stick input
            Vector3 moveVector = new Vector3(Input.x, 0, Input.y);

            if (!bDancing) ownedPawn.Move(moveVector); //Move the player via the Vector3 if not Dancing

            //Read the controller input for rotation
            Vector2 newRotation = rightJoystickRotation.ReadValue<Vector2>();

            //Check that there is actual input before proceeding to rotate the palyer
            if (newRotation.x is not 0 || newRotation.y is not 0) 
            {
                //Create a Vector3 based on the rotation input as well as the player pawns position
                Vector3 rotateTowards = new Vector3(ownedPawn.transform.position.x + newRotation.x, ownedPawn.transform.position.y, ownedPawn.transform.position.z + newRotation.y);

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
            else Debug.Log("Missing Pawn"); //Testing for Developers
        }

        //Toggle the dance state
        if (dance.WasPressedThisFrame())
        {
            bDancing = !bDancing; //Flip the value

            HumanoidPawn pawn = ownedPawn as HumanoidPawn; //Cast to player

            if (pawn is not null) //Null check
            {
                //Set the trigger only when entering the dance state
                if (bDancing) pawn.SetTrigger("StartDance"); 

                pawn.SetAnimatorBoolean("Dance", bDancing); //Set the new value
            }
            else Debug.Log("Missing Pawn"); //Testing for Developers
        }
    }

    /// <summary>
    /// Possess the parameter pawn
    /// Unposessing prior Pawn if there is one currently
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
    }

    /// <summary>
    /// Unpossess and null the current assigned Pawn
    /// </summary>
    public override void UnPossessPawn()
    {
        ownedPawn.UnPossess(); //Tell the pawn to unpossess itself
        ownedPawn = null; //Null the controllers pawn
    }
}
