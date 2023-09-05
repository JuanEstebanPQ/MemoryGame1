using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour
{
    public const int columns = 4;
    public const int rows = 4;

    private MainImageScript playerStandingCard;

    public const float Xspace = 1.2f;
    public const float Yspace = 1.2f;

    private MainImageScript permanentRevealedCard;

    [SerializeField] private MainImageScript startObject;
    [SerializeField] private Sprite[] images;
    private int[] Randomiser(int[] locations)
    {
        int[] array = locations.Clone() as int[];
        for (int i = 0; i < array.Length; i++)
        {
            int newArray = array[i];
            int j = Random.Range(i, array.Length);
            array[i] = array[j];
            array[j] = newArray;
        }

        return array;
    }

    private void Start()
    {
        int gridSize = 6; // Tamaño de la cuadrícula (6x6)
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
        int[] originalCards = { 0, 1, 2 }; // Numero de las imagenes
        int[] randomCards = new int[count];

        System.Random rng = new System.Random();
        for (int i = 0; i < count; i++)
        {
            int randomIndex = rng.Next(originalCards.Length);
            randomCards[i] = originalCards[randomIndex];
        }

        return randomCards;
    }

    private IEnumerator ShowAllCardsBriefly(float duration)
    {
        foreach (MainImageScript card in FindObjectsOfType<MainImageScript>())
        {
            card.Show();
        }

        yield return new WaitForSeconds(duration);

        foreach (MainImageScript card in FindObjectsOfType<MainImageScript>())
        {
            card.Close();
        }

        int randomIndex = Random.Range(0, columns * rows);
        MainImageScript[] allCards = FindObjectsOfType<MainImageScript>();
        permanentRevealedCard = allCards[randomIndex];
        permanentRevealedCard.Show();
    }


    public MainImageScript PermanentRevealedCard
    {
        get { return permanentRevealedCard; }
    }

    private MainImageScript firstOpen;
    // private MainImageScript secondOpen;

    private int score = 0;
    private int attempts = 0;

    [SerializeField] private TextMesh scoreText;
    [SerializeField] private TextMesh attemptsText;

    private bool timeUp = false;

    // public bool canOpen
    // {
    //     get { return secondOpen == null; }
    // }

    // public void imageOpened(MainImageScript startObject)
    // {
    //     if (firstOpen == null)
    //     {
    //         firstOpen = startObject;

    //         // Evita que la misma carta se compare consigo misma
    //         if (firstOpen != permanentRevealedCard)
    //         {
    //             StartCoroutine(CheckGuessed());
    //         }
    //     }
    // }

    public void AddScore()
    {
        score++;
        scoreText.text = "Score: " + score;
    }

    public void OnTimeUp()
    {
        if (timeUp)
        {
            // Verifica si el jugador está tocando una carta
            if (playerStandingCard != null)
            {
                if (playerStandingCard.spriteId == permanentRevealedCard.spriteId)
                {
                    // La carta es igual a la permanente, suma puntos
                    AddScore();
                }
                else
                {
                    // La carta es diferente, puedes hacer algo aquí, como destruir la carta
                    Destroy(playerStandingCard.gameObject);
                }

                playerStandingCard = null; // Restablece la carta del jugador
            }
        }
    }

    // public void RevealAllCards()
    // {
    //     foreach (MainImageScript card in FindObjectsOfType<MainImageScript>())
    //     {
    //         card.Show();
    //     }
    // }

    public void RevealAllCards()
    {
        MainImageScript[] allCards = FindObjectsOfType<MainImageScript>();

        foreach (MainImageScript card in allCards)
        {
            if (card.spriteId == permanentRevealedCard.spriteId)
            {
                card.Show();
            }
        }
    }


    //Compara los 2 objetos
    private IEnumerator CheckGuessed()
    {
        if (firstOpen.spriteId == permanentRevealedCard.spriteId)
        {
            score++;
            scoreText.text = "Score: " + score;
        }
        else
        {
            yield return new WaitForSeconds(0.5f);

            firstOpen.Close();
        }

        attempts++;

        attemptsText.text = "Attempt: " + attempts;

        firstOpen = null;
    }

    public void Restart()
    {
        SceneManager.LoadScene("MainScene");
    }
}
