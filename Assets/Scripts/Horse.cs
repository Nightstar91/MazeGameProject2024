using UnityEngine;

public class Horse : MonoBehaviour
{
    public GameObject sonarZone;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find the gameobject that represent the Sonar zone for player
        sonarZone = GameObject.Find("SonarZone");

        // Turn off immediately
        sonarZone.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
