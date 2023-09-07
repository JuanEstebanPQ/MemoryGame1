using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainImageScript : MonoBehaviour
{
    [SerializeField] private GameObject Unknow;
    [SerializeField] private GameObject Hole;

    private int _spriteId;
    public int spriteId
    {
        get { return _spriteId; }
    }

    public void ChangeSprite(int id, Sprite image)
    {
        _spriteId = id;
        GetComponent<SpriteRenderer>().sprite = image;
    }

    public void Close()
    {
        Unknow.SetActive(true); //Esconder imagen
    }

    public void Show()
    {
        Unknow.SetActive(false);
    }

    public void CloseHole()
    {
        Hole.SetActive(false);
    }

    public void ShowHole()
    {
        Hole.SetActive(true);
    }

}
