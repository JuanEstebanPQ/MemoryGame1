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
        isMouseDown = false;
        transform.localScale = originalScale;
        if (clickTime >= holdTime && gameController != null)
        {
            Debug.Log("sip");
            gameController.SendMessage(functionOnClick);
        }
        clickTime = 0.0f;
    }

    public void OnMouseExit()
    {
        ResetClickData();
        transform.localScale = originalScale;
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isMouseDown)
        {
            clickTime += Time.deltaTime;
            if (clickTime >= holdTime)
            {
                OnMouseUp(); // Llamar a OnMouseUp cuando se mantenga presionado durante 5 segundos
            }
        }
    }


    // Reinicia el clickTime
    void ResetClickData()
    {
        isMouseDown = false;
        clickTime = 0.0f;
    }
}
