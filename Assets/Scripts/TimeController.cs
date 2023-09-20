using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    [SerializeField] int min, seg;
    [SerializeField] Text tiempo;

    [SerializeField] private LayerMask Wall; // Definimos la variable Wall
    [SerializeField] private float radioCirculo;

    private float restante;
    private bool enMarcha;

    private GameControllerScript gameController;

    private PlayerController playerController;



    private void Awake()
    {
        restante = (min * 60) + seg;
        enMarcha = true;
        gameController = FindObjectOfType<GameControllerScript>();

        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (enMarcha)
        {
            restante -= Time.deltaTime;
            if (restante < 1)
            {
                enMarcha = false;
                // VerificarColisionConCarta();
            }

            int tempMin = Mathf.FloorToInt(restante / 60);
            int tempSeg = Mathf.FloorToInt(restante % 60);
            tiempo.text = string.Format("{00:00}:{01:00}", tempMin, tempSeg);
        }

        if (!enMarcha)
        {

            // bool jugadorTocandoCarta = FindObjectOfType<PlayerController>().EstaTocandoCarta();

            // if (!jugadorTocandoCarta)
            // {
            //     StartCoroutine(playerController.ShowFailedMessage());
            // }

            gameController.RevealAllCards(); // Revelar todas las cartas

            // Cambiar tiempoEnMarcha a false en el PlayerController
            FindObjectOfType<PlayerController>().SetTiempoEnMarcha(false);
            FindObjectOfType<PlayerController>().SetMoviendo(false);

            MainImageScript[] allCards = FindObjectsOfType<MainImageScript>();
            foreach (MainImageScript card in allCards)
            {
                card.GetComponent<Collider2D>().enabled = true;
            }
        }
    }

    // void VerificarColisionConCarta()
    // {
    //     Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radioCirculo, Wall);

    //     // Si no hay colisiones con cartas al final del tiempo, muestra el mensaje de failed
    //     if (colliders.Length == 0)
    //     {
    //         StartCoroutine(playerController.ShowFailedMessage());
    //     }

    // }
}