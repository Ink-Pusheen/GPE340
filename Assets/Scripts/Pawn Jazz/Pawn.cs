using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    [Header("Owning Properties")]

    public Pawn selfPawn;
    public Controller owningController;

    [Header("Attributes")]

    public float moveSpeed = 6f;

    public float rotateSpeed = 6f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public abstract void Start();

    public abstract void Move(Vector3 direction);

    public abstract void RotateTowards(Vector3 rotateDirection);

    //Possession
    public abstract void Possess(Controller newController);
    public abstract void UnPossess();
}
