using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    [Header("Owning Properties")]

    public Pawn selfPawn;
    public Controller owningController;

    [Header("Attributes")]

    public float moveSpeed = 6f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selfPawn = this;
    }

    public abstract void Move(Vector3 direction);

    //Possession
    public abstract void Possess(Controller newController);
    public abstract void UnPossess();
}
