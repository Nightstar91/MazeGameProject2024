using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{

    public int allCoinCount;
    public int coinCount;
    private float powerupRespawnTimer = 100f;

    // Variabel for unity event
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
        SearchAllCoins();
        coinCount = 0;

    }

    // Update is called once per frame
    void Update()
    {
        // Track all coins
        CoinTracker();

        // Operation for handling powerup
        PowerupTimer();
        CheckPowerupTimer();
    }


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
            // Begin coroutine to make the coin respawn after a short delay
            //CoinRespawnDelay();

            coinResetEvent.Invoke();
        }
    }


    public void CoinRespawnDelay()
    {
        StartCoroutine(CoinRespawnDelayRoutine());
    }


    IEnumerator CoinRespawnDelayRoutine()
    {
        yield return new WaitForSeconds(3f);

        coinResetEvent.Invoke();
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
            powerupRespawnTimer = 100f;
            //Debug.Log($"Powerup Timer has reseted!!  now at {powerupRespawnTimer}");
        }
    }
}
