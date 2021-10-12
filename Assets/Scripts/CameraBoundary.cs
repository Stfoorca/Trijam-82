using UnityEngine;

public class CameraBoundary : MonoBehaviour
{
    private GameManager gameManager;
    private Vector2 screenBounds;
    private float playerWidth;
    private float playerHeight;
    public float screenRatio;
    public bool removeVelocity;
    private void Start()
    {
        gameManager = GameManager.instance;
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width*screenRatio, Screen.height*screenRatio));
        playerWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        playerHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }

    void LateUpdate()
    {
        Vector2 viewPos = transform.position;
        if (removeVelocity && gameManager.gamePhase == GameManager.GamePhase.FLEEING)
        {
            if (transform.position.x <= -screenBounds.x || transform.position.x >= screenBounds.x ||
                transform.position.y <= -screenBounds.y ||
                transform.position.y >= screenBounds.y)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Enemy>().Die(false);
            }
        }
        else if (!removeVelocity || gameManager.gamePhase == GameManager.GamePhase.EATING)
        {
            viewPos.x = Mathf.Clamp(viewPos.x, -screenBounds.x + playerWidth, screenBounds.x - playerWidth);
            viewPos.y = Mathf.Clamp(viewPos.y, -screenBounds.y + playerHeight, screenBounds.y - playerHeight);
        }

        transform.position = viewPos;
        
    }
}
