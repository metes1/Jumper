using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    // Associated with this class and accessible from other classes
    public static GameController instance;
    // Title text object
    public GameObject titleTxt;
    // Game over text object
    public GameObject gameOverTxt;
    // Score text object
    public TMP_Text scoreTxt;
    public TMP_Text highScoreTxt;

    // Game state
    public bool titleScreen = true;
    public bool gameOver = false;
    // Speed of the screen scroll, scrolls to the left
    public float scrollSpeed;
    // Maximum speed
    public float maxSpeed;

    // Players score, based on elapsed time
    public float score;

    // Keep track of whether gravity is reversed
    public bool reversedGravity = false;

    private int highScore;
    private float pointUpPerSec;
    private float scoreUp;
    [SerializeField] private AudioSource bgMusic;

    // Awake called before start, make sure GameController is set up before anything else
    void Awake()
    {
        // Make sure no other instance of a game already exists
        if (instance == null)
        {
            // This is the current instance
            instance = this;

            // Get previous highscore from playerprefs and display
            if (PlayerPrefs.HasKey("HighScore"))
            {
                highScore = PlayerPrefs.GetInt("HighScore");
                highScoreTxt.text = "Best: " + highScore.ToString();
            }

            // Set beginning values
            score = 0f;
            scrollSpeed = -7f;
            maxSpeed = -10f;
            pointUpPerSec = 4f;
            scoreUp = 1.0f;
            
        } else if (instance != this)
        {
            // An instance already exists, destroy this one destroys itself
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // If player is on title screen and they press start, game starts
        if (titleScreen == true && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)))
        {
            // Remove title text and start scrolling screen
            titleTxt.SetActive(false);
            titleScreen = false;

            //start background music
            bgMusic.Play();
        }

        // If game state is game over and player either presses space or clicks left key, reload game
        if (gameOver == true && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)))
        {
            // Reload the current scene (the main scene)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // Score constantly increases as player stays alive
        if (gameOver == false && titleScreen == false)
        {
            scoreTxt.text = "Score: " + ((int)score).ToString();
            score += pointUpPerSec * Time.deltaTime;

            // Increase scroll speed after every 50 score points
            if (scrollSpeed > maxSpeed && score >= 50.0*scoreUp)
            {
                scrollSpeed -= 1.0f;
                scoreUp++;
                // Increase speed (pitch) of music
                bgMusic.pitch += 0.06f;
            }
        }

        // Speed up scroller based on current score

    }

    // Reversed gravity state switch
    public void ReverseGravity()
    {
        reversedGravity = !reversedGravity;
    }

    public void PlayerDied()
    {
        // If player score is higher than previous high score save it to playerprefs
        if ((int)score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", (int)score);
        }
        // Stop bg music
        bgMusic.Stop();
        // Enable game over text
        gameOverTxt.SetActive(true);
        // Set state of game to game over
        gameOver = true;
    }
}
