using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest_ : MonoBehaviour
{
    GameController gameController;
    Animator anm;
    public GameObject gift;
    public BoxCollider2D idle, open;
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        anm = GetComponent<Animator>();
        Invoke("setGift", 0.2f);
    }
    public void getChoice(bool sta)
    {
        if (sta)
        {
            if (gift)
                gift.SetActive(true);
            anm.SetBool("isOn", true);
            idle.enabled = false;
            open.enabled = true;
        }
        else
        {
            if (gift)
                gift.SetActive(false);
            anm.SetBool("isOn", false);
            open.enabled = false;
            idle.enabled = true;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameController.getChest(gameObject);
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameController.getChest(null);
        }
    }
    void setGift()
    {
        gift.SetActive(false);
    }    
}
