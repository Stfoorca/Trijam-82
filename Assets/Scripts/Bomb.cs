using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private AudioManager audio;
    private GameManager gameManager;
    public float bombTime;
    public List<GameObject> objectsInTrigger = new List<GameObject>();
    public float boomPower = 2f;
    public GameObject explosionParticles;
    private void Start()
    {
        audio = AudioManager.instance;
        gameManager = GameManager.instance;
        bombTime = gameManager.bombDetonationTime;
    }

    private void Update()
    {
        if (bombTime > 0)
            bombTime -= Time.deltaTime;
        else
            Detonate();
    }

    public void Detonate()
    {
        GameObject explosion = Instantiate(explosionParticles, transform.position, Quaternion.identity);
        explosion.GetComponent<ParticleSystem>().Play();
        Destroy(explosion, 3f);
        audio.PlayBoomSFX();
        foreach(GameObject gameObject in objectsInTrigger)
        {
            Vector2 boomVector = new Vector2(gameObject.transform.position.x - transform.position.x,
                gameObject.transform.position.y - transform.position.y).normalized;
            gameObject.GetComponent<Rigidbody2D>().AddForce(boomVector * boomPower);
        }
        
        Destroy(this.gameObject);
        gameManager.bombPlanted = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
            objectsInTrigger.Add(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player") && objectsInTrigger.Contains(other.gameObject))
            objectsInTrigger.Remove(other.gameObject);
    }
}
