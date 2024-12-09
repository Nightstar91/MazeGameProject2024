using TMPro;
using UnityEngine;
using UnityEngine.UI;

// State Programming help: https://gameprogrammingpatterns.com/state.html

public class Player : MonoBehaviour
{
    // Declaring class variable
    // NOTE: this array needs to be initialized in the editor or powerup will not work!! Use powerup Setter.
    // Use the following order: 1. Multiplier, 2. Speed, 3. Sonar
    public Powerup[] powerupArray = new Powerup[3];

    public PlayerMovement movement;
    private Horse horse;

    // Boolean/Variable for powerup operation
    private bool powerupMultiplierOn;
    private bool powerupSpeedOn;
    private bool powerupSonarOn;
    public bool didPlayerDie; 
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
        // This is for when the player is in a dead state
        else
        {
            DisplayEndResult();
        }

    }


    // To handle the game entity found in the scene
    private void OnTriggerEnter(Collider other)
    {
        // Checking to see if the player collide with the horse
        if(other.CompareTag("Powerup"))
        {
            // Turn off the powerup
            other.gameObject.SetActive(false);

            // Giving the powerup to the player
            AddPowerup(other.GetComponent<Powerup>());
        }

        // Checking to see if the player collide with the horse
        else if(other.name == "HorseCollison")
        {
            movement.playerState = PlayerMovement.PlayerState.STATE_DEAD;
        }
    }


    public void HandlePlayerPowerupFunction()
    {
        // Checking to see if the player picked up the multiplier powerup
        if (powerupMultiplierOn == true)
        {
            // Add upon to multiplayer
            multiplier = 4;
        }
        // for when the powerup ran out
        else
        {
            // resume normal behavior
            powerupMultiplierOn = false;
            multiplier = 1;
        }


        // Checking to see if the player picked up the speed boost powerup
        if (powerupSpeedOn == true)
        {
            // Add functionality for speed powerup
            movement.speedMultiplier = speedBoost;
        }
        // for when the powerup ran out
        else
        {
            // resume normal behavior
            powerupSpeedOn = false;
            movement.speedMultiplier = 1f;
        }


        // Checking to see if the player picked up the sonar vison powerup
        if (powerupSonarOn == true)
        {
            horse.sonarZone.SetActive(true);
        }
        // for when the powerup ran out
        else
        {
            powerupSonarOn = false;
            horse.sonarZone.SetActive(false);
        }
    }


    // "Adding" the power by enabling and or reseting the powerup timer in the player's powerup array
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
        inGameTimer += Time.deltaTime; // This specific timer variable will be use for handling logic for determining difficulty (see GameManager, CheckDifficulty method)

        if (second >= 60f)
        {
            // for labeling
            timerMinute += 1;
            second = 0;
        }

        // Displaying the timer to the player 
        timerLabel.text = string.Format("{0:D2}:{1:F1}", timerMinute, timerSecond);
    }


    private void UpdateUI()
    {
        // Casting for labeling in the player UI
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


    public void DisplayEndResult()
    {
        // Tint the screen red to indicate game over
        gameoverPanel.SetActive(true);

        // Display end result
        resultLabel.text = string.Format("GAME OVER\n\nScore: {0}\nYou have lasted for...\n{1} Minutes and {2} Seconds", score, timerMinute, timerSecond); 
    }
}
