using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MessageTimer : MonoBehaviour
{
    public GameObject succesMessage;
    public GameObject failedMessage;
    public GameObject finalMessage;
    public float timer = 60f; 

    void Update()
    {
        if (succesMessage.activeSelf || failedMessage.activeSelf || finalMessage.activeSelf)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                SceneManager.LoadScene("MenuScene");
            }
        }
    }
}
