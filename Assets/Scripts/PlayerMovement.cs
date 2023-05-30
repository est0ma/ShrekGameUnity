using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController character;
    public float jumpForce = 2f;
    public float characterSpeed = 20f;
    public float gravity;

    private Vector3 movementDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float xMovement = Input.GetAxis("Horizontal");
        float zMovement = Input.GetAxis("Vertical");

        movementDirection = transform.right * xMovement + transform.forward * zMovement;

        if (character.isGrounded)
        {
            gravity = 0f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        else
        {
            gravity -= Time.deltaTime * 9.81f; // Utilise la gravité terrestre
        }

        movementDirection.y = gravity;

        character.Move(movementDirection * characterSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        gravity = jumpForce;
    }
}
