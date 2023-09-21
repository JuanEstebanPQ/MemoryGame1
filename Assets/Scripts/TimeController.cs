using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    [SerializeField] int min, seg;
    [SerializeField] Text tiempo;

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
            }

            int tempMin = Mathf.FloorToInt(restante / 60);
            int tempSeg = Mathf.FloorToInt(restante % 60);
            tiempo.text = string.Format("{00:00}:{01:00}", tempMin, tempSeg);
        }

        if (!enMarcha)
        {

            gameController.RevealAllCards(); // Revelar todas las cartas

            // Cambiar tiempoEnMarcha a false en el PlayerController
            FindObjectOfType<PlayerController>().SetTiempoEnMarcha(false);
            FindObjectOfType<PlayerController>().SetMoviendo(false);

            if (playerController.startArea != null)
            {
                Collider2D startAreaCollider = playerController.startArea.GetComponent<Collider2D>();
                if (startAreaCollider != null)
                {
                    startAreaCollider.enabled = true;
                }
            }

            MainImageScript[] allCards = FindObjectsOfType<MainImageScript>();
            foreach (MainImageScript card in allCards)
            {
                card.GetComponent<Collider2D>().enabled = true;
            }
        }
    }
}