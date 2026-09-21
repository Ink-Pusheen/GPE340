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
        Vector3 vectorToPoint =  transform.position - rotateDirection;

        Quaternion lookRotation = Quaternion.FromToRotation(transform.position, vectorToPoint);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotateSpeed);
    }
}
