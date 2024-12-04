using TMPro;
using UnityEngine;
using UnityEngine.UI;

// State Programming help: https://gameprogrammingpatterns.com/state.html

public class Player : MonoBehaviour
{
    // Declaring class variable
    public Powerup[] powerupArray = new Powerup[3]; // NOTE: this array needs to be initialized in the editor or powerup will not work!! Use powerup Setter
    private PlayerMovement movement;
    private Horse horse;

    // Boolean/Variable for powerup operation
    private bool powerupMultiplierOn;
    private bool powerupSpeedOn;
    private bool powerupSonarOn;
    public bool didPlayerDie; // WORK ON THE PLAYER DEAD STATE, POWERUP RESPAWN TIMER AND BUILD NAVMESH FOR AI
    public bool coinPickUpDelay;
    public int multiplier;
    private const float speedBoost = 1.75f;

    // Game stat/Ui related variable
    private int timerMinute;
    private int timerSecond;
    public float inGameTimer; // This specific timer variable will be use for handling logic for determining difficulty
    private float second;
    public int score;

    // Dealing with HUD
    [SerializeField] TextMeshProUGUI scoreLabel;
    [SerializeField] TextMeshProUGUI timerLabel;
    [SerializeField] TextMeshProUGUI powerupMultiplerLabel;
    [SerializeField] TextMeshProUGUI powerupSpeedLabel;
    [SerializeField] TextMeshProUGUI powerupSonarLabel;
    [SerializeField] Slider staminaBarSlider;

    // Dealing with End Screen
    [SerializeField] GameObject gameoverPanel;
    [SerializeField] TextMeshProUGUI resultLabel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initializing all variable for game operation
        score = 0;
        multiplier = 1;
        timerMinute = 0;
        timerSecond = 0;
        second = 0;
        coinPickUpDelay = false;
        didPlayerDie = false;

        // Powerup related variable
        powerupMultiplierOn = false;
        powerupSpeedOn = false;
        powerupSonarOn = false;

        movement = GetComponent<PlayerMovement>();
        horse = GameObject.Find("Horse").GetComponent<Horse>();
    } 

    // Update is called once per frame
    void Update()
    {
        if(movement.playerState == PlayerMovement.PlayerState.STATE_WALKING || movement.playerState == PlayerMovement.PlayerState.STATE_SPRINTING)
        {
            // Logic handling powerup
            HandlePlayerPowerupFunction();

            // Logic involving countdown
            Timer();
            HandlePlayerPowerupCountdown();

            // UI
            UpdateUI();
        }
        else
        {
            DisplayEndResult();
        }

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
        // Increment the timer based on real seconds
        second += Time.deltaTime;
        inGameTimer += Time.deltaTime; // This specific timer variable will be use for handling logic for determining difficulty (see GameManager, CheckDifficulty())

        if (second >= 60f)
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

        // Displaying the current value of the stamina bar
        staminaBarSlider.value = movement.stamina;
    }

    private void DisplayEndResult()
    {
        // Tint the screen red to indicate game over
        gameoverPanel.SetActive(true);

        // Display end result
        resultLabel.text = string.Format("GAME OVER\n\nScore: {0}\nTime:{1} Minute, {2} Second", score, timerMinute, timerSecond); 
    }
}
