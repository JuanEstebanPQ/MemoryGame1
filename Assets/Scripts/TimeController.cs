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

    private void Awake()
    {
        restante = (min * 60) + seg;
        enMarcha = true;
        gameController = FindObjectOfType<GameControllerScript>();
    }

    void Update()
    {
        if (enMarcha) // Asegúrate de que el tiempo no haya terminado
        {
            restante -= Time.deltaTime;
            if (restante < 1)
            {
                enMarcha = false;
                // El tiempo ha llegado a 00:00, no necesitas marcar isTimeUp aquí
            }

            int tempMin = Mathf.FloorToInt(restante / 60);
            int tempSeg = Mathf.FloorToInt(restante % 60);
            tiempo.text = string.Format("{00:00}:{01:00}", tempMin, tempSeg);
        }

        if (!enMarcha)
        {
            gameController.RevealAllCards(); // Llama al método en el GameController para revelar todas las cartas
        }
    }
}