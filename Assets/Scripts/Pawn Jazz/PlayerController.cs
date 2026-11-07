using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    private InputAction fire; //Input for firing a currently held weapon

    private InputAction grabWeapon; //Input for grabbing nearby weaponry
    private InputAction dropWeapon; //Input to drop held weaponry

    private InputAction pause; //Input to pause and unpause the game

    [Header("Camera")]

    private Camera mainCam; //Assignment of the main camera within the scene

    [SerializeField] private bool bUsingController; //Is input being read by a controller for aim commands?

    [Header("Attributes")]

    [SerializeField] private int Lives = 3;


    [Header("UI")]

    [Tooltip("Image to fill based on the health")]
    [SerializeField] private Image healthBar;

    [Tooltip("Health text to display")]
    [SerializeField] private TMP_Text healthText;

    [Tooltip("Lives text to display")]
    [SerializeField] private TMP_Text livesText;

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

            fire = pInput.actions.FindAction("Fire");

            grabWeapon = pInput.actions.FindAction("GrabWeapon");
            dropWeapon = pInput.actions.FindAction("DropWeapon");

            pause = pInput.actions.FindAction("Pause");
        }


    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = Camera.main; //Assign the main camera

        //Add the listeners as needed
        //AddListeners();
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
        if (pause.WasPressedThisFrame())
        {
            //Ensure the game manager exists
            if (Game_Manager.instance == null) return;

            //Call to the game manager to swap the pause state
            Game_Manager.instance.TogglePauseState();
        }

        //Ensure that input can be taken first
        if (!bAllowedInput || Game_Manager.instance.bPaused)
        {
            if (ownedPawn != null) ownedPawn.Move(Vector3.zero); //Ensure the pawn stops
            return;
        }

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

        //Weapon functionality
        if(fire.WasPressedThisFrame())
        {
            //Check that the pawn and held weapon isn't null
            if (ownedPawn is not null && ownedPawn.heldWeapon is not null)
            {
                ownedPawn.heldWeapon.OnPrimaryFireStart.Invoke(); //Invoke the firing start event
            }
        }

        if (fire.WasReleasedThisFrame())
        {
            //Check that the pawn and held weapon isn't null
            if (ownedPawn is not null && ownedPawn.heldWeapon is not null)
            {
                ownedPawn.heldWeapon.OnPrimaryFireEnd.Invoke(); //Invoke the firing end event
            }
        }

        // Pickup and Drop Functionality

        if (grabWeapon.WasPressedThisFrame())
        {
            //Check the player exists and they have something in range
            if (ownedPawn is not null && ownedPawn.interactbleInRange != null)
            {
                ownedPawn.GrabWeapon();
            }
        }

        if (dropWeapon.WasPressedThisFrame())
        {
            //Check the player exists and they have a weapon
            if (ownedPawn is not null && ownedPawn.heldWeapon is not null)
            {
                ownedPawn.DropWeapon();
            }
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
        if (ownedPawn != null)
        {
            RemoveListeners(); //Remove any possible listeners before unpossessing

            ownedPawn.UnPossess(); //Unpossess
        }

        //Possess new pawn
        ownedPawn = inPawnPossession;

        //Tell the new pawn this is the new controller
        ownedPawn.Possess(this);

        //Check if pawn has health component, attaching the lose life function via observation pattern
        AddListeners();

        //Update the health data to the new pawn as needed
        UpdateHealthBar();

        //Enable input
        EnableInput();
    }

    /// <summary>
    /// Unpossess and null the current assigned Pawn
    /// </summary>
    public override void UnPossessPawn()
    {
        //If pawn has hp component, unsubscribe from the unity event
        RemoveListeners();

        ownedPawn.UnPossess(); //Tell the pawn to unpossess itself
        ownedPawn = null; //Null the controllers pawn
    }

    /// <summary>
    /// Add Listeners
    /// </summary>
    public void AddListeners()
    {
        //Check if pawn has health component, attaching the lose life function via observation pattern
        Health hp = ownedPawn.GetComponent<Health>();

        if (hp is not null) //Null check
        {
            hp.OnDeath.AddListener(LoseLife);
            hp.OnTakeDamage.AddListener(UpdateHealthBar);

            hp.OnDeath.AddListener(DisableInput);
        }
    }

    /// <summary>
    /// Remove Listeners
    /// </summary>
    public void RemoveListeners()
    {
        //If pawn has hp component, unsubscribe from the unity event
        Health hp = ownedPawn.GetComponent<Health>();

        if (hp is not null) //Null check
        {
            hp.OnDeath.RemoveListener(LoseLife);
        }
    }


    public override void OnDisableInput()
    {
        //Ensure any sort of fire stops
        if (ownedPawn.heldWeapon != null)
        {
            ownedPawn.heldWeapon.OnPrimaryFireEnd.Invoke();
            ownedPawn.heldWeapon.OnSecondaryFireEnd.Invoke();
        }
    }

    // Life functions

    /// <summary>
    /// Lose a life and update the UI accordingly
    /// </summary>
    public void LoseLife()
    {
        //Disable the input
        DisableInput();

        Lives--; //Decrement the lives

        //Update the life UI
        livesText.text = $"Lives: {Lives}";

        //Check if the player has more lives
        if (Lives is not 0)
        {
            //Respawn after 2 seconds
            Invoke("Respawn", 7);
        }
        else
        {
            //Invoke game over from the game manager
            Game_Manager.instance.GameOver();

            //Destroy this controller
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Private function to call after a few seconds to respawn if lives remain
    /// </summary>
    private void Respawn()
    {
        //Call to the game manager to respawn via spawn manager
        Game_Manager.instance.spawnManager.SpawnPlayer(this);
    }

    /// <summary>
    /// Update the health bar whenever called
    /// </summary>
    /// <param name="currentHealth"> The current health of the pawn </param>
    /// <param name="maxHealth"> The max health of the pawn </param>
    public void UpdateHealthBar()
    {
        //Grab the current health values
        Health hp = ownedPawn.GetComponent<Health>();

        float currentHealth = hp.currentHealth;
        float maxHealth = hp.maxHealth;

        //Get the fill amount for the health bar
        float fillAmount = currentHealth / maxHealth;

        //Set the fill amount to the image
        healthBar.fillAmount = fillAmount;

        //Update the health text
        healthText.text = $"{currentHealth.ToString("F0")}/{maxHealth}";
    }
}
