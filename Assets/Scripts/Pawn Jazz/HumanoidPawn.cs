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
        Vector3 normalized = Vector3.ClampMagnitude(direction.normalized * moveSpeed, 1);

        //Set movement parameters based on vector input for animation purposes

        anim.SetFloat("XInput", normalized.x);
        anim.SetFloat("ZInput", normalized.z);
    }

    public override void Possess(Controller newController)
    {
        owningController = newController;
    }

    public override void UnPossess()
    {
        owningController = null;
    }
}
