using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Powerup : MonoBehaviour
{
    // Enum for poweruptype
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


    // Reseting the player powerup timer based on its intial countdown amount
    public void ResetPowerUp()
    {
        countdownAmount = countdownAmountSave;
    }


    // Respawn the powerup found in the scene for the player to pick up again
    public void RespawnPowerup()
    {
        gameObject.SetActive(true);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
