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
    private int aciertos = 0;

    private Vector2 input;
    private GameControllerScript gameController;

    [SerializeField] private GameObject succesMessage;
    [SerializeField] private GameObject failedMessage;
    [SerializeField] private GameObject finalMessage;

    private float tiempoSinMovimiento = 0.0f;
    [SerializeField] private float tiempoLimiteSinMovimiento = 10.0f;
    [SerializeField] private GameObject mensajeDeAviso;
    private bool mensajeMostrado = false;

    private bool usandoTactil = false;

    // private bool tocandoCarta = false;

    private void Start()
    {

        puntoMovimiento = transform.position;
        gameController = FindObjectOfType<GameControllerScript>();
        animator = GetComponent<Animator>();
        aciertos = PlayerPrefs.GetInt("Aciertos", 0);

        usandoTactil = IsTouchDevice();
    }

    private bool IsTouchDevice()
    {
        return Input.touchSupported || Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
    }

    private void Update()
    {
        if (usandoTactil)
        {
            if (moviendo)
            {
                transform.position = Vector2.MoveTowards(transform.position, puntoMovimiento, velocidadMovimiento * Time.deltaTime);

                if (Vector2.Distance(transform.position, puntoMovimiento) == 0)
                {
                    moviendo = false;
                }
            }
            else if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                if (touch.phase == TouchPhase.Began)
                {

                    Vector2 puntoEvaluar = new Vector2(transform.position.x, transform.position.y) + offsetPuntoMovimiento + touchPosition;

                    if (!Physics2D.OverlapCircle(puntoEvaluar, radioCirculo, Wall))
                    {
                        moviendo = true;
                        puntoMovimiento = touchPosition;
                    }
                }
            }
        }
        else
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

        if (!moviendo)
        {
            tiempoSinMovimiento += Time.deltaTime;

            if (tiempoSinMovimiento >= tiempoLimiteSinMovimiento && !mensajeMostrado)
            {
                if (mensajeDeAviso != null)
                {
                    mensajeDeAviso.SetActive(true);
                    mensajeMostrado = true;
                }
            }
        }
        else
        {
            tiempoSinMovimiento = 0.0f;
            if (mensajeMostrado)
            {
                mensajeDeAviso.SetActive(false);
                mensajeMostrado = false;
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (tiempoEnMarcha) // Verifica si el tiempo está en marcha
        {
            return;
        }

        if (collider.CompareTag("Card"))
        {
            MainImageScript card = collider.GetComponent<MainImageScript>();
            if (card != null)
            {
                // tocandoCarta = true;

                // Comparar si la carta es igual a la permanente revelada
                if (card.spriteId == gameController.PermanentRevealedCard.spriteId)
                {
                    aciertos++;
                    PlayerPrefs.SetInt("Aciertos", aciertos);
                    PlayerPrefs.Save();
                    StartCoroutine(ShowSuccesMessage());

                    if (aciertos == 2)
                    {
                        gameController.maxCards = 4;
                        PlayerPrefs.SetInt("MaxCards", gameController.maxCards);
                        PlayerPrefs.Save();

                        gameController.nivelActual = 2;
                        PlayerPrefs.SetInt("NivelActual", gameController.nivelActual);
                        PlayerPrefs.Save();
                    }
                    if (aciertos == 4)
                    {
                        gameController.maxCards = 5;
                        PlayerPrefs.SetInt("MaxCards", gameController.maxCards);
                        PlayerPrefs.Save();

                        gameController.nivelActual = 3;
                        PlayerPrefs.SetInt("NivelActual", gameController.nivelActual);
                        PlayerPrefs.Save();
                    }
                    if (aciertos == 6)
                    {
                        gameController.maxCards = 6;
                        PlayerPrefs.SetInt("MaxCards", gameController.maxCards);
                        PlayerPrefs.Save();

                        gameController.nivelActual = 4;
                        PlayerPrefs.SetInt("NivelActual", gameController.nivelActual);
                        PlayerPrefs.Save();
                    }
                    if (aciertos == 8)
                    {
                        StartCoroutine(ShowFinalMessage());
                    }

                

                }
                else
                {
                    animator.Play("Death");
                    StartCoroutine(ShowFailedMessage());
                }
            }
        }

        else
        {
            StartCoroutine(ShowFailedMessage());
        }
    }

//     public bool EstaTocandoCarta()
// {
//     return tocandoCarta;
// }

    private void Death()
    {
        SpriteRenderer playerRenderer = GetComponent<SpriteRenderer>();
        if (playerRenderer != null)
        {
            playerRenderer.enabled = false;
        }
    }

    private IEnumerator ShowSuccesMessage()
    {
        yield return new WaitForSeconds(2f);


        succesMessage.SetActive(true);
    }

    public IEnumerator ShowFailedMessage()
    {
        yield return new WaitForSeconds(2f);

        failedMessage.SetActive(true);
    }

    private IEnumerator ShowFinalMessage()
    {
        yield return new WaitForSeconds(2f);

        finalMessage.SetActive(true);
    }


}