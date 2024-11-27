using UnityEngine;

public class Powerup : MonoBehaviour
{
    // enum
    public enum PowerupClassification
    {
        Multiplier,
        Speed,
        Sonar
    }

    private PowerupClassification PowerUp;
    private float countdownAmount;
    private float countdownAmountSave;

    public bool activatePowerup;
    public int scoreMultiplier { get; set; }
    private const float speedBoost = 2.4f;

    public Powerup(PowerupClassification _powerup, int _timeAmount)
    {
        PowerUp = _powerup;
        countdownAmountSave = _timeAmount;
        countdownAmount = countdownAmountSave;

        if(PowerUp == PowerupClassification.Multiplier)
        {
            scoreMultiplier = 2;
        }
        else
        {
            scoreMultiplier = 0;
        }
    }

    public void ResetPowerUp()
    {
        countdownAmount = countdownAmountSave;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
