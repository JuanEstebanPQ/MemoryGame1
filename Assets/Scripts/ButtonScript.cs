using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] private GameControllerScript gameController;
    [SerializeField] private string functionOnClick;
    private bool isMouseDown = false;
    private float clickTime = 0.0f;
    private float holdTime = 5.0f;
    private Vector3 originalScale;

    private void Start()
    {
        // Almacenar la escala original del botón.
        originalScale = transform.localScale;
    }

    public void OnMouseDown()
    {
        isMouseDown = true;
        transform.localScale = originalScale * 0.8f;
    }

    public void OnMouseUp()
    {
        // Si se levanta el clic antes de los 5 segundos, cancela la acción.
        if (clickTime < holdTime)
        {
            ResetClickData();
            return;
        }

        // Si se mantuvo presionado durante 5 segundos o más, realiza la acción.
        if (gameController != null)
        {
            gameController.SendMessage(functionOnClick);
        }

        ResetClickData();
        transform.localScale = originalScale;
    }

    public void OnMouseExit()
    {
        // Si el mouse sale del área, cancela la acción y restablece la escala.
        ResetClickData();
        transform.localScale = originalScale;
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isMouseDown)
        {
            clickTime += Time.deltaTime;
        }
    }


    // Reinicia el clickTime
    void ResetClickData()
    {
        isMouseDown = false;
        clickTime = 0.0f;
    }
}
