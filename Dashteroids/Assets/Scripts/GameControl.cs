using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    // vars
    public GameObject asteroidPrefab;
    public static float asteroidSpeed = 0.0025f;
    public static float collisionDampen = 0.6f;
    static int gameScore = 0;
    public ScoreCounter scCnt;
    static ScoreCounter scoreCounter;
    int creationDelay = 45;
    static int deathTimer = -1;
    public enum gameState { Title, Playing, Gameover, Scores };
    public static gameState currentState = gameState.Title;
    public GameObject gameOverMenu;
    public GameObject titleMenu;
    public GameObject highScoreMenu;
    static GameObject player;

    void Start(){
        scoreCounter = scCnt;
        player = GameObject.FindGameObjectWithTag("Player");
        player.SetActive(false);
    }

    // Update is called once per frame
    void Update(){
        if(currentState == gameState.Playing){
            // spawn asteroids
            if(creationDelay > 0){
                creationDelay -= 1;
            }else{
                spawnAsteroid();
                creationDelay = 135;
            }
            // player death
            if(deathTimer > 0){
                deathTimer -= 1;
            }else if(deathTimer == 0){
                deathTimer = -1;
                // enter the game over state
                currentState = gameState.Gameover;
                gameOverMenu.SetActive(true);
                highScoreMenu.GetComponent<HighScoreMenu>().logScore(gameScore);
                ScreenManip.screenShake(0.06f, 12);
                // clear out all asteroids
                GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
                for(var i=0; i<asteroids.Length; i++){
                    Destroy(asteroids[i]);
                }
            }
        }
    }

    void spawnAsteroid(){
        // place outside but along screen border
        int edge = ((int)Mathf.Round(Random.Range(0f, 3f)));
        Vector3 pos = Vector3.zero;
        if(edge == 0){
            // left edge
            pos += (Vector3.up * Random.Range(-(0.89f), 0.89f));
            pos += (Vector3.right * -(0.89f));
        }else if(edge == 1){
            // right edge
            pos += (Vector3.up * Random.Range(-0.89f, 0.89f));
            pos += (Vector3.right * 0.89f);
        }else if(edge == 2){
            // top edge
            pos += (Vector3.right * Random.Range(-0.89f, 0.89f));
            pos += (Vector3.up * 0.89f);
        }else{
            // bottom edge
            pos += (Vector3.right * Random.Range(-0.89f, 0.89f));
            pos += (Vector3.up * -0.89f);
        }
        GameObject asteroid = Instantiate(asteroidPrefab, pos, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
        // point to within 0.3 of center
        Vector3 target = ((Vector3.right * Random.Range(-0.3f, 0.3f)) + (Vector3.up * Random.Range(-0.3f, 0.3f)));
        Vector3 vel = (target - pos);
        vel.Normalize();
        vel *= asteroidSpeed;
        asteroid.GetComponent<AsteroidControl>().setVars((int)Random.Range(0f, 7f), Random.Range(-0.5f, 0.5f), 0, vel);
    }

    public static void tallyPoints(int pts){
        // add points to game total
        gameScore += pts;
        scoreCounter.WriteScore(gameScore);
    }

    public void startGame(){
        // starts the game anew
        gameScore = 0;
        creationDelay = 45;
        currentState = gameState.Playing;
        titleMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        player.transform.position = Vector3.zero;
        player.SetActive(true);
    }

    public void endGame(){
        // ends the game and returns to the title state
        currentState = gameState.Title;
        titleMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        highScoreMenu.SetActive(false);
    }

    public void showScores(){
        currentState = gameState.Scores;
        highScoreMenu.SetActive(true);
        titleMenu.SetActive(false);
        gameOverMenu.SetActive(false);
    }

    public void fullQuit(){
        Application.Quit();
    }

    public static void playerDie(int delay){
        deathTimer = delay;
        player.SetActive(false);
    }
}
