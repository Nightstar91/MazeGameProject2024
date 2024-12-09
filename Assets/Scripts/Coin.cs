using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Coin : MonoBehaviour
{
    // Declaring variables
    // To search for a player object in the scene
    public Player player;
    private GameManager gameManager;

    // Involving audio
    public AudioSource coinAudioSource;
    [SerializeField] private AudioClip collectClip;

    // Start is called before the first frame update
    void Start()
    {
        // Finding the player object in the scene to reference the collision
        player = GameObject.Find("Player").GetComponent<Player>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Handling audio 
        coinAudioSource = GetComponent<AudioSource>();
        coinAudioSource.clip = collectClip;

        // Subscribe to the coin reset event
        GameManager.GetCoinResetEvent().AddListener(ResetCoin);
    }


    public void ResetCoin()
    {
        // Resetting the coin object so it respawn for the player to collect again
        gameObject.SetActive(true);

        gameManager.coinCount = 0;
    }


    // On collision give the player one coin to be added to their stats
    private void OnTriggerEnter(Collider other)
    {

        // Checking to see if the collison was cause by the player
        if (other.name == "Player" && player.coinPickUpDelay == false)
        {
            player.GainPoint(10);           // Gain points with each coin pick up
            gameManager.AddToCoinCount(1);  // Add to coin count for internal tracking
            gameObject.SetActive(false);    // Using SetActive to be use for reset

            //Debug.Log($"COIN AMOUNT IN PLAYER: {gameManager.coinCount} , COIN AMOUNT IN LEVEL: {gameManager.allCoinCount} ");

            //coinAudioSource.PlayOneShot(coinAudioSource.clip);
        }
    }
}
