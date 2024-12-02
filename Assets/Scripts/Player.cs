using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Declaring class variable
    public Powerup[] powerupArray = new Powerup[3];
    private PlayerMovement movement;
    private Horse horse;

    // Boolean/Variable for powerup operation
    public bool powerupMultiplierOn;
    public bool powerupSpeedOn;
    public bool powerupSonarOn;
    public int multiplier;
    private const float speedBoost = 4f;

    // Game stat/Ui related variable
    private int timerMinute;
    private int timerSecond;
    private float second;
    public int score;

    // Dealing with UI
    [SerializeField] TextMeshProUGUI scoreLabel;
    [SerializeField] TextMeshProUGUI timerLabel;
    [SerializeField] TextMeshProUGUI powerupMultiplerLabel;
    [SerializeField] TextMeshProUGUI powerupSpeedLabel;
    [SerializeField] TextMeshProUGUI powerupSonarLabel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initializing all variable for game operation
        score = 0;
        multiplier = 1;
        timerMinute = 0;
        timerSecond = 0;
        second = 0;

        powerupMultiplierOn = false;
        powerupSpeedOn = false;
        powerupSonarOn = false;

        movement = GetComponent<PlayerMovement>();
        horse = GameObject.Find("Horse").GetComponent<Horse>();
    } 

    // Update is called once per frame
    void Update()
    {
        // Logic handling powerup
        HandlePlayerPowerupFunction();

        // Logic involving countdown
        Timer();
        HandlePlayerPowerupCountdown();

        // UI
        UpdateUI();
    }

    // To handle the powerup entity
    private void OnTriggerEnter(Collider other)
    {
        // Logic for when the power collide with the powerup object
        if(other.CompareTag("Powerup"))
        {
            // Turn off the powerup
            other.gameObject.SetActive(false);

            // Giving the powerup to the player
            AddPowerup(other.GetComponent<Powerup>());
        }
    }


    public void HandlePlayerPowerupFunction()
    {
        // Powerup for score multiplier
        if (powerupMultiplierOn == true)
        {
            // Add upon to multiplayer
            multiplier = 4;
        }
        else
        {
            // resume normal behavior
            powerupMultiplierOn = false;
            multiplier = 1;
        }


        // Powerup for speed boost
        if (powerupSpeedOn == true)
        {
            // Add functionality for speed powerup
            movement.speedMultiplier = speedBoost;
        }
        else
        {
            // resume normal behavior
            powerupSpeedOn = false;
            movement.speedMultiplier = 1f;
        }


        // Powerup for sonar vison
        if (powerupSonarOn == true)
        {
            horse.sonarZone.SetActive(true);
        }
        else
        {
            powerupSonarOn = false;
            horse.sonarZone.SetActive(false);
        }
    }

    // "Adding" the power by enabling and reseting the powerup timer in the player's powerup array
    public void AddPowerup(Powerup _powerup)
    {
        if(_powerup.powerUpType == Powerup.PowerupClassification.Multiplier)
        {
            powerupMultiplierOn = true;
            powerupArray[0].ResetPowerUp();
        }
        else if (_powerup.powerUpType == Powerup.PowerupClassification.Speed)
        {
            powerupSpeedOn = true;
            powerupArray[1].ResetPowerUp();
        }
        else
        {
            powerupSonarOn = true;
            powerupArray[2].ResetPowerUp();
        }
    }


    public void HandlePlayerPowerupCountdown()
    {
        // Counting down the powerup based on whichever is turned on
        if (powerupMultiplierOn == true) // for Multipler
        {
            // Counting down
            powerupArray[0].countdownAmount -= Time.deltaTime;

            // Setting countdown to remain at 0 and false once finished
            if(powerupArray[0].countdownAmount <= 0)
            {
                powerupArray[0].countdownAmount = 0;
                powerupMultiplierOn = false;
            }
        }


        if (powerupSpeedOn == true) // for Speed
        {
            // Counting down
            powerupArray[1].countdownAmount -= Time.deltaTime;

            // Setting countdown to remain at 0 and false once finished
            if (powerupArray[1].countdownAmount <= 0)
            {
                powerupArray[1].countdownAmount = 0;
                powerupSpeedOn = false;
            }
        }


        if (powerupSonarOn == true == true) // for Sonar
        {
            // Counting down
            powerupArray[2].countdownAmount -= Time.deltaTime;

            // Setting countdown to remain at 0 and false once finished
            if (powerupArray[2].countdownAmount <= 0)
            {
                powerupArray[2].countdownAmount = 0;
                powerupSonarOn = false;
            }
        }
    }


    public void GainPoint(int amount)
    {
        score += amount * multiplier;
    }

    public void Timer()
    {
        // Decrement the timer
        second += Time.deltaTime;
        if(second >= 60f)
        {
            timerMinute += 1;
            second = 0;
        }

        // Displaying the timer to the player 
        timerLabel.text = string.Format("{0:D2}:{1:F1}", timerMinute, timerSecond);
    }

    private void UpdateUI()
    {
        // Casting
        timerSecond = (int)second;

        // Displaying score
        scoreLabel.text = string.Format("Game Score: {0}", score);

        // Displaying the timer to the player 
        timerLabel.text = string.Format("{0:D2}:{1:D2}", timerMinute, timerSecond);

        // Displaying the powerup Timer to the player 
        powerupMultiplerLabel.text = string.Format("{0:F1}", powerupArray[0].countdownAmount);
        powerupSpeedLabel.text = string.Format("{0:F1}", powerupArray[1].countdownAmount);
        powerupSonarLabel.text = string.Format("{0:F1}", powerupArray[2].countdownAmount);
    }
}
