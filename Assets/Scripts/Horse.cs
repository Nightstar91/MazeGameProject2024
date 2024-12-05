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
    private GameObject player;
    private NavMeshAgent horseAI;
    private bool hasReachDestination;
    private bool hasSeenPlayer;
    [SerializeField] private GameObject waypointToNavigate;
    private GameObject waypointToNavigateTemp;
    private GameObject[] allWaypoint;
    private int allWaypointLength;
    public HorseState state;

    // Variable relating to powerup Operation
    public GameObject sonarZone;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initializing all variable involving AI
        horseAI = GetComponent<NavMeshAgent>();
        SearchAllWaypoint();

        hasReachDestination = false;
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
        switch(state)
        {
            case HorseState.STATE_WANDERING:
                horseAI.SetDestination(waypointToNavigate.transform.position);
                break;

            case HorseState.STATE_CHASING:
                horseAI.SetDestination(player.transform.position);
                break;
        }
    }

    private void SearchAllWaypoint()
    {
        // Search for all game object with waypoint in their tag
        allWaypoint = GameObject.FindGameObjectsWithTag("Waypoint");

        // Get the length of the array of waypoint to be use for method for randomly assigning 
        allWaypointLength = allWaypoint.Length;
    }

    private void ChooseRandomWaypoint()
    {
        if (waypointToNavigate == null)
        {
            waypointToNavigateTemp = allWaypoint[Random.Range(0, allWaypointLength - 1)]; 
        }
    }

    private void MoveToWayPoint()
    {

    }
}
