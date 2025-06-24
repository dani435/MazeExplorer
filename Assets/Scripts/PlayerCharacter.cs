using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerCharacter : MonoBehaviour
{

    public CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        float speed = 5f;
        Vector2 velocity = new Vector2(0, 0);
  
        if (Input.GetKey(KeyCode.W))
        {
            velocity.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity.y -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            velocity.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity.x += 1;
        }

        velocity = velocity.normalized;

        Vector3 dir = new Vector3(velocity.x, 0f, velocity.y);
        Vector3 move = dir * Time.deltaTime * speed;
        controller.Move(move);
    }

    public float GetHeight()
    {
        return controller.height;
    }
}
