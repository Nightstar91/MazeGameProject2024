using UnityEngine;
using UnityEngine.AI;

public class Horse : MonoBehaviour
{
    public enum HorseState
    {
        STATE_WANDERING,
        STATE_CHASING
    }

    // Declaring class variable\
    // Variable involving AI
    private NavMeshAgent horseAI;
    private FieldOfView horseFOV;

    private GameObject player;
    [SerializeField] private GameObject waypointToNavigate = null;
    private GameObject waypointToNavigateTemp;
    private GameObject[] allWaypoint;

    private bool hasReachDestination;
    private bool hasSeenPlayer;

    private int allWaypointLength;
    public float horseSpeedMultipler = 1;

    public HorseState state;

    // Variable relating to powerup Operation
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

        hasSeenPlayer = false;

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
        horseAI.speed *= horseSpeedMultipler;

        switch(state)
        {
            case HorseState.STATE_WANDERING:
                // Based on the boolean check it will either continue to move toward way point or generate a new waypoint
                MoveToWayPoint();

                // The horse check to see if it reach the way point
                hasReachDestination = CheckIfDestinationReached();

                // Checking to see if the horse see the player, if so change to state to be chasing
                if(horseFOV.canSeePlayer == true)
                {
                    state = HorseState.STATE_CHASING;
                }

                break;

            case HorseState.STATE_CHASING:
                // Reset the waypoint that the horse was navigating
                Debug.Log("HORSE SEES THE PLAYER");

                waypointToNavigate = null;

                horseAI.SetDestination(player.transform.position);

                if(horseFOV.canSeePlayer == false)
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
        if (waypointToNavigate == null)
        {
            waypointToNavigate = allWaypoint[Random.Range(0, allWaypointLength - 1)];
            waypointToNavigateTemp = waypointToNavigate;
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
        if (horseAI.remainingDistance <= horseAI.stoppingDistance)
        {
            waypointToNavigate = null;
            return true;
        }
        else
        {
            return false;
        }   
    }
}
