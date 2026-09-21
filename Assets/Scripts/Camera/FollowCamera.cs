using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] GameObject followObject; //The object that this camera will be following

    [SerializeField] Vector3 offset; //Offset of the camera

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (followObject != null) //Null Check
        {
            transform.position = followObject.transform.position + offset; //Set the position accordingly
        }
    }
}
