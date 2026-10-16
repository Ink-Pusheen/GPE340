using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DestroyOnDeath : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer meshRender; //A render of the gameobject if applicable

    [SerializeField] private bool bController; //Is this a controller to destroy?

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meshRender = GetComponentInChildren<SkinnedMeshRenderer>(); //Grab the mesh renderer component

        Health objectHP = GetComponent<Health>(); //Grab the health component

        //Ensure it's not null before subscribing automatically
        if (objectHP is not null && !bController) objectHP.OnDeath.AddListener(QueueDeterioration);

        else
        {
            //Try grabbing the AI controller to subscribe
            AIController controller = GetComponent<AIController>();

            //Null Checks
            if (controller is not null && controller.ownedPawn is not null)
            {
                //Attempt one last connection
                objectHP = controller.ownedPawn.gameObject.GetComponent<Health>();

                //Subscribe once connected if found
                if (objectHP is not null) objectHP.OnDeath.AddListener(QueueDeterioration);
            }
        }
    }

    //Public function to blink the body before destroying it
    public void QueueDeterioration()
    {
        StartCoroutine("Deteriorate"); //Run the Coroutine IEnumerator
    }

    IEnumerator Deteriorate()
    {
        float waitTime = Random.Range(4f, 7f);

        yield return new WaitForSeconds(waitTime);

        //Loop three time to blink the mesh
        for (int blink = 0; blink < 3; blink++)
        {
            if (meshRender is null) break; //Break the loop if this is null

            meshRender.enabled = false; //Hide the mesh

            yield return new WaitForSeconds(0.15f); //Delay

            meshRender.enabled = true; //Display the mesh

            yield return new WaitForSeconds(0.2f); //Delay
        }

        Destroy(gameObject); //Destroy the gameobject this is attached to
    }
}
