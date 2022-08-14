using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3, score = 0;

    [SerializeField] Text scoreText, livesText;

    [SerializeField] Image[] hearts;
    
    private void Awake() {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;

        if(numGameSessions > 1){
            Destroy(gameObject);
        }else{
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start(){
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }

    public void ProcessPlayerDeath(){
        if(playerLives > 1){
            TakeLife();
        }else{
            ResetGame();
        }
    }

    public void AddToScore(int value){
        score += value;
        scoreText.text = score.ToString();
    }

    public void AddToLive(int value){
        playerLives += value;
        if(playerLives >= 3){
            playerLives = 3;
        }
        livesText.text = playerLives.ToString();
    }    

    private void TakeLife(){
        playerLives --;
        livesText.text = playerLives.ToString();
    }

    private void ResetGame(){
        SceneManager.LoadScene(0);
    }
}
