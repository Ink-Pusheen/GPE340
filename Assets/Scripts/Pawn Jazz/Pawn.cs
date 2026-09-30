using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    [Header("Owning Properties")]

    [Tooltip("Reference to the the current Pawn Script")]
    public Pawn selfPawn;

    [Tooltip("Reference to the Controller which is currently Possessing this Pawn")]
    public Controller owningController;

    [Header("Attributes")]

    [Tooltip("The speed at which this pawn will move")]
    public float moveSpeed = 6f;

    [Tooltip("The speed at which this pawn will rotate towards its target")]
    public float rotateSpeed = 6f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public abstract void Start();

    // Movement

    /// <summary>
    /// Movement functionality for the given pawn
    /// </summary>
    /// <param name="direction"></param>
    public abstract void Move(Vector3 direction);

    /// <summary>
    /// Rotation functionality towards a point over time for the given pawn
    /// </summary>
    /// <param name="rotateDirection"></param>
    public abstract void RotateTowards(Vector3 rotateDirection);

    // Possession

    /// <summary>
    /// Assign the new controller as the current possessing controller
    /// </summary>
    /// <param name="newController"></param>
    public abstract void Possess(Controller newController);

    /// <summary>
    /// Remove and null the currently possessing controller if there is one
    /// </summary>
    public abstract void UnPossess();
}
