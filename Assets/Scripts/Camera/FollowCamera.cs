using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("Camera Attributes")]

    [SerializeField] private GameObject ObjectToFollow; //The object that this camera will be following

    [Tooltip("Offset at which the Camera will add while following the target")]
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        if (ObjectToFollow != null) //Null Check
        {
            transform.position = ObjectToFollow.transform.position + offset; //Set the position accordingly
        }
    }
}
