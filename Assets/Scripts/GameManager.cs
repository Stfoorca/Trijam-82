using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        PLAYING,
        PAUSED,
        END
    }
    public enum GamePhase
    {
        FLEEING,
        EATING
    }
    
    #region Singleton
    public static GameManager instance;

    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }
    #endregion

    [Header("GameState variables")] 
    public int points;
    public int Points
    {
        get { return points; }
        set
        {
            points = value;
            pointsUI.text = points.ToString();
        }
    }
    public GameState gameState;
    public GamePhase gamePhase;
    public float phaseTime;
    public bool bombPlanted;
    private float startTime;
    public int pointAmount;
    [Header("Phase variables")]
    public int phaseDuration;

    [Header("Enemy variables")]
    public int enemiesNumToSpawn;
    public int spawnInterval;
    private float spawnTime;
    public GameObject enemyPrefab;
    public float spawnProtection;
    
    [Header("GameObjects variables")]
    public List<GameObject> enemies = new List<GameObject>();
    public Player player;
    public GameObject bombPrefab;
    public float bombDetonationTime;

    [Header("UI variables")] 
    public Text timeUI;
    public Text pointsUI;
    public GameObject pausePanel;
    public GameObject endGamePanel;
    public Text endTimeUI;
    public Text endPointsUI;
    
    [Header("References")] 
    public AudioManager audio;

    [Header("Colors")] 
    public Color32 timeUIChargingColor;
    public Color32 timeUIFleeingColor;
    public Color32 protectedEnemiesColor;
    public Color32 enemiesChargingColor;
    public Color32 enemiesFleeingColor;
    void Start()
    {
        audio = AudioManager.instance;
        audio.PlayBackgroundMusic();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        gameState = GameState.PLAYING;
        gamePhase = GamePhase.FLEEING;
        startTime = Time.time;
        bombPlanted = false;
        timeUI.color = timeUIChargingColor;
        pointAmount = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
        
        if (gameState == GameState.PLAYING)
        {
            timeUI.text = phaseTime.ToString("0.##");
            if (phaseTime > 0)
                phaseTime -= Time.deltaTime;
            else
            {
                if (gamePhase == GamePhase.EATING)
                {
                    audio.PlayFleeingPhaseClipSFX();
                    timeUI.color = timeUIChargingColor;
                    foreach (GameObject enemy in enemies)
                    {

                        enemy.GetComponent<SpriteRenderer>().color = enemiesChargingColor;
                    }
                    gamePhase = GamePhase.FLEEING;
                    pointAmount += 1;
                }
                else
                {
                    audio.PlayEatingPhaseClipSFX();
                    timeUI.color = timeUIFleeingColor;
                    foreach (GameObject enemy in enemies)
                    {
                        enemy.GetComponent<SpriteRenderer>().color = enemiesFleeingColor;
                    }
                    gamePhase = GamePhase.EATING;
                }
                phaseTime = phaseDuration;
            }

            if (gamePhase != GamePhase.FLEEING)
                return;
            
            if (spawnTime > 0)
                spawnTime -= Time.deltaTime;
            else
            {
                spawnTime = spawnInterval;
                SpawnEnemies();
            }
        }
    }

    void PauseGame()
    {
        if (gameState == GameState.PLAYING && !pausePanel.activeSelf)
        {
            pausePanel.SetActive(true);
            gameState = GameState.PAUSED;
            Time.timeScale = 0;
        }
        else if (gameState == GameState.PAUSED && pausePanel.activeSelf)
        {
            pausePanel.SetActive(false);
            gameState = GameState.PLAYING;
            Time.timeScale = 1;
        }
    }
    private void SpawnEnemies()
    {
        audio.PlaySpawnEnemySFX();
        for (int i = 0; i < enemiesNumToSpawn; i++)
        {
            Vector2 randomSpawnPosition = new Vector2(Random.Range(-2.5f, 2.5f), Random.Range(-1.2f, 1.2f));
            GameObject enemy = Instantiate(enemyPrefab, randomSpawnPosition, Quaternion.identity);
            enemies.Add(enemy);
        }
    }

    public void EndGame()
    {
        
        gameState = GameState.END;
        pausePanel.SetActive(false);
        endPointsUI.text = pointsUI.text;
        endTimeUI.text = Math.Round(Time.time - startTime, 2).ToString();
        
        endGamePanel.SetActive(true);
        enemies.Clear();
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            Destroy(enemy);
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Explosion"))
            Destroy(enemy);
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Bomb"))
            Destroy(enemy);
        Time.timeScale = 0;
        
    }
    public void ResetGame()
    {
        Time.timeScale = 1;
        startTime = Time.time;
        endGamePanel.SetActive(false);
        pausePanel.SetActive(false);
        
        gameState = GameState.PLAYING;
        gamePhase = GamePhase.FLEEING;
        
        phaseTime = phaseDuration;
        spawnTime = spawnInterval;
        
        Points = 0;
        pointAmount = 1;

        bombPlanted = false;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.transform.position = new Vector2(0f, 0f);
    }
}
