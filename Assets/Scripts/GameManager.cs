using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public enum GameDifficulty
    {
        Easy,
        Normal,
        Hard,
        UltraHard
    }

    // Declaring class variable
    // Dealing with game object, game difficulty and game's stat
    private Player player;
    private Horse horse;
    public GameDifficulty difficulty;
    public int allCoinCount;
    public int coinCount;
    public float powerupRespawnTimer;
    public float powerupRespawnTimerReset;

    // Variable for unity event
    public static GameManager instance;
    public UnityEvent coinResetEvent;
    public UnityEvent powerupRespawnEvent;

    // Setting up the singleton for unityevent
    private void Awake()
    {
        instance = this;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize all variable
        difficulty = GameDifficulty.Easy;
        powerupRespawnTimerReset = powerupRespawnTimer;
        SearchAllCoins();
        coinCount = 0;
        player = GameObject.Find("Player").GetComponent<Player>();
        horse = GameObject.Find("Horse").GetComponent<Horse>();
    }


    // Update is called once per frame
    void Update()
    {
        // Track the game's current difficulty and changing game's stat based on said difficulty
        CheckDifficulty();
        ChangeGameStatBasedOnDifficulty();

        // Track all coins
        CoinTracker();

        // Operation for handling powerup
        PowerupTimer();
        CheckPowerupTimer();
    }


    // For subscribing to the game manager's unityevent
    public static UnityEvent GetCoinResetEvent()
    {
        return instance.coinResetEvent;
    }


    public static UnityEvent GetPowerupRespawnEvent()
    {
        return instance.powerupRespawnEvent;
    }


    private void SearchAllCoins()
    {
        // Storing all the coin inside a variable for logical operation
        allCoinCount = GameObject.FindGameObjectsWithTag("Coin").Length;

        Debug.Log($"There are {allCoinCount} coins in the level");
    }


    public void AddToCoinCount(int amount)
    {
        coinCount += amount;
    }


    private void CoinTracker()
    {
        // If the player coin count equals to the amount of coin inside a level...
        if (coinCount == allCoinCount)
        {
            // Setting the boolean for coin pick up delay to true for player
            player.coinPickUpDelay = true;

            // Begin coroutine to make the coin respawn after a short delay
            CoinRespawnDelay();
        }
    }


    public void CoinRespawnDelay()
    {
        StartCoroutine(CoinRespawnDelayRoutine());
    }


    IEnumerator CoinRespawnDelayRoutine()
    {
        // Set a delay for the coin to respawn in a scene
        coinResetEvent.Invoke();
        yield return new WaitForSeconds(0.5f);

        // Make it so that the player can pick up coin again (prevent a bug)
        player.coinPickUpDelay = false;
    }


    private void PowerupTimer()
    { 
        powerupRespawnTimer -= Time.deltaTime;
    }


    private void CheckPowerupTimer()
    {
        // Once the timer reaches zero, respawn all powerup in a scene
        if(powerupRespawnTimer <= 0)
        {
            powerupRespawnEvent.Invoke();
            powerupRespawnTimer = powerupRespawnTimerReset;
            //Debug.Log($"Powerup Timer has reseted!!  now at {powerupRespawnTimer}");
        }
    }


    // Difficulty will affect the poweruprespawn timer and movement speed of the horse
    public void ChangeGameStatBasedOnDifficulty()
    {
        switch(difficulty)
        {
            case GameDifficulty.Easy:
                powerupRespawnTimerReset = 30f;
                horse.horseSpeedMultipler = 1;
                break;

            case GameDifficulty.Normal:
                powerupRespawnTimerReset = 45f;
                horse.horseSpeedMultipler = 1.5f;
                break;

            case GameDifficulty.Hard:
                powerupRespawnTimerReset = 75f;
                horse.horseSpeedMultipler = 2.5f;
                break;

            case GameDifficulty.UltraHard:
                powerupRespawnTimerReset = 120f;
                horse.horseSpeedMultipler = 4f;
                break;
        }
    }


    public void CheckDifficulty()
    {
        // Checking to see if the player's timer is within range of 0-1 minutes for easy mode
        if(player.inGameTimer >= 0f && player.inGameTimer <= 60f)
        {
            difficulty = GameDifficulty.Easy;
        }
        // Checking to see if the player's timer is within range of 1-3 minutes for Normal mode
        else if (player.inGameTimer > 60f && player.inGameTimer <= 180f)
        {
            difficulty = GameDifficulty.Normal;
        }
        // Checking to see if the player's timer is within range of 3-6 minutes for Hard mode
        else if (player.inGameTimer > 180f && player.inGameTimer <= 360f)
        {
            difficulty = GameDifficulty.Hard;
        }
        // Past 6+ minutes for Ultra mode
        else
        {
            difficulty = GameDifficulty.UltraHard;
        }
    }
}
