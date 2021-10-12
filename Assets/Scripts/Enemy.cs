using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameManager gameManager;
    private AudioManager audio;
    private Vector2 move;
    private Rigidbody2D rb;
    private Player player;
    [SerializeField] private float moveSpeed = 1f;
    private float protectionTime;
    public bool isPlayerProtected;

    private bool alreadyProtected;
    // Start is called before the first frame update
    void Start()
    {
        audio = AudioManager.instance;
        gameManager = GameManager.instance;
        rb = GetComponent<Rigidbody2D>();
        player = gameManager.player;
        protectionTime = gameManager.spawnProtection;
        isPlayerProtected = true;
        GetComponent<SpriteRenderer>().sortingOrder = -1;
        StartCoroutine(YouAreProtected());
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameState != GameManager.GameState.PLAYING)
            return;
        if (protectionTime > 0)
            protectionTime -= Time.deltaTime;
        else
            isPlayerProtected = false;
        
        move = new Vector2(player.transform.position.x-transform.position.x, player.transform.position.y-transform.position.y).normalized;
    }

    IEnumerator YouAreProtected()
    {
        GetComponent<SpriteRenderer>().color = gameManager.protectedEnemiesColor;
        GetComponent<Collider2D>().enabled = false;
        alreadyProtected = true;
        yield return new WaitForSeconds(gameManager.spawnProtection);
        GetComponent<SpriteRenderer>().sortingOrder = 0;
        if (gameManager.gamePhase == GameManager.GamePhase.FLEEING)
            GetComponent<SpriteRenderer>().color = gameManager.enemiesChargingColor;
        GetComponent<Collider2D>().enabled = true;
    }
    void FixedUpdate()
    {
        if (gameManager.gameState != GameManager.GameState.PLAYING)
            return;
        if(gameManager.gamePhase == GameManager.GamePhase.FLEEING && !isPlayerProtected)
            rb.AddForce(move * moveSpeed * 100 * Time.fixedDeltaTime);
        else if(gameManager.gamePhase == GameManager.GamePhase.EATING && !isPlayerProtected)
            rb.AddForce(move * -moveSpeed/8 * 100 * Time.fixedDeltaTime);
    }

    public void Die(bool punkty_question_mark)
    {
        if (punkty_question_mark)
        {
            audio.PlayEnemyDeathSFX();
            gameManager.Points += gameManager.pointAmount;
        }

        gameManager.enemies.Remove(gameObject);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && gameManager.gamePhase == GameManager.GamePhase.FLEEING && !isPlayerProtected)
            other.gameObject.GetComponent<Player>().Die();
    }
}