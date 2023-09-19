using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameControllerScript : MonoBehaviour
{
    public const int columns = 4;
    public const int rows = 4;


    public const float Xspace = 1;
    public const float Yspace = 1;

    private MainImageScript permanentRevealedCard;

    public int aciertos = 0;

    public int nivelActual = 1;
    public TextMeshProUGUI levelText;

    [SerializeField] private MainImageScript startObject;
    [SerializeField] private Sprite[] images;

    [SerializeField] private GameObject randomCardObjetive;
    [SerializeField] private GameObject randomCardBackground;

    public int maxCards = 3;



    private void Start()
    {

        int[] originalCards = new int[maxCards];
        for (int i = 0; i < maxCards; i++)
        {
            originalCards[i] = i;
        }

        maxCards = PlayerPrefs.GetInt("MaxCards", 3);
        aciertos = PlayerPrefs.GetInt("Aciertos", 0);

        nivelActual = PlayerPrefs.GetInt("NivelActual", 1);

        if (levelText != null)
        {
            levelText.text = "Nivel " + nivelActual;
        }

        int gridSize = 6; // Tamaño de la cuadrícula ( 6x6 o la cantidad que decida)
        int[] locations = RandomCards(gridSize * gridSize);

        Vector3 startPosition = startObject.transform.position;

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                MainImageScript gameImage;
                if (i == 0 && j == 0)
                {
                    gameImage = startObject;
                }
                else
                {
                    gameImage = Instantiate(startObject) as MainImageScript;
                }

                int index = j * gridSize + i;
                int id = locations[index];
                gameImage.ChangeSprite(id, images[id]);

                float positionX = (Xspace * i) + startPosition.x;
                float positionY = (Yspace * j) - startPosition.y;

                gameImage.transform.position = new Vector3(positionX, positionY, startPosition.z);
            }
        }

        StartCoroutine(ShowAllCardsBriefly(1.0f));
    }


    private int[] RandomCards(int count)
    {
        // Asegúrate de que count sea igual o mayor que maxCards
        if (count < maxCards)
        {
            count = maxCards;
        }

        int[] originalCards = new int[maxCards];
        for (int i = 0; i < maxCards; i++)
        {
            originalCards[i] = i;
        }

        int[] randomCards = new int[count];

        System.Random rng = new System.Random();
        for (int i = 0; i < count; i++)
        {
            int randomIndex = rng.Next(maxCards);
            randomCards[i] = originalCards[randomIndex];
        }

        return randomCards;
    }

    // private int[] RandomCards(int count)
    //     {
    //         int[] originalCards = { 0, 1, 2 }; // Numero de las imagenes
    //         int[] randomCards = new int[count];

    //         System.Random rng = new System.Random();
    //         for (int i = 0; i < count; i++)
    //         {
    //             int randomIndex = rng.Next(originalCards.Length);
    //             randomCards[i] = originalCards[randomIndex];
    //         }

    //         return randomCards;
    //     }

    private IEnumerator ShowAllCardsBriefly(float duration)
    {
        MainImageScript[] allCards = FindObjectsOfType<MainImageScript>();

        foreach (MainImageScript card in allCards)
        {
            card.Show();
            card.CloseHole();
        }

        yield return new WaitForSeconds(10.0f);

        foreach (MainImageScript card in allCards)
        {
            card.Close();
        }
        int randomIndex = Random.Range(0, allCards.Length);
        MainImageScript selectedCard = allCards[randomIndex];

        SpriteRenderer randomCardRenderer = randomCardObjetive.GetComponent<SpriteRenderer>();
        if (randomCardRenderer != null)
        {
            randomCardRenderer.sprite = selectedCard.GetComponent<SpriteRenderer>().sprite;

            randomCardBackground.SetActive(true);
        }

        permanentRevealedCard = selectedCard;
    }

    public MainImageScript PermanentRevealedCard
    {
        get { return permanentRevealedCard; }
    }

    public void RevealAllCards()
    {
        if (permanentRevealedCard != null)
        {
            MainImageScript[] allCards = FindObjectsOfType<MainImageScript>();

            foreach (MainImageScript card in allCards)
            {
                if (card.spriteId == permanentRevealedCard.spriteId)
                {
                    card.Show();
                }
                else
                {
                    card.Show();
                    card.ShowHole();
                }
            }
        }
    }


    public void Restart()
    {

        SceneManager.LoadScene("MainScene");
    }


    public void ReturnToMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void Empezar()
    {
        maxCards = 3;
        PlayerPrefs.SetInt("MaxCards", maxCards);
        PlayerPrefs.Save();

        aciertos = 0;
        PlayerPrefs.SetInt("Aciertos", aciertos);
        PlayerPrefs.Save();

        nivelActual = 1;
        PlayerPrefs.SetInt("NivelActual", nivelActual);
        PlayerPrefs.Save();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    // public void Jugar2()
    // {
    //     maxCards = 4;
    //     PlayerPrefs.SetInt("MaxCards", maxCards);
    //     PlayerPrefs.Save();

    //     aciertos = 2;
    //     PlayerPrefs.SetInt("Aciertos", aciertos);
    //     PlayerPrefs.Save();

    //     nivelActual = 2;
    //     PlayerPrefs.SetInt("NivelActual", nivelActual);
    //     PlayerPrefs.Save();

    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    // }

    // public void Jugar3()
    // {
    //     maxCards = 5;
    //     PlayerPrefs.SetInt("MaxCards", maxCards);
    //     PlayerPrefs.Save();

    //     aciertos = 4;
    //     PlayerPrefs.SetInt("Aciertos", aciertos);
    //     PlayerPrefs.Save();

    //     nivelActual = 3;
    //     PlayerPrefs.SetInt("NivelActual", nivelActual);
    //     PlayerPrefs.Save();

    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    // }
}
