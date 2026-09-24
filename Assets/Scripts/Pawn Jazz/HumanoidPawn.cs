using UnityEngine;

public class HumanoidPawn : Pawn
{
    public Animator anim;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Grab components
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 500, Color.red);
    }

    public override void Move(Vector3 direction)
    {
        Vector3 normalizedDirection = Vector3.ClampMagnitude(direction.normalized, 1); //Normalize the direction

        normalizedDirection *= moveSpeed; //Multiply the movespeed

        normalizedDirection = transform.InverseTransformDirection(normalizedDirection);

        //Set movement parameters based on vector input for animation purposes

        anim.SetFloat("XInput", normalizedDirection.x);
        anim.SetFloat("ZInput", normalizedDirection.z);
    }

    public override void Possess(Controller newController)
    {
        owningController = newController;
    }

    public override void UnPossess()
    {
        owningController = null;
    }

    public override void RotateTowards(Vector3 rotateDirection)
    {
        Vector3 vectorToPoint = rotateDirection - transform.position;
        //Debug.Log(vectorToPoint); //Testing
        Quaternion lookRotation = Quaternion.LookRotation(vectorToPoint, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
    }
}
