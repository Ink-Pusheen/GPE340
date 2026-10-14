using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    [Tooltip("The Pawn this controller owns and controlls")]
    public Pawn ownedPawn;

    [Tooltip("Controls if input will be accepted")]
    public bool bAllowedInput = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void Decisions();

    //Possession
    public abstract void PossessPawn(Pawn inPawnPossession);
    public abstract void UnPossessPawn();


    /// <summary>
    /// Public function for after death functionality
    /// </summary>
    public void DisableInput()
    {
        bAllowedInput = false;
    }
}
