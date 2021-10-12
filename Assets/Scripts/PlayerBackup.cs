using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBackup : MonoBehaviour
{
    internal Vector2 velocity;

    private Vector2 directionalInput;

    private float targetMaxSpeed = 10;
    private float acceleration = 1;
    private float friction = 0 ;
    private float stopThreshold = 0.1f;

    private void Update()
    {
        directionalInput.x = Input.GetKey(KeyCode.D) ? 1 : (Input.GetKey(KeyCode.A) ? -1 : 0);
        directionalInput.y = Input.GetKey(KeyCode.W) ? 1 : (Input.GetKey(KeyCode.S) ? -1 : 0);

        Vector2 nextTranslation = velocity;

        nextTranslation += directionalInput.normalized * acceleration * Time.deltaTime;

        if (velocity.magnitude < targetMaxSpeed)
        {

        }
        else
        {

        }


        
        if (velocity.magnitude > stopThreshold && directionalInput!=Vector2.zero)
        {
            nextTranslation = -directionalInput.normalized * friction;
        }
        else
        {
            velocity = Vector2.zero;
        }
        
        transform.position = (Vector2)transform.position + velocity;

    }

}
