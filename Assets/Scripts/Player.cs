using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Declaring class variable
    private Powerup[] powerupArray = new Powerup[3];
    private int timerMinute;
    private float timerSecond;
    public int score;
    private int multiplier;

    [SerializeField] TextMeshProUGUI scoreLabel;
    [SerializeField] TextMeshProUGUI timerLabel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
        multiplier = 1;
        timerMinute = 0;
        timerSecond = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        scoreLabel.text = string.Format("Game Score: {0}", score);

        Timer();
    }


    public void GainPoint(int amount)
    {
        score += amount * multiplier;
    }

    public void Timer()
    {
        // Decrement the timer
        timerSecond += Time.deltaTime;
        if(timerSecond >= 60f)
        {
            timerMinute += 1;
            timerSecond = 0;
        }

        // Displaying the timer to the player 
        timerLabel.text = string.Format("{0:D2}:{1:F1}", timerMinute, timerSecond);
    }
}
