using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

// !!! CREDIT for this entire script: Comp-3 Interactive on youtube "How to Add a Field of View for Your Enemies [Unity Tutorial]", https://www.youtube.com/watch?v=j1-OyLo77ss
// Comment that you see in the code was written by me and not Comp-3 Interactive
public class FieldOfView : MonoBehaviour
{
    // Declaring class variable
    // Attributes for manipulating the field of view's angle and range
    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    private void Start()
    {
        // Finding player in the scene and starting a special while loop to scanning player within FOV
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }


    // This will act as a update loop but will only activate every 0.2 seconds instead of every frame ()
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }


    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        // Making sure there was something detecting inside the overlap sphere
        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform; // Using the target mask, we know that the player is the only one that will be stored inside the array
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // Checking to see if the player is really in the horse fov by checking if the player is inside the view 
            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                // This last check is to make sure the player is in clear, open view. If player is hiding behind the wall, then the horse cant see the player
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }

}
