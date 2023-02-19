using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    // Player score
    public int score = 0;
    private int highScore = 0;

    public float restartDelay = 1f;
    [SerializeField]private bool gameHasEnded = false;
    public GameObject completeLevelUI;
    public PlayerController playerController;
    public GameObject compoleteLevelUI;
    #region Singleton
    // Static instance of the Game Manager,
    // can be access from anywhere

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    #endregion
    public void CompleteLevel()
    {
        Debug.Log("Level completed!");
        completeLevelUI.SetActive(true);
        playerController.enabled = false;
    }

    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("Game Over!");
            Invoke("RestartGame", restartDelay); // delay restart
        }

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Restarted");
    }


    // Increase score
    public void IncreaseScore(int amount)
    {
        // Increase the score by the given amount
        score += amount;
        // Show the new score in the console
        //print("New Score: " + score.ToString());

        if (score > highScore)
        {
            highScore = score;
            //print("New high score: " + highScore);
        }
    }
}