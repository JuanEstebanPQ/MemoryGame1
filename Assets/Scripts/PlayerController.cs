using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 5f;
    public Vector2 direction;

    Rigidbody2D rigidBody;

    private GameControllerScript gameController;

    public void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        gameController = FindObjectOfType<GameControllerScript>();
    }

    private void FixedUpdate()
    {
        rigidBody.velocity = direction * speed;
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Card")) // Asegúrate de que las cartas tengan el mismo tag
    {
        MainImageScript card = other.GetComponent<MainImageScript>();
        if (card != null)
        {
            // Comparar si la carta es igual a la permanente revelada
            if (card.spriteId == gameController.PermanentRevealedCard.spriteId)
            {
                // La carta es igual a la permanente, suma puntos o realiza acciones
                gameController.AddScore();
            }
            else
            {
                // La carta es diferente, puedes hacer algo aquí, como destruir la carta
                Destroy(other.gameObject);
            }
        }
    }
}

}