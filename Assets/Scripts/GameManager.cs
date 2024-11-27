using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    public int allCoinCount;
    public int coinCount;
    private bool collectedAllCoin;

    // Variabel for unity event
    public static GameManager instance;
    public UnityEvent coinResetEvent;

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
        collectedAllCoin = false;

    }

    // Update is called once per frame
    void Update()
    {
        // Track all coins
        CoinTracker();
    }

    public static UnityEvent GetCoinResetEvent()
    {
        return instance.coinResetEvent;
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
            // They have successfully collected all the coin
            collectedAllCoin = true;

            coinResetEvent.Invoke();
        }
    }
}
