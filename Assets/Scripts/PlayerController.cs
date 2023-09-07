using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private Vector2 puntoMovimiento;

    [SerializeField] private Vector2 offsetPuntoMovimiento;
    [SerializeField] private LayerMask Wall;
    [SerializeField] private float radioCirculo;

    Animator animator;

    private bool tiempoEnMarcha = true;

    private bool moviendo = false;

    private Vector2 input;
    private GameControllerScript gameController;

    private void Start()
    {

        puntoMovimiento = transform.position;
        gameController = FindObjectOfType<GameControllerScript>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        if (moviendo)
        {
            transform.position = Vector2.MoveTowards(transform.position, puntoMovimiento, velocidadMovimiento * Time.deltaTime);

            if (Vector2.Distance(transform.position, puntoMovimiento) == 0)
            {
                moviendo = false;
            }
        }

        if ((input.x != 0 || input.y != 0) && !moviendo)
        {
            Vector2 puntoEvaluar = new Vector2(transform.position.x, transform.position.y) + offsetPuntoMovimiento + input;

            if (!Physics2D.OverlapCircle(puntoEvaluar, radioCirculo, Wall))
            {
                moviendo = true;
                puntoMovimiento += input;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(puntoMovimiento + offsetPuntoMovimiento, radioCirculo);
    }

    public void SetTiempoEnMarcha(bool value)
    {
        tiempoEnMarcha = value;
    }

    public void SetMoviendo(bool value)
    {
        moviendo = value;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (tiempoEnMarcha) // Verifica si el tiempo está en marcha
        {
            return; // No realices ninguna acción si el tiempo está en marcha
        }

        if (other.CompareTag("Card"))
        {
            MainImageScript card = other.GetComponent<MainImageScript>();
            if (card != null)
            {
                // Comparar si la carta es igual a la permanente revelada
                if (card.spriteId == gameController.PermanentRevealedCard.spriteId)
                {
                    Debug.Log("El jugador tocó una carta correcta.");
                }
                else
                {
                    animator.Play("Death");
                }

            }
        }

    }

    private void Death()
    {
        Destroy(gameObject);
    }

    //     public float speed = 5f;
    //     public Vector2 direction;

    //     Rigidbody2D rigidBody;

    //     private GameControllerScript gameController;

    //     public void Start()
    //     {
    //         rigidBody = GetComponent<Rigidbody2D>();
    //         gameController = FindObjectOfType<GameControllerScript>();
    //     }

    //     private void FixedUpdate()
    //     {
    //         rigidBody.velocity = direction * speed;
    //     }

    //     private void Update()
    //     {
    //         Movement();
    //     }

    //     private void Movement()
    //     {
    //         direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    //     }

    //     private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Card")) // Asegúrate de que las cartas tengan el mismo tag
    //     {
    //         MainImageScript card = other.GetComponent<MainImageScript>();
    //         if (card != null)
    //         {
    //             // Comparar si la carta es igual a la permanente revelada
    //             if (card.spriteId == gameController.PermanentRevealedCard.spriteId)
    //             {
    //                 // La carta es igual a la permanente, suma puntos o realiza acciones
    //                 gameController.AddScore();
    //             }
    //             else
    //             {
    //                 // La carta es diferente, puedes hacer algo aquí, como destruir la carta
    //                 Destroy(other.gameObject);
    //             }
    //         }
    //     }
    // }

}