using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MessageTimer : MonoBehaviour
{
    public GameObject succesMessage;
    public GameObject failedMessage;
    public GameObject finalMessage;
    public GameObject tutorialMessage;
    public GameObject alertMessage;
    public float timer = 60f;
    public float alertTimer = 10f;

    void Update()
    {
        if (succesMessage.activeSelf || failedMessage.activeSelf || finalMessage.activeSelf || tutorialMessage.activeSelf)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                SceneManager.LoadScene("MenuScene");
            }
        }
        if (alertMessage.activeSelf)
        {
            alertTimer -= Time.deltaTime;
            if (alertTimer <= 0f)
            {
                alertMessage.SetActive(false);
            }
        }
    }
}
