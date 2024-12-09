using UnityEngine;

public class PowerupMat : MonoBehaviour
{
    //Declaring class variable
    private Powerup powerup;
    private GameObject powerupEntity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initializing variable
        powerup = GetComponent<Powerup>();
        powerupEntity = this.gameObject;

        // Determining what powerup the entity is and changing the material color based on powerup type
        if(powerup.powerUpType == Powerup.PowerupClassification.Multiplier) // Yellow for multiplier
        {
            powerupEntity.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else if(powerup.powerUpType == Powerup.PowerupClassification.Speed) // cyan for speed boost
        {
            powerupEntity.GetComponent<Renderer>().material.color = Color.cyan;
        }
        else // red for sonar vision
        {
            powerupEntity.GetComponent<Renderer>().material.color = Color.red;
        }
    }


}
