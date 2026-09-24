using UnityEngine;

public class AIPawn : Pawn
{
    public override void Move(Vector3 direction)
    {
        //throw new System.NotImplementedException();
    }

    public override void Possess(Controller newController)
    {
        owningController = newController;
    }

    public override void RotateTowards(Vector3 rotateDirection)
    {
        //throw new System.NotImplementedException();
    }

    public override void UnPossess()
    {
        owningController = null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
