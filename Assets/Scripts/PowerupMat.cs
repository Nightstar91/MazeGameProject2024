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

        // Determining what powerup the entity is and changing the color based on powerup type
        if(powerup.powerUpType == Powerup.PowerupClassification.Multiplier)
        {
            powerupEntity.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else if(powerup.powerUpType == Powerup.PowerupClassification.Speed)
        {
            powerupEntity.GetComponent<Renderer>().material.color = Color.cyan;
        }
        else
        {
            powerupEntity.GetComponent<Renderer>().material.color = Color.red;
        }
    }


}
