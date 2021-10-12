using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameManager gameManager;
    private AudioManager audio;
    private Vector2 move;
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        audio = AudioManager.instance;
        gameManager = GameManager.instance;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameState != GameManager.GameState.PLAYING)
            return;
        if (Input.GetKeyDown(KeyCode.Mouse0) && !gameManager.bombPlanted)
        {
            gameManager.bombPlanted = true;
            SpawnBomb();
        }
        move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    void FixedUpdate()
    {
        if (gameManager.gameState != GameManager.GameState.PLAYING)
            return;
        rb.AddForce(move * moveSpeed * 100 * Time.fixedDeltaTime);
    }

    void SpawnBomb()
    {
        audio.PlaySpawnBombSFX();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Instantiate(gameManager.bombPrefab, mousePos, Quaternion.identity);
    }

    public void Die()
    { 
        audio.PlayPlayerDeathSFX();
        gameManager.EndGame();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && gameManager.gamePhase == GameManager.GamePhase.EATING)
        {
            other.gameObject.GetComponent<Enemy>().Die(true);
        }
    }
}
