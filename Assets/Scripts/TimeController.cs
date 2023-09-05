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
        if (enMarcha)
        {
            restante -= Time.deltaTime;
            if (restante < 1)
            {
            enMarcha = false;
            gameController.OnTimeUp(); // Llama a la función para verificar la carta al finalizar el tiempo
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