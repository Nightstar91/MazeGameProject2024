using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Powerup : MonoBehaviour
{
    // enum
    public enum PowerupClassification
    {
        Multiplier,
        Speed,
        Sonar
    }

    // Declaring class variable
    [SerializeField] public PowerupClassification powerUpType;
    public float countdownAmount;
    private float countdownAmountSave;


    public void ResetPowerUp()
    {
        countdownAmount = countdownAmountSave;
    }


    public void RespawnPowerup()
    {
        gameObject.SetActive(true);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        // Just in case powertype wasn't set for any reason.
        if (powerUpType == null)
        {
            powerUpType = PowerupClassification.Multiplier;
            Debug.Log($"poweruptype was null, entity can be found here at X:{gameObject.transform.position.x}, Y:{gameObject.transform.position.y}, Z{gameObject.transform.position.z}, Multiplier automatically applied.");
        }

        // Initializing the time variable to be transfer to the player based on what the powerup type is set by the level designer
        if(powerUpType == PowerupClassification.Multiplier)
        {
            countdownAmount = 120f;
            countdownAmountSave = countdownAmount;
            countdownAmount = 0f;
        }
        else if (powerUpType == PowerupClassification.Speed)
        {
            countdownAmount = 45f;
            countdownAmountSave = countdownAmount;
            countdownAmount = 0f;
        }
        else
        {
            countdownAmount = 30f;
            countdownAmountSave = countdownAmount;
            countdownAmount = 0f;
        }

        // Subscribe to the powerup respawn event
        GameManager.GetPowerupRespawnEvent().AddListener(RespawnPowerup);
    }

}
