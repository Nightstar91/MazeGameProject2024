using UnityEngine;
using UnityEngine.AI;

public class Horse : MonoBehaviour
{
    public enum HorseState
    {
        STATE_WANDERING,
        STATE_CHASING
    }

    // Declaring class variable
    // Variable involving AI
    private NavMeshAgent horseAI;
    private FieldOfView horseFOV;
    private GameObject player;
    [SerializeField] private GameObject waypointToNavigate = null;
    private GameObject[] allWaypoint;
    private bool hasReachDestination;
    private int allWaypointLength;

    // Variable involving the horse's stat
    public float horseSpeedMultipler = 1;
    private const float horseSpeed = 3f;
    public HorseState state;

    // Variable relating to powerup operation
    public GameObject sonarZone;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initializing all variable involving AI
        horseAI = GetComponent<NavMeshAgent>();
        horseFOV = GetComponent<FieldOfView>(); 
        SearchAllWaypoint();
        ChooseRandomWaypoint();
        horseAI.SetDestination(waypointToNavigate.transform.position);

        // Setting the default state
        state = HorseState.STATE_WANDERING;

        // Find the gameobject that represent the Sonar zone for player
        sonarZone = GameObject.Find("SonarZone");
        player = GameObject.Find("Player");

        // Turn off immediately
        sonarZone.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Setting the speed based on the difficulty
        horseAI.speed = horseSpeed * horseSpeedMultipler;

        switch(state)
        {
            case HorseState.STATE_WANDERING:
                // Based on the boolean check it will either continue to move toward way point or generate a new waypoint
                MoveToWayPoint();

                // The horse check to see if it reached the way point
                hasReachDestination = CheckIfDestinationReached();

                // Checking to see if the horse see the player, if so change to state to chasing
                if(horseFOV.canSeePlayer == true)
                {
                    state = HorseState.STATE_CHASING;
                }

                break;

            case HorseState.STATE_CHASING:
                // Reset the waypoint that the horse was navigating
                waypointToNavigate = null;

                // Set the waypoint to the player position to chase them
                horseAI.SetDestination(player.transform.position);

                // If the horse lose sight of the player or is dead, Go back to wandering
                if(horseFOV.canSeePlayer == false || player.GetComponent<Player>().movement.playerState != PlayerMovement.PlayerState.STATE_DEAD)
                {
                    state = HorseState.STATE_WANDERING;
                }

                break;
        }
    }

    private void SearchAllWaypoint()
    {
        // Search for all game object with waypoint in their tag
        allWaypoint = GameObject.FindGameObjectsWithTag("Waypoint");

        // Get the length of the array of waypoint to be use for method for randomly assigning 
        allWaypointLength = allWaypoint.Length;

        Debug.Log($"There are {allWaypointLength} waypoints in the map");
    }


    private void ChooseRandomWaypoint()
    {
        // Only generate a new waypoint if the waypoint variable is null
        if (waypointToNavigate == null)
        {
            // Pick a random waypoint found in the scene
            waypointToNavigate = allWaypoint[Random.Range(0, allWaypointLength - 1)];
        }
    }


    private void MoveToWayPoint()
    {
        // For wandering ai path
        if(hasReachDestination != true && waypointToNavigate != null)
        {
            horseAI.SetDestination(waypointToNavigate.transform.position);
        }
        // Generate a new ai path after completing the wandering path
        else
        {
            ChooseRandomWaypoint();
            horseAI.SetDestination(waypointToNavigate.transform.position);
        }
    }


    private bool CheckIfDestinationReached()
    {
        // Checking to see if the horse reach the waypoint based on distance
        if (horseAI.remainingDistance <= horseAI.stoppingDistance)
        {
            // Reset waypoint
            waypointToNavigate = null;

            return true;
        }
        else
        {
            return false;
        }   
    }
}
