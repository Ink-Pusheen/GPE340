using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("Camera Attributes")]

    [SerializeField] private GameObject ObjectToFollow; //The object that this camera will be following

    [Tooltip("Offset at which the Camera will add while following the target")]
    public Vector3 offset;

    [Tooltip("Rotation of the camera during gameplay")]
    public Quaternion gameplayRotation;


    [Header("Non gameplay elements")]

    [Tooltip("Location of the camera when not playing")]
    [SerializeField] private Vector3 idlePosition;

    [Tooltip("Rotation of the camera no in gameplay")]
    public Quaternion idleRotation;


    private void Start()
    {
        idlePosition = transform.position; //Set the initial idle position
        idleRotation = transform.rotation; //Set the initial idle rotation
    }

    // Update is called once per frame
    void Update()
    {
        //If the game manager isn't null and not in gameplay, have the camera idle
        if (Game_Manager.instance != null && !Game_Manager.instance.bPlaying)
        {
            transform.position = idlePosition;
            transform.rotation = idleRotation;
        }

        if (ObjectToFollow != null) //Null Check
        {
            transform.position = ObjectToFollow.transform.position + offset; //Set the position accordingly
            transform.rotation = gameplayRotation; //Set the rotation accordingly
        }
        else
        {
            findTargetPlayer();
        }
    }

    private void findTargetPlayer()
    {
        //Set the target of the player
        if (Game_Manager.instance != null && Game_Manager.instance.player != null)
        {
            //Get the player controller component
            PlayerController playerController = Game_Manager.instance.player.GetComponent<PlayerController>();

            if (playerController is not null && playerController.ownedPawn != null) //Ensure the controller isn't null
            {
                ObjectToFollow = playerController.ownedPawn.gameObject; //Set the target
            }
            
        }
    }
}
